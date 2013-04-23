using System.Net;
using System.Net.Http;
using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service
{
    public abstract class requesting_existing_link
        : using_db
    {
        Establish ctx = () =>
        {
            _url = "r/some-link";

            _link = new Link
                {
                    Alias = "some-link",
                    Target = "http://target-resource.com",
                };

            using (var session = _store.OpenSession())
            {
                session.Store(_link);
                session.SaveChanges();
            }
        };

        protected static Link load_link()
        {
            using (var session = _store.OpenSession())
            {
                return session.Load<Link>(_link.Id);
            }
        }

        protected static Link _link;
    }

    [Behaviors]
    public class valid_redirection
    {
        It returns_redirect_response = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Redirect);

        It redirects_to_link_target = () => _response.Content.ReadAsStringAsync().Result.ShouldEqual(_link.Target);

        protected static HttpResponseMessage _response;

        protected static Link _link;
    }

    [Subject("API")]
    public class when_requesting_existing_link_for_the_first_time
        : requesting_existing_link
    {
        Behaves_like<valid_redirection> valid_redirection;

        It adds_new_visit = () => load_link().Visits.ShouldNotBeEmpty();
    }

    [Subject("API")]
    public class when_requesting_existing_link_for_nth_time
        : requesting_existing_link
    {
        Establish ctx = () =>
            {
                var link = load_link();
                link.Visits.Add(new Visit());
                link.Visits.Add(new Visit());

                using (var session = _store.OpenSession())
                {
                    session.Store(link);
                    session.SaveChanges();
                }
            };

        Behaves_like<valid_redirection> valid_redirection;

        It adds_another_visit = () => load_link().Visits.Count.ShouldEqual(3);
    }
}