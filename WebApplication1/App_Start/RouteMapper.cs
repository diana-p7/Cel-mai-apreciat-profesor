using DotNetNuke.Web.Api;


namespace ProfApreciat
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("ProfApreciat", "default", "{controller}/{action}", new[] { "ProfApreciat.Controllers" });
        }
    }
}