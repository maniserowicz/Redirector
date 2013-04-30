using Machine.Specifications;
using Raven.Client.Embedded;

namespace Procent.Redirector.Tests.API
{
    public abstract class using_db
    {
        Establish ctx = () =>
        {
            store = new EmbeddableDocumentStore() { RunInMemory = true };
            store.Initialize();
        };

        protected static EmbeddableDocumentStore store;
    }
}