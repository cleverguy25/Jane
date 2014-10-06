// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure.Interfaces
{
   using System.Collections.Generic;

   using Jane.Models;

   public interface IPostQueries
   {
      IEnumerable<Post> GetAllPosts();

      IEnumerable<Post> GetRecentPosts();

      IEnumerable<Post> GetRelatedPosts(Post post);

      Post GetPostBySlug(string slug);

      IEnumerable<Post> GetPostsByTag(string tag);
   }
}