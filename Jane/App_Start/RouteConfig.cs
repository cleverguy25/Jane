// --------------------------------------------------------------------------------------------------------------------
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

         routes.MapRoute("Robots.txt", "robots.txt", new { controller = "Seo", action = "Robots" });

         routes.MapRoute("SiteMap.xml", "sitemap.xml", new { controller = "Seo", action = "SiteMap" });

         RegisterBlogRoutes(routes);

         routes.MapRoute("TagCloud", "tag", new { controller = "Tag", action = "TagCloud" });

         routes.MapRoute("Home", string.Empty, new { controller = "Blog", action = "List" });

         routes.MapRoute("404", "{*url}", new { controller = "Error", action = "NotFound" });
      }

      private static void RegisterBlogRoutes(RouteCollection routes)
      {
         routes.MapRoute("BlogsRecent", "blog/recent", new { controller = "Blog", action = "Recent" });

         routes.MapRoute("BlogsRss", "blog/rss", new { controller = "Feed", action = "Rss" });

         routes.MapRoute("BlogsRss+Tag", "blog/rss/tagged/{tag}", new { controller = "Feed", action = "RssByTag", tag = UrlParameter.Optional });

         routes.MapRoute("BlogsAtom", "blog/atom", new { controller = "Feed", action = "Atom" });

         routes.MapRoute("BlogsAtom+Tag", "blog/atom/tagged/{tag}", new { controller = "Feed", action = "AtomByTag", tag = UrlParameter.Optional });

         routes.MapRoute("Blogs", "blog", new { controller = "Blog", action = "List" });

         routes.MapRoute("BlogsByTag", "blog/tagged/{tag}", new { controller = "Blog", action = "GetByTag", tag = UrlParameter.Optional });

         routes.MapRoute("BlogBySlug", "blog/{slug}", new { controller = "Blog", action = "GetBySlug", slug = UrlParameter.Optional });

         routes.MapRoute("BlogRelated", "blog/{slug}/related", new { controller = "Blog", action = "Related", slug = UrlParameter.Optional });
      }
   }
}