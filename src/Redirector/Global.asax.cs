using System.Web.Http;
using System.Web.Mvc;
using Procent.Redirector.Configuration;

namespace Procent.Redirector
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            Bootstraper.ConfigureWebApi(GlobalConfiguration.Configuration);

            Bootstraper.ConfigureRaven();
        }
    }
}