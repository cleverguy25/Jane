// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class PostQueries : IPostQueries
   {
      private readonly List<Post> posts;

      public PostQueries(IStorage<Post> storage)
      {
         Contract.Requires(storage != null);

         this.posts = storage.Load().OrderByDescending(post => post.PublishedDate).ToList();
      }

      public IEnumerable<Post> GetAllPosts()
      {
         return this.posts;
      }

      public IEnumerable<Post> GetRecentPosts()
      {
         return this.posts.Take(5);
      }

      public Post GetPostBySlug(string slug)
      {
         return this.posts.FirstOrDefault(post => post.Slug.ToLowerInvariant() == slug.ToLowerInvariant());
      }

      public IEnumerable<Post> GetPostsByTag(string tag)
      {
         return this.posts.Where(post => post.Tags.Contains(tag));
      }

      public IEnumerable<Post> GetRelatedPosts(Post post)
      {
         var postsQuery = from relatedPost in this.posts
                          let count = post.Tags.Intersect(relatedPost.Tags).Count()
                          where count > 0
                          orderby count
                          select relatedPost;

         return postsQuery;
      }
   }
}