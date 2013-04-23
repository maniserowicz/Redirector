using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service.environment
{
    [Subject("Environment check")]
    public class when_requesting_root
        : making_request
    {
        Establish ctx = () => _url = "";

        It returns_success_response = () => _response.IsSuccessStatusCode.ShouldBeTrue();
    }
}