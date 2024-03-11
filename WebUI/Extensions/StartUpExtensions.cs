using AutoMapper;
using Business.Abstract;
using Business.AutoMapper;
using Business.Concreate;
using Core.Entities.Concreate.EntityFremawork;
using DataAccess.Abstract;
using DataAccess.Concreate.SQLServer;
using Entities.Concreate;
using Microsoft.AspNetCore.Identity;
using WebUI.CustomValidations;
using WebUI.Localizations;

namespace WebUI.Extensions
{
    public static class StartUpExtensions 
    {

        public static void AddIdentityWithExt(this IServiceCollection services)
        {



            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true; //Login olan E-Posta ile ayni E-Posta ile Login olamaz.

                options.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklmnbvcxz1234567890_QWERTYUIOPASDFGHJKLZXCVBNM"; //kullanici adi izin verilen karakterler.

                options.Password.RequiredLength = 6;

                options.Password.RequireNonAlphanumeric = false;

                options.Password.RequireLowercase = true;

                options.Password.RequireUppercase = true;

                options.Password.RequireDigit = false;

                options.Lockout.MaxFailedAccessAttempts = 3;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

            }).AddPasswordValidator<PasswordVailidator>().AddDefaultTokenProviders().AddUserValidator<UserValidator>
            ().AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();
        }
       
    }
}
