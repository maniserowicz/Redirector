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
                defaults: new {controller = "Home", action = "Index"}
            );

            routes.MapRoute(
                name: "login",
                url: "login",
                defaults: new {controller = "Auth", action = "Index"}
            );
            routes.MapRoute(
                name: "logout",
                url: "logout",
                defaults: new {controller = "Auth", action = "Logout"}
            );
            routes.MapRoute(
                name: "WorldDominationAutomatedMvc-Redirect",
                url: "authentication/redirect/{providerkey}/{additionaldata}",
                defaults: new { controller = "Auth", action = "RedirectToAuthenticate", additionaldata = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "WorldDominationAutomatedMvc-AuthenticateCallback",
                url: "authentication/authenticatecallback",
                defaults: new { controller = "Auth", action = "AuthenticateCallback", additionaldata = UrlParameter.Optional }
            );
        }
    }
}