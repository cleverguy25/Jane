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

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class TagQueries : ITagQueries
   {
      private readonly List<Tuple<string, int>> tagCounts;

      public TagQueries(IPostQueries postQueries)
      {
         Contract.Requires(postQueries != null);

         var tagQuery = from post in postQueries.GetAllPosts()
                        from tag in post.Tags
                        group tag by tag
                        into tagGroup
                        select new Tuple<string, int>(tagGroup.Key, tagGroup.Count());

         this.tagCounts = tagQuery.ToList();
      }

      public IEnumerable<Tuple<string, int>> GetTagsWithCounts()
      {
         return this.tagCounts;
      }
   }
}