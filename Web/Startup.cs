using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.CustomMapping;
using System;
using System.Text;
using WebFramework;
using WebFramework.Configuration;
using WebFramework.Middlewares;

namespace Web
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;
        public IConfiguration Configuration { get; }
        //private static readonly byte[] _homePayload = Encoding.UTF8.GetBytes("Endpoint Routing sample endpoints:" + Environment.NewLine + "/plaintext");


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AutoMapperConfiguration.InitializeAutoMapper();
            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));
            services.AddDbContext(Configuration);
            services.AddCustomIdentity(_siteSetting.IdentitySettings);
            services.AddMyServicesAndRepositories();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpContextAccessor();
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddJwtAuthentication(_siteSetting.JwtSettings);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseWhen(c => c.Request.Path.StartsWithSegments("/api"), conf =>
            {
                conf.UseCustomExceptionHandler();
                conf.UseMvc();
            });

            app.UseWhen(c => !c.Request.Path.StartsWithSegments("/api"), conf =>
            {
                if (env.IsDevelopment()) conf.UseDeveloperExceptionPage();
                else { conf.UseStatusCodePagesWithReExecute("/Error/{0}"); }
                conf.UseMvc();
            });

            // app.UseHsts(env);
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}