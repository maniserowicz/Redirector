using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.app
{
    [Subject("App")]
    public class when_posting_new_link
    {
        It adds_link_to_database;

        It redirects_to_links_page;
    }
}