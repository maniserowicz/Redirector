using System.Web.Http;
using Procent.Redirector.Configuration.DI;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Procent.Redirector.Configuration
{
    public static class Bootstraper
    {
        public static void ConfigureWebApi(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "web app root",
                routeTemplate: "",
                defaults: new
                    {
                        controller = "WebApp",
                        action = "Index"
                    }
            );

            config.Routes.MapHttpRoute(
                name: "redirection",
                routeTemplate: "r/{alias}",
                defaults: new
                    {
                        controller = "Redirect",
                        action = "Redirect"
                    }
            );

            config.Routes.MapHttpRoute(
                name: "default api",
                routeTemplate: "{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );

            config.DependencyResolver = new SimpleContainer();
        }

        /// <param name="store">Initialized <see cref="IDocumentStore"/> to use in application. If null, new default store will be created and initialized.</param>
        public static void ConfigureRaven(IDocumentStore store = null)
        {
            if (store == null)
            {
                store = new EmbeddableDocumentStore {ConnectionStringName = "links-db"};
                store.Initialize();
            }
            store.Conventions.IdentityPartsSeparator = "-";
            RavenStore.SetDocumentStore(store);
        }
    }
}