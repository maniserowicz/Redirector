using System;
using System.Net;
using System.Net.Http;
using Machine.Specifications;
using Procent.Redirector.API;
using System.Linq;

namespace Procent.Redirector.Tests.API
{
    public abstract class adding_new_link
        : using_db
    {
        Establish ctx = () =>
        {
            newLink = new Link
            {
                Alias = "new-link",
                Target = "http://new-target.com/",
            };

            controller = new LinksController { NewSession = () => store.OpenSession() };
            controller.Request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://myservice/links/")
                };
        };

        Because of = () => response = controller.Post(newLink);

        protected static Link newLink;
        protected static LinksController controller;
        protected static HttpResponseMessage response;
    }

    [Subject(typeof(LinksController))]
    public class when_adding_new_link_with_unique_alias
        : adding_new_link
    {
        It stores_new_link_in_database = () => new_link_from_db.ShouldNotBeNull();

        It returns_created_response = () => response.StatusCode.ShouldEqual(HttpStatusCode.Created);

        It returns_new_resource_url_in_header = () => response.Headers.Location.AbsoluteUri.ShouldEqual("http://myservice/links/" + new_link_from_db.Id);

        static Link new_link_from_db
        {
            get
            {
                using (var session = store.OpenSession())
                {
                    return session.Query<Link>()
                        .FirstOrDefault(x => x.Alias == newLink.Alias && x.Target == newLink.Target);
                }
            }
        }
    }

    [Subject(typeof(LinksController))]
    public class when_adding_new_link_with_existing_alias
        : adding_new_link
    {
        Establish ctx = () =>
            {
                using (var session = store.OpenSession())
                {
                    session.Store(new Link
                        {
                            Alias = newLink.Alias
                        });
                    session.SaveChanges();
                }
            };

        It returns_error_response = () => response.StatusCode.ShouldEqual(HttpStatusCode.Conflict);
    }
}