using Machine.Specifications;
using Raven.Client.Embedded;

namespace Procent.Redirector.Tests.API
{
    public abstract class using_db
    {
        Establish ctx = () =>
        {
            _store = new EmbeddableDocumentStore() { RunInMemory = true };
            _store.Initialize();
        };

        protected static EmbeddableDocumentStore _store;
    }
}