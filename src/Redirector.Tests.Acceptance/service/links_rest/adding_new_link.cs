using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Machine.Specifications;
using System.Linq;
using Procent.Redirector.API;

namespace Procent.Redirector.Tests.Acceptance.service.links_rest
{
    public abstract class adding_new_link
        : using_db
    {
        Establish ctx = () =>
            {
                newLink = new LinksController.link_write_model
                {
                    alias = "new-link",
                    target = "http://new-target.com/",
                };
                request = new HttpRequestMessage(HttpMethod.Post, "links");
                request.Content = new ObjectContent(newLink.GetType(), newLink, new JsonMediaTypeFormatter());
            };

        protected static LinksController.link_write_model newLink;
    }

    [Subject("API")]
    public class when_adding_new_link
        : adding_new_link
    {
        It stores_link_in_database = () => use_db(s => s.Query<Link>().Count(x => x.Target == newLink.target && x.Alias == newLink.alias))
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
                        Alias = newLink.alias,
                        Target = "http://some-target.com"
                    });
                s.SaveChanges();
            });

        It returns_error_response = () => response.StatusCode.ShouldEqual(HttpStatusCode.Conflict);
    }

    [Subject("API")]
    public class when_adding_link_with_empty_alias
        : adding_new_link
    {
        Establish ctx = () => newLink.alias = string.Empty;

        It returns_error_response = () => response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }

    [Subject("API")]
    public class when_adding_link_with_empty_target
        : adding_new_link
    {
        Establish ctx = () => newLink.target = string.Empty;

        It returns_error_response = () => response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }
}