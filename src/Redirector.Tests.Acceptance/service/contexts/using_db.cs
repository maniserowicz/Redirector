using System;
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
        protected static void use_db(Action<IDocumentSession> action)
        {
            using (var session = store.OpenSession())
            {
                action(session);
            }
        }
        protected static T use_db<T>(Func<IDocumentSession, T> action)
        {
            T result = default(T);

            use_db(s =>
                {
                    result = action(s);
                });

            return result;
        }
    }
}