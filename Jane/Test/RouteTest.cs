// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteTest.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System.Web.Routing;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class RouteTest
   {
      [TestMethod, TestCategory("UnitTest")]
      public void NotFoundRouteRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/notfound", new { controller = "Error", action = "NotFound" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void Page404Route()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/asdf", new { controller = "Error", action = "NotFound" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void ServerErrorRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/servererror", new { controller = "Error", action = "ServerError" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void HomeRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/", new { controller = "Blog", action = "List" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void BlogListRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog", new { controller = "Blog", action = "List" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostBySlugRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/first-post", new { controller = "Blog", action = "GetBySlug", slug = "first-post" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostRelatedRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/first-post/related", new { controller = "Blog", action = "Related", slug = "first-post" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostByTagRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/tagged/foo", new { controller = "Blog", action = "GetByTag", tag = "foo" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void TagRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/tag", new { controller = "Tag", action = "TagCloud" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void RssRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/rss", new { controller = "Feed", action = "Rss" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void RssByTagRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/rss/tagged/foo", new { controller = "Feed", action = "RssByTag", tag = "foo" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void AtomRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/atom", new { controller = "Feed", action = "Atom" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void AtomByTagRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/atom/tagged/foo", new { controller = "Feed", action = "AtomByTag", tag = "foo" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void SiteMapRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/sitemap.xml", new { controller = "Seo", action = "SiteMap" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void RobotsRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/robots.txt", new { controller = "Seo", action = "Robots" });
      }
   }
}