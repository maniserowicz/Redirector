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

        Because of = () => response = httpClient.GetAsync(url).Result;

        Cleanup client = () => httpClient.Dispose();

        static HttpClient httpClient;

        protected static string url;
        protected static HttpResponseMessage response;
    }
}