// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerHelper.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the ControllerTestHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System;
   using System.IO;
   using System.Security.Policy;
   using System.Web;
   using System.Web.Mvc;
   using System.Web.Routing;

   using Moq;

   public static class ControllerHelper
   {
      public static void SetupControllerContext(this Controller controller, string url = "http://localhost/")
      {
         SetupControllerContext(controller, TextWriter.Null, url);
      }

      public static void SetupControllerContext(this Controller controller, TextWriter textWriter, string url = "http://localhost/")
      {
         var request = new HttpRequest(string.Empty, url, string.Empty);
         var response = new HttpResponse(textWriter);
         var httpContext = new HttpContextWrapper(new HttpContext(request, response));
         controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);
         
         var routes = new RouteCollection();
         RouteConfig.RegisterRoutes(routes);
         
         controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);
         controller.Url = new UrlHelper(new RequestContext(httpContext, new RouteData()), routes);
      }
   }
}