using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FriendlyUrlForMvc.Web.Infrastructure {
    /// <summary>
    /// Custom Route Handler for SEO Management
    /// </summary>
    public class SeoMvcRouteHandler : MvcRouteHandler {

        /// <summary>
        /// This is default MVC route template
        /// </summary>
        private static readonly Regex TypicalLink = new Regex("^.+/.+(/.*)?");

        /// <summary>Returns the HTTP handler by using the specified HTTP context.</summary>
        /// <returns>The HTTP handler.</returns>
        /// <param name="requestContext">The request context.</param>
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext) {
            if (requestContext == null) {
                throw new ArgumentNullException(nameof(requestContext));
            }
            var url = requestContext.HttpContext.Request.Path.TrimStart('/');
            if (string.IsNullOrEmpty(url) || TypicalLink.IsMatch(url)) return base.GetHttpHandler(requestContext);
            var page = FriendlyUrlProvider.Default.GetPageByFriendlyUrl(url);
            if (page == null) return base.GetHttpHandler(requestContext);
            requestContext.RouteData.Values["controller"] = page.ControllerName;
            requestContext.RouteData.Values["action"] = page.ActionName;
            requestContext.RouteData.Values["id"] = page.PageId.ToString();
            return base.GetHttpHandler(requestContext);
        }
    }
}
