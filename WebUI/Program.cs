using Business.DependencyResolver;
using DataAccess.Concreate.SQLServer;
using Entities.Concreate;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using WebUI.Helperservices;
using WebUI.OptionsModel;
using WebUI.Resource;
using WebUI.Services;
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



builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
	opt.TokenLifespan = TimeSpan.FromHours(2);
});


builder.Services.AddDefaultIdentity<User>().AddRoles<AppRole>()
	.AddEntityFrameworkStores<AppDbContext>();

//builder.Services.ConfigureApplicationCookie(option =>
//{
//    option.LoginPath = "/Auth/Login";
//});




builder.Services.AddAuthentication().AddFacebook(options =>
{
	options.AppId = builder.Configuration["Authentication:Facebook:AppID"];
	options.AppSecret = builder.Configuration["Authentication:Facebook:Appsecret"];
	options.CallbackPath = new PathString("/signin/facebook");
}).AddGoogle(opts =>
{
	opts.ClientId = builder.Configuration["Authentication:Google:ClientID"];
    opts.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
	opts.CallbackPath = new PathString("/signin/google");
}); 




builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();

var cookieBuilder = new CookieBuilder();

builder.Services.ConfigureApplicationCookie(opt =>
{
	cookieBuilder.Name = "AppCookie";

	opt.LoginPath = new PathString("/Auth/Login");

	opt.Cookie = cookieBuilder;

	opt.ExpireTimeSpan = TimeSpan.FromDays(90);

	opt.SlidingExpiration = true;
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
