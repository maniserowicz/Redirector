using System.Net.Http;
using System.Web.Http;

namespace Procent.Redirector
{
    public class WebAppController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            return this.Request.CreateResponse();
        }
    }
}