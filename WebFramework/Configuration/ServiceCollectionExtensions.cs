using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace WebFramework
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlServer(configuration.GetConnectionString("SqlServer"))
                    //Tips
                    .ConfigureWarnings(warning => warning.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });
        }

        public static void AddMinimalMvc(this IServiceCollection services,HttpContext context)
        {
            //https://github.com/aspnet/Mvc/blob/release/2.2/src/Microsoft.AspNetCore.Mvc/MvcServiceCollectionExtensions.cs
            services.AddMvcCore(options =>
            {
                //options.ModelValidatorProviders.Insert(0, new DataAnnotationsModelValidatorProvider());
                options.Filters.Add(new AuthorizeFilter());

                //Like [ValidateAntiforgeryToken] attribute but dose not validatie for GET and HEAD http method
                //You can ingore validate by using [IgnoreAntiforgeryToken] attribute
                //Use this filter when use cookie 
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                //options.UseYeKeModelBinder();
            })
            .AddApiExplorer()
            .AddAuthorization()
            .AddFormatterMappings()
            .AddDataAnnotations()
            .AddJsonFormatters(/*options =>
            {
                options.Formatting = Newtonsoft.Json.Formatting.Indented;
                options.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }*/)
            .AddCors()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

    }
}