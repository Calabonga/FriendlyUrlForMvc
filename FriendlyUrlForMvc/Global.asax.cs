using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FriendlyUrlForMvc.Web.Controllers;
using FriendlyUrlForMvc.Web.Infrastructure.Services;

namespace FriendlyUrlForMvc.Web {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyContainer.Initialize();
        }

        protected void Application_Error() {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            if (httpException != null) {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode) {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;

                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }
            Response.TrySkipIisCustomErrors = true;
            IController errorsController = DependencyResolver.Current.GetService<SiteController>();

            var log = DependencyResolver.Current.GetService<ILogService>();
            log.LogError(exception);

            var wrapper = new HttpContextWrapper(Context);
            var rc = new RequestContext(wrapper, routeData);
            errorsController.Execute(rc);

        }
    }
}
