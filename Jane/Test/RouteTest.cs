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
      public void NotFoundRoute()
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
      public void ServerError()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/servererror", new { controller = "Error", action = "ServerError" });
      }

      [TestMethod]
      public void Home()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/", new { controller = "Blog", action = "List" });
      }

      [TestMethod]
      public void BlogList()
      {
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);

         routes.AssertRoute("~/blog", new { controller = "Blog", action = "List" });
      }
   }
}