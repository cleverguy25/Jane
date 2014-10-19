// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System.Diagnostics.Contracts;
   using System.Web.Mvc;
   using System.Web.Routing;

   using Jane.MetaWeblog;

   using Microsoft.Ajax.Utilities;

   public class RouteConfig
   {
      public static void RegisterRoutes(RouteCollection routes)
      {
         Contract.Requires(routes != null);

         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute("RSD", "rsd", new { controller = "Feed", action = "Rsd" });

         routes.MapRoute("NotFound", "NotFound", new { controller = "Error", action = "NotFound" });

         routes.MapRoute("ServerError", "ServerError", new { controller = "Error", action = "ServerError" });

         routes.MapRoute("Robots.txt", "robots.txt", new { controller = "Seo", action = "Robots" });

         routes.MapRoute("SiteMap.xml", "sitemap.xml", new { controller = "Seo", action = "SiteMap" });

         RegisterBlogRoutes(routes);

         routes.MapRoute("TagCloud", "tag", new { controller = "Tag", action = "TagCloud" });
         
         routes.MapRoute("TopNav", "topnav", new { controller = "TopNavigation", action = "TopNavigation" });

         routes.MapRoute("Home", string.Empty, new { controller = "Blog", action = "List" });

         RegisterAccountRoutes(routes);

         routes.Add(new Route("metaweblogapi", null, null, new MetaWeblogRouteHandler()));

         routes.MapRoute("404", "{*url}", new { controller = "Error", action = "NotFound" });
      }

      private static void RegisterAccountRoutes(RouteCollection routes)
      {
         routes.MapRoute("Login", "Account/Login", new { controller = "Account", action = "Login" });

         routes.MapRoute("LogOff", "Account/LogOff", new { controller = "Account", action = "LogOff" });

         routes.MapRoute("Register", "Account/Register", new { controller = "Account", action = "Register" });

         routes.MapRoute("Manage", "Account/Manage", new { controller = "Account", action = "Manage" });

         routes.MapRoute("ManageLogins", "Account/ManageLogins", new { controller = "Account", action = "ManageLogins" });

         routes.MapRoute("SetPassword", "Account/SetPassword", new { controller = "Account", action = "SetPassword" });

         routes.MapRoute("LinkLogin", "Account/LinkLogin", new { controller = "Account", action = "LinkLogin" });

         routes.MapRoute("LinkLoginCallback", "Account/LinkLoginCallback", new { controller = "Account", action = "LinkLoginCallback" });

         routes.MapRoute("RemoveLogin", "Account/RemoveLogin", new { controller = "Account", action = "RemoveLogin" });

         routes.MapRoute("ExternalLogin", "Account/ExternalLogin", new { controller = "Account", action = "ExternalLogin" });

         routes.MapRoute("ExternalLoginCallback", "Account/ExternalLoginCallback", new { controller = "Account", action = "ExternalLoginCallback" });

         routes.MapRoute("ExternalLoginConfirmation", "Account/ExternalLoginConfirmation", new { controller = "Account", action = "ExternalLoginConfirmation" });

         routes.MapRoute("ExternalLoginFailure", "Account/ExternalLoginFailure", new { controller = "Account", action = "ExternalLoginFailure" });
      }

      private static void RegisterBlogRoutes(RouteCollection routes)
      {
         routes.MapRoute("BlogsFutures", "blog/future", new { controller = "Blog", action = "Future" });

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