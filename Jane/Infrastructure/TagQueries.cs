// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Runtime.Caching;
   using System.Threading.Tasks;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class TagQueries : ITagQueries
   {
      private readonly IPostQueries postQueries;

      private readonly MemoryCache cache = new MemoryCache("TagCounts");

      public TagQueries(IPostQueries postQueries)
      {
         Contract.Requires(postQueries != null);
         this.postQueries = postQueries;
      }

      public async Task<IEnumerable<Tuple<string, int>>> GetTagsWithCountsAsync()
      {
         const string KeyName = "TagCounts";
         var tagCounts = this.cache.Get(KeyName) as IEnumerable<Tuple<string, int>>;
         if (tagCounts == null)
         {
            var tagQuery = from post in await this.postQueries.GetAllPostsAsync()
                           from tag in post.Tags
                           group tag by tag
                              into tagGroup
                              select new Tuple<string, int>(tagGroup.Key, tagGroup.Count());

            tagCounts = tagQuery.ToList();
            this.cache.Set(KeyName, tagCounts, new CacheItemPolicy());
         }

         return tagCounts;
      }
   }
}