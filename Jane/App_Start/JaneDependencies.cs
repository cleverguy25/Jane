// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JaneDependencies.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane
{
   using System;
   using System.IO;
   using System.Web.Hosting;

   using Jane.Identity.Models;
   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using SimpleInjector;

   using Post = Jane.Models.Post;

   public static class JaneDependencies
   {
      private static Container container;

      public static Container Container
      {
         get
         {
            return SetUpContainer();
         }
      }

      public static Container SetUpContainer()
      {
         if (container != null)
         {
            return container;
         }

         container = new Container();

         var rootPath = HostingEnvironment.MapPath("~");
         if (rootPath == null)
         {
            throw new Exception("Could not map root path");
         }

         var contentPath = Path.Combine(rootPath, @"content\posts\");
         var basePath = Path.Combine(rootPath, @"app_data\");
         var path = Path.Combine(rootPath, @"app_data\posts.json");
         var navPath = Path.Combine(rootPath, @"app_data\topNav.json");
         var futurePath = Path.Combine(rootPath, @"app_data\future.json");
         var userPath = Path.Combine(rootPath, @"app_data\users.json");
         var rolePath = Path.Combine(rootPath, @"app_data\roles.json");

         var storage = new PostJsonStorage(path, post => LocalPostContent.CreatePostContent(post, contentPath));

         container.Register<IStorage<Post, Guid>>(() => storage, Lifestyle.Singleton);
         container.Register<ILoadStorage<Post, Guid>>(() => storage, Lifestyle.Singleton);
         container.Register<ISaveStorage<Post, Guid>>(() => storage, Lifestyle.Singleton);

         container.Register<ILoadStorage<FuturePost, string>>(
            () => new JsonStorage<FuturePost, string>(futurePath, item => item.Title), 
            Lifestyle.Singleton);
         container.Register<ILoadStorage<NavigationItem, string>>(
            () => new JsonStorage<NavigationItem, string>(navPath, item => item.Name), 
            Lifestyle.Singleton);
         container.Register<IStorage<User, Guid>>(
            () => new JsonStorage<User, Guid>(userPath, item => item.Id), 
            Lifestyle.Singleton);
         container.Register<ILoadStorage<Role, Guid>>(
            () => new JsonStorage<Role, Guid>(rolePath, item => item.Id), 
            Lifestyle.Singleton);
         container.Register<ICommentStorage>(() => new CommentsStorage(basePath), Lifestyle.Singleton);

         container.Register<IPostQueries, PostQueries>();
         container.Register<IFuturePostQueries, FuturePostQueries>();
         container.Register<INavigationQueries, NavigationQueries>();
         container.Register<ITagQueries, TagQueries>(Lifestyle.Singleton);
         container.RegisterMvcControllers();

         container.Verify();

         MetaWeblog.MetaWeblog.ContentPath = contentPath;
         return container;
      }
   }
}