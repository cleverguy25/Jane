// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostJsonStorage.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Dynamic;
   using System.IO;
   using System.Linq;
   using System.Threading.Tasks;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Newtonsoft.Json;

   public class PostJsonStorage : IStorage<Post, Guid>
   {
      private readonly string path;

      private readonly Func<Post, IPostContent> postContentFactory;

      public PostJsonStorage(string path, Func<Post, IPostContent> postContentFactory)
      {
         this.path = path;
         this.postContentFactory = postContentFactory;
      }

      public Task<IEnumerable<Post>> LoadAsync()
      {
         var json = new JsonSerializer();
         using (var stream = new StreamReader(this.path))
         {
            var posts = (List<Post>)json.Deserialize(stream, typeof(List<Post>));
            foreach (var post in posts)
            {
               post.Content = this.postContentFactory(post);
            }

            return Task.FromResult(posts.AsEnumerable());
         }
      }

      public async Task<Post> LoadAsync(Guid id)
      {
         var items = await this.LoadAsync();
         return items.FirstOrDefault(post => post.Guid == id);
      }

      public async Task<Guid> AddAsync(Post item)
      {
         var items = await this.LoadAsync();
         var posts = items.ToList();
         posts.Add(item);
         SaveContent(item);
         this.Save(posts);
         return item.Guid;
      }

      public async Task UpdateAsync(Post item)
      {
         var items = await this.LoadAsync();
         var posts = items.ToList();
         var index = posts.FindIndex(post => post.Guid == item.Guid);
         posts[index] = item;
         SaveContent(item);
         this.Save(posts);
      }

      public Task DeleteAsync(Post item)
      {
         return this.DeleteAsync(item.Guid);
      }

      public async Task DeleteAsync(Guid id)
      {
         var items = await this.LoadAsync();
         var posts = items.ToList();
         posts.RemoveAll(post => post.Guid == id);
         this.Save(posts);
      }

      private static void SaveContent(Post post)
      {
         if (post.Link == null)
         {
            post.Link = post.Slug + ".html";
         }

         post.Content.Save(post.Link);
      }

      private void Save(IEnumerable<Post> posts)
      {
         posts = posts.ToList();
         foreach (var post in posts)
         {
            post.Content = null;
         }

         var json = new JsonSerializer();
         using (var stream = new StreamWriter(this.path))
         {
            json.Serialize(stream, posts);
         }
      }
   }
}