using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.service
{
    public class when_requesting_existing_link
    {
        It returns_redirect_response;

        It redirects_to_link_target;

        It increments_link_visits_count;
    }
}
