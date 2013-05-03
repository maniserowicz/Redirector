using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service.environment
{
    [Subject("Environment check")]
    public class when_requesting_api_root
        : using_db
    {
        Establish ctx = () => url = "links";

        It returns_success_response = () => response.IsSuccessStatusCode.ShouldBeTrue();
    }
}