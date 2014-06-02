﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Jane">
//   Copyright (c) Jane Contributors
// </copyright>
// <summary>
//   Defines the RouteConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System.Web.Mvc;
   using System.Web.Routing;

   using Microsoft.Ajax.Utilities;

   public class RouteConfig
   {
      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute("NotFound", "NotFound", new { controller = "Error", action = "NotFound" });

         routes.MapRoute("ServerError", "ServerError", new { controller = "Error", action = "ServerError" });

         routes.MapRoute("SiteMap.xml", "sitemap.xml", new { controller = "Seo", action = "SiteMap" });

         RegisterBlogRoutes(routes);

         routes.MapRoute("Home", string.Empty, new { controller = "Blog", action = "List" });

         routes.MapRoute("404", "{*url}", new { controller = "Error", action = "NotFound" });
      }

      private static void RegisterBlogRoutes(RouteCollection routes)
     {
         routes.MapRoute("BlogsRss", "blog/rss", new { controller = "Feed", action = "Rss" });

         routes.MapRoute("BlogsAtom", "blog/atom", new { controller = "Feed", action = "Atom" });

         routes.MapRoute("Blogs", "blog", new { controller = "Blog", action = "List" });

         routes.MapRoute("BlogBySlug", "blog/{slug}", new { controller = "Blog", action = "GetBySlug", slug = UrlParameter.Optional });
      }
   }
}