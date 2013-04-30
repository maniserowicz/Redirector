using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Machine.Specifications;
using System.Linq;

namespace Procent.Redirector.Tests.Acceptance.service.links_rest
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
                request = new HttpRequestMessage(HttpMethod.Post, "links");
                request.Content = new ObjectContent(newLink.GetType(), newLink, new JsonMediaTypeFormatter());
            };

        protected static Link newLink;
    }

    [Subject("API")]
    public class when_adding_new_link
        : adding_new_link
    {
        It stores_link_in_database = () => use_db(s => s.Query<Link>().Count(x => x.Target == newLink.Target && x.Alias == newLink.Alias))
            .ShouldEqual(1);
    }

    [Subject("API")]
    public class when_adding_link_with_duplicate_alias
        : adding_new_link
    {
        Establish ctx = () => use_db(s =>
            {
                s.Store(new Link()
                    {
                        Alias = newLink.Alias,
                        Target = "http://some-target.com"
                    });
                s.SaveChanges();
            });

        It returns_error_response = () => response.StatusCode.ShouldEqual(HttpStatusCode.Conflict);
    }
}