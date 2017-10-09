using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using WebApplicationOwin.Provider;

namespace OwinAuthServer
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public Startup()
        {
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                //sets the URL by which the client will receive the token
                TokenEndpointPath = new PathString("/token"),
                Provider = new OAuthAppProvider(),

                //It indicates the route by which the user will be redirected for authorization.
                AuthorizeEndpointPath = new PathString("/api/User/LogIn"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
                AllowInsecureHttp = true
            };
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
