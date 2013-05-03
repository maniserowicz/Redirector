using System;
using System.Linq;
using System.Net.Http;
using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service.links_rest
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
            request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete
                };

        };

        protected static Link existingLink;
    }

    [Subject("API")]
    public class when_deleting_existing_link
        : deleting_link
    {
        Establish ctx = () => request.RequestUri = new Uri("links/" + existingLink.Id, UriKind.Relative);

        It removes_link_from_database = () => use_db(s => s.Query<Link>().Count(x => x.Target == existingLink.Target && x.Alias == existingLink.Alias))
            .ShouldEqual(0);
    }

    [Subject("API")]
    public class when_deleting_nonexisting_link
        : deleting_link
    {
        Establish ctx = () => request.RequestUri = new Uri("links/nonexisting-link", UriKind.Relative);

        It does_not_return_error = () => response.IsSuccessStatusCode.ShouldBeTrue();
    }
}