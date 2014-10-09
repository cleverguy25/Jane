// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuturePostQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System.Collections.Generic;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class FuturePostQueries : IFuturePostQueries
   {
      private readonly IEnumerable<FuturePost> futurePosts;

      public FuturePostQueries(IEnumerable<FuturePost> futurePosts)
      {
         this.futurePosts = futurePosts;
      }

      public IEnumerable<FuturePost> GetFuturePosts()
      {
         return this.futurePosts;
      }
   }
}