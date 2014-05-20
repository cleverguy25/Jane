﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the ErrorController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Controllers
{
   using System.Net;
   using System.Web.Mvc;

   public class ErrorController : Controller
   {
      public ActionResult NotFound()
      {
         this.Response.StatusCode = (int)HttpStatusCode.NotFound;

         return this.View();
      }

      public ActionResult ServerError()
      {
         this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

         return this.View("Error");
      }
   }
}