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
   using System.IO;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Newtonsoft.Json;

   public class PostJsonStorage : IStorage<Post>
   {
      private readonly string path;

      private readonly Func<Post, IPostContent> postContentFactory;

      public PostJsonStorage(string path, Func<Post, IPostContent> postContentFactory)
      {
         this.path = path;
         this.postContentFactory = postContentFactory;
      }

      public IEnumerable<Post> Load()
      {
         var json = new JsonSerializer();
         using (var stream = new StreamReader(this.path))
         {
            var posts = (List<Post>)json.Deserialize(stream, typeof(List<Post>));
            foreach (var post in posts)
            {
               post.Content = this.postContentFactory(post);
            }

            return posts;
         }
      }

      public void Save(IEnumerable<Post> posts)
      {
         throw new NotImplementedException();
      }
   }
}