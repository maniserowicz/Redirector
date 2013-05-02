using Machine.Specifications;
using Newtonsoft.Json;

namespace Procent.Redirector.Tests.Acceptance.service.links_rest
{
    [Subject("API")]
    public class when_requesting_all_links
        : using_db
    {
        Establish ctx = () =>
            {
                url = "links";

                link1 = new Link
                {
                    Alias = "first-link",
                    Target = "http://first-target.com/",
                };
                link2 = new Link
                {
                    Alias = "second-link",
                    Target = "http://second-target.com/",
                };

                using (var session = store.OpenSession())
                {
                    session.Store(link1);
                    session.Store(link2);
                    session.SaveChanges();
                }
            };

        It returns_all_links_from_database = () => response.DeserializeAsJson<Link[]>().ShouldBeLike(new[] { link1, link2 });

        static Link link1;
        static Link link2;
    }
}