using Common;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.CustomMapping;
using Services.Dto;
using Services.Services.Utilities;
using WebFramework;
using WebFramework.Configuration;
using WebFramework.Middlewares;

namespace Web
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;
        public IConfiguration Configuration { get; }

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
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IPenaltyService, PenaltyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddMvc();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseStatusCodePagesWithRedirects("/Error/Index/{0}");
            app.UseCustomExceptionHandler();
            // app.UseHsts(env);
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            //For Area
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "Admin",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

        }
    }
}