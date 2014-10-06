// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleAndLogErrorAttribute.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System;
   using System.Web.Mvc;
   
   public class HandleAndLogErrorAttribute : HandleErrorAttribute
   {
      public override void OnException(ExceptionContext filterContext)
      {
         var correlationId = Guid.NewGuid();
         filterContext.Controller.TempData["CorrelationId"] = correlationId.ToString();

         var controllerName = (string)filterContext.RouteData.Values["controller"];
         var action = (string)filterContext.RouteData.Values["action"];
         var exception = filterContext.Exception;

         Tracing.Log.ServerError(
            correlationId.ToString(),
            controllerName,
            action,
            exception.Message,
            exception.ToString());

         base.OnException(filterContext);
      }
   }
}