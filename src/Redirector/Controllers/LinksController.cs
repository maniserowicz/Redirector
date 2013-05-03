using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Procent.Redirector.API
{
    public class LinksController : RedirectorControllerBase
    {
        public IEnumerable<link_read_model> Get()
        {
            using (var session = NewSession())
            {
                return session.Query<Link>().ToList()
                    .Select(x => link_read_model.MapFrom(x))
                    .ToList();
            }
        }

        public class link_read_model
        {
            public string id { get; set; }
            public string alias { get; set; }
            public string target { get; set; }
            public int visitsCount { get; set; }

            public static link_read_model MapFrom(Link source)
            {
                return new link_read_model
                {
                    id = source.Id,
                    alias = source.Alias,
                    target = source.Target,
                    visitsCount = source.Visits.Count
                };
            }
        }

        public HttpResponseMessage Post(link_write_model link)
        {
            if (ModelState.IsValid == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var newLink = link.MapToLink();

            using (var session = NewSession())
            {
                if (session.Query<Link>().Any(x => x.Alias == newLink.Alias))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, string.Format("Link with alias '{0}' is already defined", newLink.Alias));
                }

                session.Store(newLink);
                session.SaveChanges();
            }

            var response = Request.CreateResponse(HttpStatusCode.Created);
            string requestUri = Request.RequestUri.AbsoluteUri;
            if (false == requestUri.EndsWith("/"))
            {
                requestUri += "/";
            }
            response.Headers.Location = new Uri(requestUri + newLink.Id);

            return response;
        }

        public class link_write_model
        {
            [Required]
            public string alias { get; set; }

            [Required]
            [Url]
            public string target { get; set; }

            public Link MapToLink()
            {
                return new Link
                {
                    Alias = this.alias,
                    Target = this.target
                };
            }
        }

        public link_details_read_model Get(string id)
        {
            using (var session = NewSession())
            {
                var link = session.Load<Link>(id);

                if (link == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                return link_details_read_model.MapFrom(link);
            }
        }

        public class link_details_read_model
        {
            public string id { get; set; }
            public string alias { get; set; }
            public string target { get; set; }
            public visit_read_model[] visits { get; set; }

            public static link_details_read_model MapFrom(Link source)
            {
                return new link_details_read_model
                {
                    id = source.Id,
                    alias = source.Alias,
                    target = source.Target,
                    visits = source.Visits.Select(x => visit_read_model.MapFrom(x)).ToArray()
                };
            }

            public class visit_read_model
            {
                public DateTime occured { get; set; }
                public Uri referrer { get; set; }

                public static visit_read_model MapFrom(Visit source)
                {
                    return new visit_read_model
                    {
                        occured = source.Occured,
                        referrer = source.Referrer
                    };
                }
            }
        }

        public void Delete(string id)
        {
            using (var session = NewSession())
            {
                var link = session.Load<Link>(id);

                if (link != null)
                {
                    session.Delete(link);
                    session.SaveChanges();
                }
            }
        }
    }
}