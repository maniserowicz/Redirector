using System.Net.Http;
using System.Web.Http;

namespace Procent.Redirector
{
    public class RedirectController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Redirect(string alias)
        {
            return this.Request.CreateResponse();
        }
    }
}