// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleAndLogErrorAttribute.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the HandleAndLogErrorAttribute type.
// </summary>
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

         base.OnException(filterContext);
      }
   }
}