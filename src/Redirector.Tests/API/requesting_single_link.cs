using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Machine.Specifications;
using Procent.Redirector.API;

namespace Procent.Redirector.Tests.API
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

            controller = new LinksController() {NewSession = store.OpenSession};
        };

        Because of = () =>
            {
                try
                {
                    returned_link = controller.Get(requested_link_id);
                }
                catch (HttpResponseException exc)
                {
                    error = exc;
                }
            };

        static LinksController controller;

        protected static Link link;
        protected static string requested_link_id;
        protected static Link returned_link;
        protected static HttpResponseException error;
    }

    [Subject(typeof(LinksController))]
    public class when_requesting_single_existing_link
        : requesting_single_link
    {
        Establish ctx = () =>
        {
            requested_link_id = link.Id;
        };

        It returns_link_from_database = () => returned_link.ShouldBeLike(link);

        It returns_visits_along_with_link = () => returned_link.Visits.Count.ShouldEqual(2);
    }

    [Subject(typeof(LinksController))]
    public class when_requesting_single_nonexisting_link
        : requesting_single_link
    {
        Establish ctx = () =>
        {
            requested_link_id = "nonexisting-link";
        };

        It returns_404_not_found = () => error.Response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
}