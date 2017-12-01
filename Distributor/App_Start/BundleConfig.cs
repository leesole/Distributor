﻿using System.Web;
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/js/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/css/bootstrap.min.css",
                      "~/css/bootstrap-theme.css",
                      "~/css/modern-business.css",
                      "~/css/jquery-ui.min.css",
                      "~/Content/site.css",
                      "~/css/jquery.dynatable.css",
                      "~/Content/font-awesome/css/font-awesome.min.css",
                      "~/Content/distributor.min.css"));

            //bundles.Add(new StyleBundle("~/css").Include(
            //          "~/css/jquery-ui.min.css"));
        }
    }
}
