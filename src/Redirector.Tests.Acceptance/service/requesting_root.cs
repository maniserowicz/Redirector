using System;
using System.Net.Http;
using System.Web.Http.SelfHost;
using Machine.Specifications;
using Procent.Redirector.Configuration;

namespace Procent.Redirector.Tests.Acceptance.service
{
    public class when_requesting_root
    {
        Establish ctx = () =>
            {
                var address = new Uri("http://localhost:8355");
                var config = new HttpSelfHostConfiguration(address);
                WebApiBootstrap.Configure(config);

                var server = new HttpSelfHostServer(config);

                _httpClient = new HttpClient(server);
                _httpClient.BaseAddress = address;
            };

        Because of = () => _response = _httpClient.GetAsync("").Result;

        It returns_success_response = () => _response.IsSuccessStatusCode.ShouldBeTrue();

        Cleanup stuff = () => _httpClient.Dispose();

        static HttpClient _httpClient;
        static HttpResponseMessage _response;
    }
}