using System.Web.Optimization;

namespace Jane
{
   public static class Foundation
   {
      public static Bundle Scripts()
      {
         return new ScriptBundle("~/bundles/foundation").Include(
            "~/Scripts/foundation/fastclick.js",
            "~/Scripts/jquery.cookie.js",
            "~/Scripts/foundation/foundation.js",
            "~/Scripts/foundation/foundation.*",
            "~/Scripts/foundation/app.js");
      }

      public static Bundle FoundationIcons()
      {
         return new StyleBundle("~/Content/Fonts/foundation-icons/css").Include(
            "~/Content/Fonts/foundation-icons.css",
            new CssRewriteUrlTransform());
      }
   }
}