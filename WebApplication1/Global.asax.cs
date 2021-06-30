using System.Web.Http;
using ProfApreciat.Filters;

namespace ProfApreciat
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
