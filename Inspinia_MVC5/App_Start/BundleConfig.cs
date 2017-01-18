using System.Web;
using System.Web.Optimization;

namespace BootStrap
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.1.js", "~/Scripts/jquery.tooltip.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/carousel").Include("~/assets/js/owl.carousel.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include("~/Scripts/knockout-{version}.js", "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/assets/js/jquery.matchHeight-min.js",
                "~/assets/js/hideMaxListItem.js",
                "~/assets/js/script.js",
                "~/assets/js/pace.min.js",
                "~/assets/js/fileinput.js",
                "~/assets/js/form-validation.js",
                "~/assets/plugins/autocomplete/jquery.mockjax.js",
                "~/assets/plugins/autocomplete/jquery.autocomplete.js",
                "~/assets/plugins/autocomplete/usastates.js",
                "~/assets/plugins/autocomplete/autocomplete-demo.js",
                "~/assets/plugins/bxslider/jquery.bxslider.js",
                "~/plugins/jquery.fs.scroller/jquery.fs.scroller.js",
                "~/plugins/jquery.fs.selecter/jquery.fs.selecter.js"));

            // summernote styles
            bundles.Add(new StyleBundle("~/plugins/summernoteStyles").Include(
                      "~/assets/plugins/summernote/summernote.css",
                      "~/assets/plugins/summernote/summernote-bs3.css"));

            // summernote 
            bundles.Add(new ScriptBundle("~/plugins/summernote").Include(
                      "~/assets/plugins/summernote/summernote.min.js"));




            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/assets/css/bootstrap.css",
                "~/assets/css/style.css",
                "~/assets/css/animation.css",
                "~/assets/css/owl.carousel.css",
                "~/assets/css/owl.theme.css",
                "~/assets/css/fileinput.css",
                "~/assets/plugins/bxslider/jquery.bxslider.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}