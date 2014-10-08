// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System.IO;
   using System.Web;
   using System.Web.Mvc;
   using System.Web.Optimization;
   using System.Web.Routing;
   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using SimpleInjector;
   using SimpleInjector.Integration.Web.Mvc;

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
         var navPath = Path.Combine(rootPath, @"app_data\topnav.json");

         var container = new Container();

         container.Register<IPostQueries>(
            () => PostQueriesJsonFactory.Create(path, (post) => LocalPostContent.CreatePostContent(post, contentPath)),
            Lifestyle.Singleton);

         container.Register<INavigationQueries>(
            () => NavigationQueriesJsonFactory.Create(navPath),
            Lifestyle.Singleton);
         container.Register<ITagQueries, TagQueries>(Lifestyle.Singleton);
         container.RegisterMvcControllers();

         container.Verify();

         DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
      }
   }
}