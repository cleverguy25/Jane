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
   using System.IO;
   using System.Web;
   using System.Web.Mvc;
   using System.Web.Routing;

   public static class ControllerHelper
   {
      public static void SetupControllerContext(this Controller controller)
      {
         var request = new HttpRequest(string.Empty, "http://localhost/", string.Empty);
         var response = new HttpResponse(TextWriter.Null);
         var httpContext = new HttpContextWrapper(new HttpContext(request, response));
         controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);
      }
   }
}