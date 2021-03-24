using DotNetNuke.Web.Api;


namespace WebApplication1
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("WebApplication1", "default", "{controller}/{action}", new[] { "WebApplication1.Controllers" });
        }
    }
}