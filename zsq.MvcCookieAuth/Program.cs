using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using zsq.MvcCookieAuth.Data;

namespace zsq.MvcCookieAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                // .MigrateDbContext<ApplicationDbContext>((context, services) =>
                // {
                //     new ApplicationDbContextSeed().SeedAsync(context, services).Wait();
                // })
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Environment.CurrentDirectory)
                                    .AddJsonFile("host.json")
                                    .Build();

            //var url = configuration["urls"];

            return WebHost.CreateDefaultBuilder(args)
                          .UseConfiguration(configuration)
                          //.UseUrls(url)
                          .UseStartup<Startup>();
        }
    }
}
