using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Raven.Client;
using System.Linq;

namespace Procent.Redirector.API
{
    public class RedirectController : RedirectorControllerBase
    {
        public RedirectController(Func<IDocumentSession> newSession)
            : base(newSession)
        {
        }

        [HttpGet]
        public HttpResponseMessage Redirect(string alias)
        {
            using (var session = _newSession())
            {
                Link link = session.Query<Link>().FirstOrDefault(x => x.Alias == alias);

                if (link == null)
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Link with alias '{0}' is not defined", alias));
                }

                var redirectResponse = this.Request.CreateResponse(HttpStatusCode.Redirect);

                redirectResponse.Headers.Location = new Uri(link.Target);

                link.Visits.Add(new Visit()
                    {
                        Occured = DateTime.UtcNow,
                        Referrer = Request.Headers.Referrer
                    });

                session.SaveChanges();

                return redirectResponse;
            }

        }
    }
}