using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service.environment
{
    [Subject("Environment check")]
    public class when_requesting_root
        : making_request
    {
        Establish ctx = () => url = "";

        It returns_success_response = () => response.IsSuccessStatusCode.ShouldBeTrue();
    }
}