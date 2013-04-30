using System;
using System.Collections.Generic;
using System.Net;
using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service
{
    [Subject("API")]
    public class when_requesting_existing_redirection
        : using_db
    {
        Establish ctx = () =>
        {
            url = "r/some-link";

            _link = new Link
                {
                    Alias = "some-link",
                    Target = "http://target-resource.com/",
                    Visits = new List<Visit>{new Visit{Occured = DateTime.UtcNow, Referrer = new Uri("http://some-referrer.com")}}
                };

            using (var session = store.OpenSession())
            {
                session.Store(_link);
                session.SaveChanges();
            }
        };

        It returns_redirect_response = () => response.StatusCode.ShouldEqual(HttpStatusCode.Redirect);

        It redirects_to_link_target = () => response.Headers.Location.AbsoluteUri.ShouldEqual(_link.Target);

        It saves_visit_in_db = () => _link_from_db.Visits.Count.ShouldEqual(2);

        static Link _link;

        static Link _link_from_db
        {
            get
            {
                using (var session = store.OpenSession())
                {
                    return session.Load<Link>(_link.Id);
                }
            }
        }
    }
}