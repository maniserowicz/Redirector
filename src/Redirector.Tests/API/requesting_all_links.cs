using System;
using System.Collections.Generic;
using System.Net.Http;
using Machine.Specifications;
using Procent.Redirector.API;

namespace Procent.Redirector.Tests.API
{
    public abstract class requesting_all_links
        : using_db
    {
        Establish ctx = () =>
            {
                controller = new LinksController { NewSession = () => store.OpenSession() };
                controller.Request = new HttpRequestMessage();
            };

        Because of = () => links = controller.Get();

        protected static LinksController controller;
        protected static IEnumerable<LinksController.link_read_model> links;
    }

    [Subject(typeof(LinksController))]
    public class when_requesting_all_links_from_filled_database
        : requesting_all_links
    {
        Establish ctx = () =>
            {
                var link1 = new Link
                    {
                        Alias = "alias1", Target = "http://target1.com/"
                    };
                var link2 = new Link
                    {
                        Alias = "alias2", Target = "http://target2.com/",
                        Visits = new List<Visit>
                            {
                                new Visit()
                                    {
                                        Occured = DateTime.UtcNow
                                        , Referrer = new Uri("http://some-referrer.com/")
                                    }
                            }
                    };

                using (var session = store.OpenSession())
                {
                    session.Store(link1);
                    session.Store(link2);
                    session.SaveChanges();
                }

                existing_links = new[] { LinksController.link_read_model.MapFrom(link1), LinksController.link_read_model.MapFrom(link2)};
            };

        It returns_all_defined_links = () => links.ShouldBeLike(existing_links);

        static IEnumerable<LinksController.link_read_model> existing_links;
    }

    [Subject(typeof(LinksController))]
    public class when_requesting_all_links_from_empty_database
        : requesting_all_links
    {
        It returns_empty_links_collection = () => links.ShouldBeEmpty();
    }
}