using Machine.Specifications;

namespace Procent.Redirector.Tests.Acceptance.app
{
    [Subject("App")]
    public class when_posting_duplicate_link
    {
        It redisplays_the_same_page;

        It shows_error_message;
    }
}