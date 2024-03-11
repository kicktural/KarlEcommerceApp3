using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;



namespace WebUI
{
    public class Startup
    {

        public IConfiguration configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

			// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

			services.AddAuthentication()
           .AddFacebook(options =>
           {
               options.AppId = configuration["Authentication:Facebook:AppId"];
               options.AppSecret = configuration["Authentication:Facebook:AppSecret"];

		   });

           
		}


        
    }
}
