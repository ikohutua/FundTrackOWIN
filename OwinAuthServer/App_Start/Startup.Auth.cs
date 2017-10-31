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
                AllowInsecureHttp = true,
                AccessTokenFormat = new SecureTokenFormatter()
            };
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }

    public class SecureTokenFormatter : ISecureDataFormat<AuthenticationTicket>
    {
        private TicketSerializer serializer;

        public SecureTokenFormatter()
        {
            serializer = new TicketSerializer();
        }

        public string Protect(AuthenticationTicket ticket)
        {
            var ticketData = serializer.Serialize(ticket);
            var protectedString = Convert.ToBase64String(ticketData);
            return protectedString;
        }

        public AuthenticationTicket Unprotect(string text)
        {
            var protectedData = Convert.FromBase64String(text);
            var ticket = serializer.Deserialize(protectedData);
            return ticket;
        }
    }

    public class InterceptResponseMiddleware
    {
        private readonly AppFunc next;

        public InterceptResponseMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            IOwinContext context = new OwinContext(env);

            // Buffer the response
            var stream = context.Response.Body;
            var buffer = new MemoryStream();
            context.Response.Body = buffer;

            await next(env);

            buffer.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(buffer);
            string responseBody = await reader.ReadToEndAsync();

            dynamic stuff = JsonConvert.DeserializeObject(responseBody);

            var token = stuff.access_token;
            Debug.WriteLine(new string('-', 50));
            Debug.WriteLine((string)token);
            Debug.WriteLine(new string('-', 50));

            // Now, you can access response body.
            Debug.WriteLine(responseBody);

            // You need to do this so that the response we buffered
            // is flushed out to the client application.
            buffer.Seek(0, SeekOrigin.Begin);
            await buffer.CopyToAsync(stream);
        }
    }
}
