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
        protected static string url { get { return _url; } set { _url = value; _request = null; } }
        static string _url;
        /// <summary>
        /// initialize request in derived class to take full control over http communication
        /// </summary>
        protected static HttpRequestMessage request { get { return _request; } set { _request = value; _url = null; } }
        static HttpRequestMessage _request;

        protected static HttpResponseMessage response;
    }
}