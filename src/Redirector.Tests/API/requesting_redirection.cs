using System.Net;
using System.Net.Http;
using Machine.Specifications;
using Procent.Redirector.API;

namespace Procent.Redirector.Tests.API
{
    public abstract class requesting_redirection
        : using_db
    {
        Establish ctx = () =>
            {
                _controller = new RedirectController {NewSession = () => store.OpenSession()};
                _controller.Request = new HttpRequestMessage();
            };

        Because of = () => _response = _controller.Redirect(_alias);

        protected static RedirectController _controller;
        protected static string _alias;
        protected static HttpResponseMessage _response;
    }

    public abstract class requesting_existing_redirection
        : requesting_redirection
    {
        Establish ctx = () =>
            {
                _alias = "existing-link";

                using (var session = store.OpenSession())
                {
                    _link = new Link { Alias = _alias, Target = "http://www.target.com/" };
                    session.Store(_link);
                    session.SaveChanges();
                }
            };

        protected static Link load_link()
        {
            using (var session = store.OpenSession())
            {
                return session.Load<Link>(_link.Id);
            }
        }

        protected static Link _link;
    }

    [Behaviors]
    public class valid_redirection
    {
        It returns_redirect = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Redirect);

        It redirects_to_target_url = () => _response.Headers.Location.AbsoluteUri.ShouldEqual(_link.Target);

        protected static HttpResponseMessage _response;
        protected static Link _link;
    }

    [Subject(typeof(RedirectController))]
    public class when_requesting_existing_redirection_for_the_first_time
        : requesting_existing_redirection
    {
        Behaves_like<valid_redirection> valid_redirection;

        It adds_new_visit = () => load_link().Visits.ShouldNotBeEmpty();
    }

    [Subject(typeof(RedirectController))]
    public class when_requesting_existing_redirection_for_nth_time
        : requesting_existing_redirection
    {
        Establish ctx = () =>
        {
            var link = load_link();
            link.Visits.Add(new Visit());
            link.Visits.Add(new Visit());

            using (var session = store.OpenSession())
            {
                session.Store(link);
                session.SaveChanges();
            }
        };

        Behaves_like<valid_redirection> valid_redirection;

        It adds_another_visit = () => load_link().Visits.Count.ShouldEqual(3);
    }

    [Subject(typeof(RedirectController))]
    public class when_requesting_nonexisting_redirection
        : requesting_redirection
    {
        Establish ctx = () => _alias = "nonexisting-link";

        It returns_not_found = () => _response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
}