using System;
using System.Collections.Generic;
using System.Net;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Procent.Redirector.Tests.Acceptance.service.links_rest
{
    public abstract class requesting_single_link
        : using_db
    {
        Establish ctx = () =>
            {
                link = new Link
                    {
                        Alias = "link-alias",
                        Target = "http://target.com/",
                        Visits = new List<Visit>
                            {
                                new Visit
                                    {
                                        Occured = DateTime.UtcNow,
                                        Referrer = null,
                                    },
                                new Visit
                                    {
                                        Occured = DateTime.UtcNow,
                                        Referrer = null,
                                    }
                            }
                    };

                using (var session = store.OpenSession())
                {
                    session.Store(link);
                    session.SaveChanges();
                }
            };

        protected static Link link;
    }

    [Subject("API")]
    public class when_requesting_single_existing_link
        : requesting_single_link
    {
        Establish ctx = () =>
            {
                url = "links/" + link.Id;
            };

        It returns_link_from_database = () => response.DeserializeAsJson<Link>().ShouldBeLike(link);

        static Link DeserializedResponse
        {
            get { return JsonConvert.DeserializeObject<Link>(response.Content.ReadAsStringAsync().Result); }
        }
    }

    [Subject("API")]
    public class when_requesting_single_nonexisting_link
        : requesting_single_link
    {
        Establish ctx = () =>
            {
                url = "links/nonexisting-link";
            };

        It returns_404_not_found = () => response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
}