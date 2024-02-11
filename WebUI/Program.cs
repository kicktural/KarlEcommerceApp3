using Business.Abstract;
using Business.Concreate;
using DataAccess.Abstract;
using DataAccess.Concreate.SQLServer;
using Business.DependencyResolver;
using Entities.Concreate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.CookiePolicy;
using WebUI.Services;
using System.Reflection;
using static WebUI.Services.LanguageService;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using WebUI.Resource;
var builder = WebApplication.CreateBuilder(args);



#region Localizer
builder.Services.AddSingleton<LanguageService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization(options => 
options.DataAnnotationLocalizerProvider = (type, factory) =>
{
    var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
    return factory.Create(nameof(SharedResource), assemblyName.Name);
});
#endregion


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var SupportCultures = new List<CultureInfo>
    {
        new CultureInfo("en-En"),
        new CultureInfo("ru-RU"),
        new CultureInfo("az-Az")
    };
    options.DefaultRequestCulture = new RequestCulture(culture: "en-EN", uiCulture: "en-EN");
    options.SupportedCultures = SupportCultures;
    options.SupportedUICultures = SupportCultures;
    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();




builder.Services.AddDefaultIdentity<User>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Auth/Login";
});



builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.MinimumSameSitePolicy = SameSiteMode.None;
	options.HttpOnly = HttpOnlyPolicy.Always; // HttpOnly özelli?ini her zaman etkinle?tir
	options.Secure = CookieSecurePolicy.Always; // Çerezlerin sadece güvenli ba?lant?larda iletilmesini zorunlu k?l
});

builder.Services.Run();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
 
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
