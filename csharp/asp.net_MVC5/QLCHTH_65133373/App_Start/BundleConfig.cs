using System.Web;
using System.Web.Optimization;

namespace QLCHTH_65133373
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // jQuery - sử dụng path cụ thể thay vì {version} pattern
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.7.0.js"));

            // jQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            // Modernizr
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.8.3.js"));

            // Bootstrap JS
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            // SB Admin Template Scripts
            bundles.Add(new ScriptBundle("~/bundles/sbadmin").Include(
                      "~/Content/js/scripts.js"));

            // CSS Default Bootstrap
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

            // SB Admin Template CSS (sử dụng cho Admin và Cashier Areas)
            bundles.Add(new StyleBundle("~/Content/sbadmin").Include(
                      "~/Content/css/styles.css"));
        }
    }
}
