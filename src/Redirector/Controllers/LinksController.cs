using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Procent.Redirector.API
{
    public class LinksController : RedirectorControllerBase
    {
        public IEnumerable<Link> Get()
        {
            using (var session = NewSession())
            {
                return session.Query<Link>().ToList();
            }
        }

        public HttpResponseMessage Post(Link link)
        {
            using (var session = NewSession())
            {
                if (session.Query<Link>().Any(x => x.Alias == link.Alias))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, string.Format("Link with alias '{0}' is already defined", link.Alias));
                }

                session.Store(link);
                session.SaveChanges();
            }

            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri + link.Id);

            return response;
        }
    }
}