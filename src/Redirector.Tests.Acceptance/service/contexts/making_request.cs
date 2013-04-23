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

                _httpClient = new HttpClient(server);
                _httpClient.BaseAddress = address;
            };

        Because of = () => _response = _httpClient.GetAsync(_url).Result;

        Cleanup client = () => _httpClient.Dispose();

        static HttpClient _httpClient;

        protected static string _url;
        protected static HttpResponseMessage _response;
    }
}