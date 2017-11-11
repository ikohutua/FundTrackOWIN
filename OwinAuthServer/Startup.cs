using Microsoft.Owin;
using Owin;
using System.Web.Http;
using WebApplicationOwin;

[assembly: OwinStartup(typeof(OwinAuthServer.Startup))]

namespace OwinAuthServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureAuth(app);
            
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}
