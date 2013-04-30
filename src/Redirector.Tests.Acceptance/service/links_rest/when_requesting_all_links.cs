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
                _url = "links";

                _link1 = new Link
                {
                    Alias = "first-link",
                    Target = "http://first-target.com/",
                };
                _link2 = new Link
                {
                    Alias = "second-link",
                    Target = "http://second-target.com/",
                };

                using (var session = _store.OpenSession())
                {
                    session.Store(_link1);
                    session.Store(_link2);
                    session.SaveChanges();
                }
            };

        It returns_all_links_from_database = () => JsonConvert.DeserializeObject<Link[]>(_response.Content.ReadAsStringAsync().Result)
            .ShouldBeLike(new[] { _link1, _link2 });

        static Link _link1;
        static Link _link2;
    }
}