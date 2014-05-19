// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the RouteConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System.Web.Mvc;
   using System.Web.Routing;

   public class RouteConfig
   {
      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute("NotFound", "NotFound", new { controller = "Error", action = "NotFound" });

         routes.MapRoute("404", "{*url}", new { controller = "Error", action = "NotFound" });
      }
   }
}