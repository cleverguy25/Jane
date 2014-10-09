// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane
{
   using System.Diagnostics;
   using System.Diagnostics.Contracts;
   using System.Web.Optimization;

   using Jane.Bundles;
   using Jane.Infrastructure;

   public class BundleConfig
   {
      // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
      public static void RegisterBundles(BundleCollection bundles)
      {
         Contract.Requires(bundles != null);
         
         bundles.UseCdn = SiteConfiguration.UseCdn.Value;
         BundleTable.EnableOptimizations = Debugger.IsAttached == false;

         var jquery = new ScriptBundle("~/bundles/jquery", "//ajax.aspnetcdn.com/ajax/jquery/jquery-2.1.1.min.js").Include("~/Scripts/jquery-2.1.1.js");
         jquery.CdnFallbackExpression = "window.jQuery";
         bundles.Add(jquery);

         var modernizr = new ScriptBundle("~/bundles/modernizr", "http://ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.6.2.js").Include("~/Scripts/modernizr-*");
         modernizr.CdnFallbackExpression = "window.Modernizr";
         bundles.Add(modernizr);

         bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

         bundles.Add(Foundation.FoundationIcons());

         bundles.Add(Foundation.Scripts());
      }
   }
}