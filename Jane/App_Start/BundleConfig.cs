// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System.Diagnostics.Contracts;
   using System.Web.Optimization;

   public class BundleConfig
   {
      // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
      public static void RegisterBundles(BundleCollection bundles)
      {
         Contract.Requires(bundles != null);

         bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

         bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

         bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

         // Set EnableOptimizations to false for debugging. For more information,
         // visit http://go.microsoft.com/fwlink/?LinkId=301862
         BundleTable.EnableOptimizations = true;
      }
   }
}