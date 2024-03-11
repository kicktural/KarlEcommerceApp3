using AutoMapper;
using Business.Abstract;
using Business.AutoMapper;
using Business.Concreate;
using Core.Entities.Concreate.EntityFremawork;
using DataAccess.Abstract;
using DataAccess.Concreate.SQLServer;
using Entities.Concreate;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolver
{
    public static class ServiceRegistiration
    {

        public static void Run(this IServiceCollection Services)
        {
            Services.AddScoped<AppDbContext>();

            Services.AddScoped<ICategoryService, CategoryManager>();
            Services.AddScoped<ICategoryDAL, EFCategoryDAL>();

            Services.AddScoped<IProductService, ProductManager>();
            Services.AddScoped<IProductDAL, EFProductDAL>();

            Services.AddScoped<IPictureService, PictureManager>();
            Services.AddScoped<IPictureDAL, EFPictureDAL>();

            Services.AddScoped<IOrderServices, OrderManager>();
            Services.AddScoped<IOrderDAL, EFOrderDAL>();

            Services.AddScoped<IPictureService, PictureManager>();
            Services.AddScoped<IPictureDAL, EFPictureDAL>();

            Services.AddScoped<IUserService, UserManager>();
            Services.AddScoped<IUserDAL, EFUserDAL>();

            Services.Configure<IdentityOptions>(options =>
            {
                //options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);
            });


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            Services.AddSingleton(mapper);
        }
    }
}
