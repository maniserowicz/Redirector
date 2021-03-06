﻿using System;
using System.Net;
using System.Net.Http;
using Machine.Specifications;
using Procent.Redirector.API;
using System.Linq;

namespace Procent.Redirector.Tests.API
{
    public abstract class requesting_redirection
        : using_db
    {
        Establish ctx = () =>
            {
                controller = new RedirectController {NewSession = () => store.OpenSession()};
                controller.Request = new HttpRequestMessage();
                controller.Request.Headers.Referrer = new Uri("http://source.com/");

                requestTime = new DateTime(2012, 11, 28);
                ApplicationTime._replaceCurrentTimeLogic(() => requestTime);
            };

        Because of = () => response = controller.Redirect(alias);

        Cleanup date = () => ApplicationTime._revertToDefaultLogic();

        protected static RedirectController controller;
        protected static string alias;
        protected static HttpResponseMessage response;
        protected static DateTime requestTime;
    }

    public abstract class requesting_existing_redirection
        : requesting_redirection
    {
        Establish ctx = () =>
            {
                alias = "existing-link";

                using (var session = store.OpenSession())
                {
                    link = new Link { Alias = alias, Target = "http://www.target.com/" };
                    session.Store(link);
                    session.SaveChanges();
                }
            };

        public static Link load_link()
        {
            using (var session = store.OpenSession())
            {
                return session.Load<Link>(link.Id);
            }
        }

        protected static Link link;
    }

    [Behaviors]
    public class valid_redirection
    {
        It returns_redirect = () => response.StatusCode.ShouldEqual(HttpStatusCode.Redirect);

        It redirects_to_target_url = () => response.Headers.Location.AbsoluteUri.ShouldEqual(link.Target);

        It saves_referrer_in_visit = () => requesting_existing_redirection.load_link().Visits.Last().Referrer.AbsoluteUri.ShouldEqual("http://source.com/");

        It saves_visit_time = () => requesting_existing_redirection.load_link().Visits.Last().Occured.ShouldEqual(requestTime);

        protected static HttpResponseMessage response;
        protected static Link link;
        protected static DateTime requestTime;
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
        Establish ctx = () => alias = "nonexisting-link";

        It returns_not_found = () => response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
}