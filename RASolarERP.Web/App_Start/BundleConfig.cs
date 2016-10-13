using System;
using System.Web;
using System.Web.Optimization;

namespace RASolarERP.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.IgnoreList.Clear();
            //AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}-custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/RASolarCustomJS").Include(
                "~/Scripts/RASolarCustomFunction.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                "~/Content/RASolarERPSecurity.css",
                "~/Content/RASolarERP.css"));

            bundles.Add(new StyleBundle("~/Content/themes/custom-theme/css").Include(
                        "~/Content/themes/custom-theme/jquery.ui.core.css",
                        "~/Content/themes/custom-theme/jquery.ui.resizable.css",
                        "~/Content/themes/custom-theme/jquery.ui.selectable.css",
                        "~/Content/themes/custom-theme/jquery.ui.accordion.css",
                        "~/Content/themes/custom-theme/jquery.ui.autocomplete.css",
                        "~/Content/themes/custom-theme/jquery.ui.button.css",
                        "~/Content/themes/custom-theme/jquery.ui.dialog.css",
                        "~/Content/themes/custom-theme/jquery.ui.slider.css",
                        "~/Content/themes/custom-theme/jquery.ui.tabs.css",
                        "~/Content/themes/custom-theme/jquery.ui.datepicker.css",
                        "~/Content/themes/custom-theme/jquery.ui.progressbar.css",
                        "~/Content/themes/custom-theme/jquery.ui.theme.css"));

            //BundleTable.EnableOptimizations = true;
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.core.min.css", OptimizationMode.WhenDisabled);
        }
    }
}