using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Machine.Specifications;
using Procent.Redirector.API;

namespace Procent.Redirector.Tests.API
{
    public abstract class deleting_link
        : using_db
    {
        Establish ctx = () =>
        {
            existingLink = new Link
            {
                Alias = "existing-link",
                Target = "http://target.com/",
            };

            using (var session = store.OpenSession())
            {
                session.Store(existingLink);
                session.Store(new Link
                {
                    Alias = "another link",
                    Target = "http://another-target.com/"
                });
                session.SaveChanges();
            }

            controller = new LinksController { NewSession = () => store.OpenSession() };
            controller.Request = new HttpRequestMessage();
        };

        Because of = () =>
            {
                try
                {
                    controller.Delete(deletedLinkId);
                }
                catch (Exception exc)
                {
                    error = exc;
                }
            };

        protected static string deletedLinkId;
        protected static Link existingLink;
        protected static LinksController controller;
        protected static Exception error;
    }

    [Subject(typeof(LinksController))]
    public class when_deleting_existing_link
        : deleting_link
    {
        Establish ctx = () => deletedLinkId = existingLink.Id;

        It removes_link_from_database = () => deleted_link_from_db.ShouldBeNull();

        It does_not_remove_any_other_links = () => all_links_from_db.Count.ShouldEqual(1);

        static Link deleted_link_from_db
        {
            get
            {
                using (var session = store.OpenSession())
                {
                    return session.Query<Link>()
                        .FirstOrDefault(x => x.Alias == existingLink.Alias && x.Target == existingLink.Target);
                }
            }
        }

        static List<Link> all_links_from_db
        {
            get
            {
                using (var session = store.OpenSession())
                {
                    return session.Query<Link>().ToList();
                }
            }
        }
    }

    [Subject(typeof(LinksController))]
    public class when_deleting_nonexisting_link
        : deleting_link
    {
        Establish ctx = () => deletedLinkId = "nonexisting-link";

        It does_not_throw = () => error.ShouldBeNull();
    }
}