using System.Net.Http;
using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service
{
    public class when_requesting_root
    {
        Establish ctx = () =>
            {
                _httpClient = new HttpClient();
            };

        Because of = () => _response = _httpClient.GetAsync("http://localhost:8355").Result;

        It returns_success_response = () => _response.IsSuccessStatusCode.ShouldBeTrue();

        static HttpClient _httpClient;
        static HttpResponseMessage _response;
    }
}