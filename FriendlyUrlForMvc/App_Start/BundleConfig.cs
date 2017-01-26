using System.Web.Optimization;

namespace FriendlyUrlForMvc.Web {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/editor")
                .Include("~/Scripts/froala-editor/js/froala_editor.min.js",
                    "~/Scripts/froala-editor/js/languages/ru.js")
                .IncludeDirectory("~/Scripts/froala-editor/js/plugins", "*.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/editor").Include(
                        "~/Scripts/froala-editor/css/froala_editor.min.css",
                        "~/Scripts/froala-editor/css/plugins/*.css",
                        "~/Scripts/froala-editor/css/froala_style.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
