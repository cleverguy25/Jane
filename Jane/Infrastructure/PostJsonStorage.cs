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

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Newtonsoft.Json;

   public class PostJsonStorage : ILoadStorage<Post>, ISaveStorage<Post>
   {
      private readonly string path;

      private readonly Func<Post, IPostContent> postContentFactory;

      public PostJsonStorage(string path, Func<Post, IPostContent> postContentFactory)
      {
         this.path = path;
         this.postContentFactory = postContentFactory;
      }

      public static ILoadStorage<Post> DefaultLoad { get; set; }

      public static ISaveStorage<Post> DefaultSave { get; set; }

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

      public Post Load(string id)
      {
         var postGuid = ConvertPostIdToGuid(id);
         return this.Load().FirstOrDefault(post => post.Guid == postGuid);
      }
      
      public void Add(Post item)
      {
         var posts = this.Load().ToList();
         posts.Add(item);
         SaveContent(item);
         this.Save(posts);
      }

      public void Update(Post item)
      {
         var posts = this.Load().ToList();
         var index = posts.FindIndex(post => post.Guid == item.Guid);
         posts[index] = item;
         SaveContent(item);
         this.Save(posts);
      }

      public void Delete(string id)
      {
         var postGuid = ConvertPostIdToGuid(id);
         var posts = this.Load().ToList();
         posts.RemoveAll(post => post.Guid == postGuid);
         this.Save(posts);
      }

      private static Guid ConvertPostIdToGuid(string postid)
      {
         Guid postGuid;
         if (Guid.TryParse(postid, out postGuid) == false)
         {
            throw new ArgumentException("postid should be a guid.", "postid");
         }

         return postGuid;
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