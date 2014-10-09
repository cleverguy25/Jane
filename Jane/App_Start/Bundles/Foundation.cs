// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Foundation.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Bundles
{
   using System.Web.Optimization;

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

      public static Bundle IncludeFoundationIcons(this Bundle bundle)
      {
         return bundle.Include(
            "~/Content/Fonts/foundation-icons.css", 
            new CssRewriteUrlTransform());
      }
   }
}