// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the PostQueries type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System.Collections.Generic;
   using System.Linq;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class PostQueries : IPostQueries
   {
      private readonly List<Post> posts;

      public PostQueries(IEnumerable<Post> posts)
      {
         this.posts = posts.OrderByDescending(post => post.PublishedDate).ToList();
      }

      public IEnumerable<Post> GetRecentPosts()
      {
         return this.posts.Take(5);
      }
   }
}