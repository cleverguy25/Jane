// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Controllers
{
   using System.Net;
   using System.Web.Mvc;

   public class ErrorController : Controller
   {
      public ActionResult NotFound()
      {
         Response.StatusCode = (int)HttpStatusCode.NotFound;

         return this.View();
      }

      public ActionResult ServerError()
      {
         Response.StatusCode = (int)HttpStatusCode.InternalServerError;

         return this.View("Error");
      }
   }
}