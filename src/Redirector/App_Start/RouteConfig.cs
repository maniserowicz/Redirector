using System.Web.Mvc;
using System.Web.Routing;

namespace Procent.Redirector.Configuration
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "root",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}