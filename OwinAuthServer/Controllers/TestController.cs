using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApplicationOwin.Models;

namespace OwinAuthServer.Controllers
{
    [Authorize]
    //The user must have a token that OAuth gave him
    public class ClaimsController : ApiController
    {
        public IHttpActionResult Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            if (principal == null)
            {
                return BadRequest();
            }
            User user = new User
            {
                Id = Convert.ToInt32(principal.Claims.Where(c => c.Type == "UserId").FirstOrDefault()?.Value),
                Login = principal.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault()?.Value,
                Email = principal.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault()?.Value,
                Role = principal.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault()?.Value
            };

            return Ok(user);
        }
    }
}
