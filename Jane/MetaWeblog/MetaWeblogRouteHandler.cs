// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaWeblogRouteHandler.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.MetaWeblog
{
   using System;
   using System.Web;
   using System.Web.Routing;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;

   public class MetaWeblogRouteHandler : IRouteHandler
   {
      public IHttpHandler GetHttpHandler(RequestContext requestContext)
      {
         var storage = JaneDependencies.Container.GetInstance<IStorage<Models.Post, Guid>>();
         return new MetaWeblog(storage);
      }
   }
}