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
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/distributor").Include(
                        "~/Scripts/metisMenu*",
                        "~/Scripts/sb-admin-2*",
                        "~/Scripts/datatables/datatables*",
                        "~/Scripts/datatables/jquery*",
                        "~/Scripts/datatables-plugins/datatables*",
                        "~/Scripts/datatables-responsive/datatables*",
                        "~/Scripts/morris/morris*",
                        "~/Scripts/raphael/raphael*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Distributorcss").Include(
                      "~/Content/font-awesome.css",
                      "~/Content/metisMenu.css",
                      "~/Content/sb-admin-2.css",
                      "~/Content/datatables/datatables*",
                      "~/Content/datatables/jquery*",
                      "~/Content/datatables-plugins/datatables*",
                      "~/Content/datatables-responsive/datatables*",
                      "~/Content/morris/morris*"));

            bundles.Add(new StyleBundle("~/Contect/SCSS").Include(
                      "~/Content/SCSSStyles/font-awesome/scss/_animated.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_bordered-pulled.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_core.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_extras.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_fixed_width.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_icons.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_larger.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_list.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_mixins.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_path.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_rotated-flipped.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_screen-reader.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_spinning.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_stacked.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/_variables.scss",
                      "~/Content/SCSSStyles/font-awesome/scss/font-awesome.scss"));
        }
    }
}
