// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaWeblogRouteHandler.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.MetaWeblog
{
   using System.Web;
   using System.Web.Routing;

   using Jane.Infrastructure;

   public class MetaWeblogRouteHandler : IRouteHandler
   {
      public IHttpHandler GetHttpHandler(RequestContext requestContext)
      {
         return new MetaWeblog(PostJsonStorage.DefaultLoad, PostJsonStorage.DefaultSave);
      }
   }
}