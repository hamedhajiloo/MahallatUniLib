using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using Common;
using Microsoft.AspNetCore.Identity;
using Data;

namespace WebFramework.Configuration
{
    public static class IdentityConfigurationExtensions
    {
        public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
        {
            services.AddIdentity<User, Role>(identityOptions =>
            {
                #region Password Settings

                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequiredLength = 3;
                identityOptions.Password.RequireNonAlphanumeric = false; //#@!
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireLowercase = false;

                #endregion

                #region Username Settings
                identityOptions.User.RequireUniqueEmail = false;
                #endregion

                #region Signin Sttings
                identityOptions.SignIn.RequireConfirmedEmail = false;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = false;
                #endregion

                #region Lockout Settings(we do'nt use this beacuse we use token and not use cookie)

                identityOptions.Lockout.MaxFailedAccessAttempts = 15;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                identityOptions.Lockout.AllowedForNewUsers = false;
                #endregion

            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
