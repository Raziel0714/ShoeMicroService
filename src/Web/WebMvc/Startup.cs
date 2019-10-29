using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShoesOnContainers.Web.WebMvc;
using ShoesOnContainers.Web.WebMvc.Infrastructure;
using ShoesOnContainers.Web.WebMvc.Services;

namespace WebMvc
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
            services.Configure<AppSettings>(Configuration);
            services.AddSingleton<IHttpClient, CustomHttpClient>();
            services.AddTransient<ICatalogService, CatalogService>();

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var callBackUrl = Configuration.GetValue<string>("CallBackUrl");
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(option =>
            {
                option.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.Authority = identityUrl.ToString();
                option.SignedOutRedirectUri = callBackUrl.ToString();
                option.ClientId = "mvc";
                option.ClientSecret = "secret";
                option.ResponseType = "code id_token";
                option.SaveTokens = true;
                option.GetClaimsFromUserInfoEndpoint = true;
                option.RequireHttpsMetadata = false;
                option.Scope.Add("openid");
                option.Scope.Add("profile");
                option.Scope.Add("offline_access");
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });


            services.AddMvc();
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
                app.UseExceptionHandler("/Catalog/Error");
                
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Catalog}/{action=Index}/{id?}");
            });
        }
    }
}
