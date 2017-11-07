using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApplicationOwin.Models;

namespace OwinAuthServer.Controllers
{
    public class ClaimsController : ApiController
    {
        //The user must have a token that OAuth gave him
        public IHttpActionResult Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            if (principal == null)
            {
                return BadRequest();
            }
            User user = new User();

            user.Id = Convert.ToInt32(principal.Claims.Where(c => c.Type == "UserId").SingleOrDefault()?.Value);
            user.Login = principal.Claims.Where(c => c.Type == ClaimTypes.Name).SingleOrDefault()?.Value;
            user.Email = principal.Claims.Where(c => c.Type == ClaimTypes.Email).SingleOrDefault()?.Value;
            user.Role = principal.Claims.Where(c => c.Type == ClaimTypes.Role).SingleOrDefault()?.Value;
            

            return Ok(user);
        }
    }
}
