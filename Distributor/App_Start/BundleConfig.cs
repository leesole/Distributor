using System.Web;
using System.Web.Optimization;

namespace Distributor
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/jquery.dynatable.js"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/js/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/css/vendor/jquery").Include(
                      "~/css/vendor/jquery/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/css/vendor").Include(
                      "~/css/vendor/jquery/jquery.min.js",
                      "~/Scripts/jquery-ui.min.js",
                      "~/css/vendor/bootstrap/js/bootstrap.min.js",
                      "~/css/vendor/metisMenu/metisMenu.min.js",
                      "~/css/vendor/datatables/js/jquery.dataTables.min.js",
                      "~/css/vendor/datatables-plugins/dataTables.bootstrap.min.js",
                      "~/css/vendor/datatables-responsive/dataTables.responsive.js",
                      "~/css/vendor/dist/js/sb-admin-2.js"));

            bundles.Add(new StyleBundle("~/css").Include(
                      "~/css/vendor/bootstrap/css/bootstrap.min.css",
                      "~/css/vendor/metisMenu/metisMenu.min.css",
                      "~/css/vendor/datatables-plugins/dataTables.bootstrap.css",
                      "~/css/vendor/datatables-responsive/dataTables.responsive.css",
                      "~/css/vendor/sb-admin-2.min.css",
                      "~/css/vendor/font-awesome/css/font-awesome.min.css"));

            //bundles.Add(new StyleBundle("~/css").Include(
            //          "~/css/jquery-ui.min.css"));
        }
    }
}
