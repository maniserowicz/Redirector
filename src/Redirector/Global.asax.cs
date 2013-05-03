using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Procent.Redirector.Configuration;

namespace Procent.Redirector
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Bootstraper.ConfigureWebApi(GlobalConfiguration.Configuration);

            Bootstraper.ConfigureRaven();
        }
    }
}