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

         this.SetUpContainer();
      }

      protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
      {
         HttpContext.Current.Response.Headers.Remove("Server");
      }

      private void SetUpContainer()
      {
         var rootPath = Server.MapPath("~");
         var contentPath = Path.Combine(rootPath, @"content\posts\");
         var path = Path.Combine(rootPath, @"app_data\posts.json");
         var navPath = Path.Combine(rootPath, @"app_data\topNav.json");
         var futurePath = Path.Combine(rootPath, @"app_data\future.json");

         var container = new Container();

         container.Register<IStorage<Post>>(() => new PostJsonStorage(path, (post) => LocalPostContent.CreatePostContent(post, contentPath)), Lifestyle.Singleton);
         container.Register<IStorage<FuturePost>>(() => new JsonStorage<FuturePost>(futurePath), Lifestyle.Singleton);
         container.Register<IStorage<NavigationItem>>(() => new JsonStorage<NavigationItem>(navPath), Lifestyle.Singleton);
         
         container.Register<IPostQueries, PostQueries>();
         container.Register<IFuturePostQueries, FuturePostQueries>();
         container.Register<INavigationQueries, NavigationQueries>();
         container.Register<ITagQueries, TagQueries>(Lifestyle.Singleton);
         container.RegisterMvcControllers();

         container.Verify();

         DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
      }
   }
}