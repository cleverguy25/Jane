// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the MvcApplication type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System.IO;
   using System.Web;
   using System.Web.Mvc;
   using System.Web.Optimization;
   using System.Web.Routing;

   using Jane.Infrastructure.Interfaces;
   using Jane.LightInject;

   public class MvcApplication : HttpApplication
   {
      protected void Application_Start()
      {
         AreaRegistration.RegisterAllAreas();
         FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
         RouteConfig.RegisterRoutes(RouteTable.Routes);
         BundleConfig.RegisterBundles(BundleTable.Bundles);

         this.SetUpContainer();
      }

      private void SetUpContainer()
      {
         var container = new ServiceContainer();

         container.RegisterControllers();

         container.EnableMvc();
      }
   }
}