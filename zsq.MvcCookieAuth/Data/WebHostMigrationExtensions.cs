using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace zsq.MvcCookieAuth.Data
{
    public static class WebHostMigrationExtensions
    {
        public static IWebHost MigrateDbContext<TContext>(this IWebHost host, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    context.Database.Migrate();
                    //由外部决定调用内容
                    seeder(context, services);

                    logger.LogInformation($"执行DBContext{ typeof(TContext).Name } seed执行成功");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"执行DBContext { typeof(TContext).Name } seed方法失败");
                }
            }

            return host;
        }
    }
}