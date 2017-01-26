using System.Web.Mvc;
using System.Web.Routing;
using FriendlyUrlForMvc.Web.Infrastructure;

namespace FriendlyUrlForMvc.Web {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ).RouteHandler = new SeoMvcRouteHandler();
        }
    }
}
