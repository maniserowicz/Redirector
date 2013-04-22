using System.Web.Http;

namespace Procent.Redirector.Configuration
{
    public static class WebApiBootstrap
    {
        public static void Configure(HttpConfiguration config)
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
        }
    }
}