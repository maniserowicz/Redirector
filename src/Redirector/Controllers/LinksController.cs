using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
            string requestUri = Request.RequestUri.AbsoluteUri;
            if (false == requestUri.EndsWith("/"))
            {
                requestUri += "/";
            }
            response.Headers.Location = new Uri(requestUri + link.Id);

            return response;
        }

        public Link Get(string id)
        {
            using (var session = NewSession())
            {
                var link = session.Load<Link>(id);

                if (link == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                return link;
            }
        }
    }
}