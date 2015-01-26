// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostQueries.cs" company="Jane OSS">
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

   public class PostQueries : IPostQueries
   {
      private readonly ILoadStorage<Post, Guid> storage;
      
      private readonly MemoryCache cache = new MemoryCache("PostCache");

      public PostQueries(ILoadStorage<Post, Guid> storage)
      {
         Contract.Requires(storage != null);
         this.storage = storage;
      }

      public async Task<IEnumerable<Post>> GetAllPostsAsync()
      {
         return await this.GetPosts();
      }

      public async Task<IEnumerable<Post>> GetRecentPostsAsync()
      {
         var posts = await this.GetPosts();
         return posts.Take(5);
      }

      public async Task<PostView> GetPostBySlugAsync(string slug)
      {
         var posts = await this.GetPosts();
         posts = posts.OrderBy(post => post.PublishedDate).ToList();
         for (var i = 0; i < posts.Count; i++)
         {
            var post = posts[i];
            if (post.Slug.ToLowerInvariant() != slug.ToLowerInvariant())
            {
               continue;
            }

            var result = new PostView() { Post = post };
            if (i > 0)
            {
               result.PreviousPost = posts[i - 1];
            }

            if (i < posts.Count - 1)
            {
               result.NextPost = posts[i + 1];
            }

            return result;
         }

         return null;
      }

      public async Task<IEnumerable<Post>> GetPostsByTagAsync(string tag)
      {
         var posts = await this.GetPosts();
         return posts.Where(post => post.Tags.Contains(tag));
      }

      public async Task<IEnumerable<Post>> GetRelatedPostsAsync(Post post)
      {
         var posts = await this.GetPosts();
         var postsQuery = from relatedPost in posts
                          let count = post.Tags.Intersect(relatedPost.Tags).Count()
                          where count > 0
                          orderby count
                          select relatedPost;

         return postsQuery;
      }

      private async Task<IList<Post>> GetPosts()
      {
         const string KeyName = "Posts";
         var posts = this.cache.Get(KeyName) as IEnumerable<Post>;
         if (posts == null)
         {
            posts = await this.storage.LoadAsync();
            this.cache.Set(KeyName, posts, new CacheItemPolicy());
         }

         return posts.OrderByDescending(post => post.PublishedDate).ToList();
      }
   }
}