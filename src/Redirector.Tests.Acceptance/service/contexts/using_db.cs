using System.IO;
using Machine.Specifications;
using Procent.Redirector.Configuration;
using Raven.Client;
using Raven.Client.Embedded;

namespace Procent.Redirector.Tests.Acceptance.service
{
    public class using_db
        : making_request
    {
        static string db_path = "./test-db";

        Establish ctx = () =>
            {
                ClearDatabase();

                store = new EmbeddableDocumentStore() { DataDirectory = db_path };
                store.Initialize();

                Bootstraper.ConfigureRaven(store);
            };

        private static void ClearDatabase()
        {
            if (Directory.Exists(db_path))
            {
                foreach (var file in Directory.GetFiles(db_path, "*.*", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }
            }
        }

        Cleanup db = () => store.Dispose();

        protected static IDocumentStore store;
    }
}