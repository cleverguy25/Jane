// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System.Web.Routing;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class RouteTest
   {
      private RouteCollection routes;

      [TestInitialize]
      public void Setup()
      {
         this.routes = new RouteCollection();
         RouteConfig.RegisterRoutes(this.routes);
      }

      [TestMethod, TestCategory("UnitTest")]
      
      public void NotFoundRouteRoute()
      {
         this.routes.AssertRoute("~/notfound", new { controller = "Error", action = "NotFound" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void Page404Route()
      {
         this.routes.AssertRoute("~/asdf", new { controller = "Error", action = "NotFound" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void ServerErrorRoute()
      {
         this.routes.AssertRoute("~/servererror", new { controller = "Error", action = "ServerError" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void HomeRoute()
      {
         this.routes.AssertRoute("~/", new { controller = "Blog", action = "List" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void BlogListRoute()
      {
         this.routes.AssertRoute("~/blog", new { controller = "Blog", action = "List" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostBySlugRoute()
      {
         this.routes.AssertRoute(
            "~/blog/first-post",
            new { controller = "Blog", action = "GetBySlug", slug = "first-post" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostRelatedRoute()
      {
         this.routes.AssertRoute(
            "~/blog/first-post/related",
            new { controller = "Blog", action = "Related", slug = "first-post" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostByTagRoute()
      {
         this.routes.AssertRoute("~/blog/tagged/foo", new { controller = "Blog", action = "GetByTag", tag = "foo" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void TagRoute()
      {
         this.routes.AssertRoute("~/tag", new { controller = "Tag", action = "TagCloud" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void BlogRecentRoute()
      {
         this.routes.AssertRoute("~/blog/recent", new { controller = "Blog", action = "Recent" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void BlogFutureRoute()
      {
         this.routes.AssertRoute("~/blog/future", new { controller = "Blog", action = "Future" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void RsdRoute()
      {
         this.routes.AssertRoute("~/rsd", new { controller = "Feed", action = "Rsd" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void RssRoute()
      {
         this.routes.AssertRoute("~/blog/rss", new { controller = "Feed", action = "Rss" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void RssByTagRoute()
      {
         this.routes.AssertRoute("~/blog/rss/tagged/foo", new { controller = "Feed", action = "RssByTag", tag = "foo" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void AtomRoute()
      {
         this.routes.AssertRoute("~/blog/atom", new { controller = "Feed", action = "Atom" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void AtomByTagRoute()
      {
         this.routes.AssertRoute(
            "~/blog/atom/tagged/foo",
            new { controller = "Feed", action = "AtomByTag", tag = "foo" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void SiteMapRoute()
      {
         this.routes.AssertRoute("~/sitemap.xml", new { controller = "Seo", action = "SiteMap" });
      }

      [TestMethod, TestCategory("UnitTest")]
      public void RobotsRoute()
      {
         this.routes.AssertRoute("~/robots.txt", new { controller = "Seo", action = "Robots" });
      }
   }
}