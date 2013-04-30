using System;
using System.Net.Http;
using System.Web.Http.SelfHost;
using Machine.Specifications;
using Procent.Redirector.Configuration;

namespace Procent.Redirector.Tests.Acceptance.service
{
    public class making_request
    {
        Establish ctx = () =>
            {
                var address = new Uri("http://localhost:8355");
                var config = new HttpSelfHostConfiguration(address);

                Bootstraper.ConfigureWebApi(config);

                var server = new HttpSelfHostServer(config);

                httpClient = new HttpClient(server);
                httpClient.BaseAddress = address;
            };

        Because of = () =>
            {
                if (url != null && request != null)
                {
                    throw new InvalidOperationException("Cannot make request if both url and request objects are initialized; initialize only one of them depending on whether you're trying to issue a basic GET request or take full control over HTTP communication");
                }
                if (url != null)
                {
                    response = httpClient.GetAsync(url).Result;
                }
                if (request != null)
                {
                    response = httpClient.SendAsync(request).Result;
                }
            };

        Cleanup client = () => httpClient.Dispose();

        static HttpClient httpClient;

        /// <summary>
        /// initialize url in derived class to issue a basic http get request to a given address
        /// </summary>
        protected static string url;
        /// <summary>
        /// initialize request in derived class to take full control over http communication;
        /// </summary>
        protected static HttpRequestMessage request;
        protected static HttpResponseMessage response;
    }
}