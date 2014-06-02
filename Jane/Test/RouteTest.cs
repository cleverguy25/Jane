// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteTest.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the RouteTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System.Web.Routing;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class RouteTest
   {
      [TestMethod]
      public void NotFoundRouteRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/notfound", new { controller = "Error", action = "NotFound" });
      }

      [TestMethod]
      public void Page404Route()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/asdf", new { controller = "Error", action = "NotFound" });
      }

      [TestMethod]
      public void ServerErrorRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/servererror", new { controller = "Error", action = "ServerError" });
      }

      [TestMethod]
      public void HomeRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/", new { controller = "Blog", action = "List" });
      }

      [TestMethod]
      public void BlogListRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog", new { controller = "Blog", action = "List" });
      }

      [TestMethod]
      public void GetPostBySlugRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/first-post", new { controller = "Blog", action = "GetBySlug", slug = "first-post" });
      }

      [TestMethod]
      public void RssRoute()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog/rss", new { controller = "Feed", action = "Rss" });
      }
   }
}