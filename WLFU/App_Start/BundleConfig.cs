using System.Web.Optimization;
 
namespace JokerKS.WLFU
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Мої стилі
            bundles.Add(new StyleBundle("~/content/styles/css").Include(
                "~/Content/Styles.css"));

            // Jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            // Jquery UI
            bundles.Add(new StyleBundle("~/content/jquery-ui/css").Include(
                "~/Content/themes/base/all.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery-ui-{version}.js"));
        }
    }
}