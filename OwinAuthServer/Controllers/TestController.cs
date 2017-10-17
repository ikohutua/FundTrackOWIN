using System.Web.Http;

namespace OwinAuthServer.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok("Good result:)");
        }
    }
}
