using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace zsq.OidcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = "http://localhost:5002";
                options.RequireHttpsMetadata = false;
                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.SaveTokens = true;

                // 设为true会获取到所有信息
                // options.GetClaimsFromUserInfoEndpoint = true;

                // 以下设置均为报异常
                /*
                    Error.
                    An error occurred while processing your request.
                    Request ID: 0HLJJQR05RCSJ:00000001

                    Development Mode
                    Swapping to Development environment will display more detailed information about the error that occurred.

                    Development environment should not be enabled in deployed applications, 
                    as it can result in sensitive information from exceptions being displayed to end users. 
                    For local debugging, 
                    development environment can be enabled by setting the ASPNETCORE_ENVIRONMENT environment variable to Development, and restarting the application.
                 */
                // options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                // options.ClaimActions.MapJsonKey("sub", "sub");
                // options.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
                // options.ClaimActions.MapJsonKey("avatar", "avatar");
                // options.ClaimActions.MapCustomJson("sub", j => j["role"].ToString());

                // options.Scope.Add("openid");
                // options.Scope.Add("profile");
                // options.Scope.Add("offline_access");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}