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

   using Antlr.Runtime.Misc;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.LightInject;
   using Jane.Models;

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
         var rootPath = Server.MapPath("~");
         var contentPath = Path.Combine(rootPath, @"content\posts\");
         var path = Path.Combine(rootPath, @"app_data\posts.json");

         var container = new ServiceContainer();

         container.Register<IPostQueries>(
            factory =>
               PostQueriesJsonFactory.Create(path, (post) => LocalPostContent.CreatePostContent(post, contentPath)),
            new PerContainerLifetime());
         container.Register<ITagQueries, TagQueries>(new PerContainerLifetime());
         container.RegisterControllers();

         container.EnableMvc();
      }
   }
}