using Microsoft.Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using Saiot.WebRole.ClientApi.App_Start;
using System.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(Saiot.WebRole.ClientApi.Startup))]
namespace Saiot.WebRole.ClientApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            ConfigureAuth(app);
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    TokenValidationParameters = new TokenValidationParameters() {
                        ValidAudience = ConfigurationManager.AppSettings["Audience"]
                    },
                    Tenant = ConfigurationManager.AppSettings["Tenant"]
                });
        }
    }
}