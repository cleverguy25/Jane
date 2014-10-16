// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuturePostQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Threading.Tasks;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class FuturePostQueries : IFuturePostQueries
   {
      private readonly ILoadStorage<FuturePost, string> futurePostsStorage;

      private IEnumerable<FuturePost> futurePosts;

      public FuturePostQueries(ILoadStorage<FuturePost, string> futurePostsStorage)
      {
         Contract.Requires(futurePostsStorage != null);

         this.futurePostsStorage = futurePostsStorage;
      }

      public async Task<IEnumerable<FuturePost>> GetFuturePostsAsync()
      {
         if (this.futurePosts == null)
         {
            this.futurePosts = await this.futurePostsStorage.LoadAsync();
         }

         return this.futurePosts;
      }
   }
}