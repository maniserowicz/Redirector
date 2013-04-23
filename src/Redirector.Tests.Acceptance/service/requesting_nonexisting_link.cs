using System.Net;
using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service
{
    [Subject("API")]
    public class when_requesting_nonexisting_link
        : making_request
    {
        Establish ctx = () => _url = "r/notexisting_link";

        It returns_404_not_found = () => _response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
}
