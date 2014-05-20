// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleErrorAndLogAttributeTest.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the ErrorHandling type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System;
   using System.Linq;
   using System.Web;
   using System.Web.Mvc;
   using System.Web.Routing;

   using FluentAssertions;

   using Jane.Infrastructure;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using Moq;

   [TestClass]
   public class HandleErrorAndLogAttributeTest
   {
      [TestMethod]
      public void AttributeLogs()
      {
         var httpContext = new Mock<HttpContextBase>();
         var entries = SemanticLoggingHelper.SubscribeToEvents();

         const string Controller = "FooController";
         const string Action = "FooAction";
         var exception = new ArgumentException("Foo");
         var routeData = new RouteData();
         routeData.Values["controller"] = Controller;
         routeData.Values["action"] = Action;
         var controllerContext = new ControllerContext(httpContext.Object, routeData, new TestController());
         var filterContext = new ExceptionContext(controllerContext, exception);
         
         var attribute = new HandleAndLogErrorAttribute();
         attribute.OnException(filterContext);

         var correlationId = filterContext.Controller.TempData["CorrelationId"];
         SemanticLoggingHelper.TestServerErrorEntry(entries, correlationId, Controller, Action, exception);
      }

      [TestMethod]
      public void RegisteredAsGlobalFilter()
      {
         var filters = new GlobalFilterCollection();
         FilterConfig.RegisterGlobalFilters(filters);

         filters.Any(filter => typeof(HandleAndLogErrorAttribute) == filter.Instance.GetType()).Should().BeTrue();
      }

      private class TestController : Controller
      {
      }
   }
}