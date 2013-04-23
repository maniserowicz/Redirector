using Raven.Client;

namespace Procent.Redirector.Configuration
{
    public static class RavenStore
    {
        private static IDocumentStore _documentStore;

        public static void SetDocumentStore(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public static IDocumentSession Session()
        {
            return _documentStore.OpenSession();
        }
    }
}