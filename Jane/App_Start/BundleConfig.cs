// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane
{
   using System.Diagnostics.Contracts;
   using System.Web.Optimization;

   using Jane.Bundles;

   public class BundleConfig
   {
      // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
      public static void RegisterBundles(BundleCollection bundles)
      {
         Contract.Requires(bundles != null);

         bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
         bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

         bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

         bundles.Add(Foundation.FoundationIcons());

         bundles.Add(Foundation.Scripts());
      }
   }
}