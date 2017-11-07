using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using WebApplicationOwin.Provider;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Serializer;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

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
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
                AllowInsecureHttp = true              
            };
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
