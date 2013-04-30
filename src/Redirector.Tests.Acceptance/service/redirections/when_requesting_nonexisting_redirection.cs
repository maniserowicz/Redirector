using System.Net;
using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service
{
    [Subject("API")]
    public class when_requesting_nonexisting_redirection
        : using_db
    {
        Establish ctx = () => url = "r/notexisting_link";

        It returns_404_not_found = () => response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
}
