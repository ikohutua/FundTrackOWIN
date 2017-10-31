using System.Web.Http;

namespace OwinAuthServer.Controllers
{
    [RoutePrefix("api/Claims")]
    [Authorize]
    public class TestController : ApiController
    {
        //The user must have a token that OAuth gave him
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(ActionContext.Request.Headers.Authorization.Parameter);
        }
    }
}
