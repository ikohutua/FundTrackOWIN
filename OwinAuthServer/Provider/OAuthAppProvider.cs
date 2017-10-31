using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using OwinAuthServer;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplicationOwin.Models;
using WebApplicationOwin.Service;

namespace WebApplicationOwin.Provider
{
    public class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        ///  Is used for token generating
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                var username = context.UserName;
                var password = context.Password;
                var userService = new UserService();
                User user = userService.GetUserByCredentials(username, password);
                if (user != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim("UserID", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    ClaimsIdentity oAutIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);
                    var tiket = new AuthenticationTicket(oAutIdentity, new AuthenticationProperties());
                    context.Validated(tiket);
                }
                else
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                }
            });
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }
    }
}