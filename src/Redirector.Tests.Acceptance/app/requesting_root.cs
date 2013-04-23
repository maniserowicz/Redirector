using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.app
{
    [Subject("App")]
    public class when_requesting_root
    {
        It returns_html;

        It displays_all_defined_links;
    }
}