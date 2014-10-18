// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System;
   using System.IO;
   using System.Web;
   using System.Web.Mvc;
   using System.Web.Optimization;
   using System.Web.Routing;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Microsoft.Owin.Security;

   using SimpleInjector;
   using SimpleInjector.Integration.Web.Mvc;

   public class MvcApplication : HttpApplication
   {
      protected void Application_Start()
      {
         MvcHandler.DisableMvcResponseHeader = true;
         AreaRegistration.RegisterAllAreas();
         FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
         RouteConfig.RegisterRoutes(RouteTable.Routes);
         BundleConfig.RegisterBundles(BundleTable.Bundles);
      }

      protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
      {
         HttpContext.Current.Response.Headers.Remove("Server");
      }
   }
}