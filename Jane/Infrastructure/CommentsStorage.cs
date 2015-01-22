// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentsStorage.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading.Tasks;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Microsoft.Ajax.Utilities;

   using Newtonsoft.Json;

   public class CommentsStorage : ICommentStorage
   {
      private readonly string basePath;

      public CommentsStorage(string basePath)
      {
         this.basePath = basePath;
      }

      public Task<IEnumerable<Comment>> LoadAsync()
      {
         return Task.FromResult(new List<Comment>().AsEnumerable());
      }

      public Task<IEnumerable<Comment>> LoadByPostId(Guid postId)
      {
         var comments = this.LoadPostCommentsFromPath(postId);
         return Task.FromResult(comments);
      }

      public Task<Comment> LoadAsync(Guid id)
      {
         return Task.FromResult((Comment)null);
      }

      public async Task<Guid> AddAsync(Comment item)
      {
         var items = await this.LoadByPostId(item.PostId);
         var comments = items.ToList();
         comments.Add(item);
         this.Save(item.PostId, comments);
         return item.PostId;
      }

      public async Task UpdateAsync(Comment item)
      {
         var items = await this.LoadByPostId(item.PostId);
         var comments = items.ToList();
         var index = comments.FindIndex(comment => comment.Id == item.Id);
         if (index >= 0)
         {
            comments[index] = item;
            this.Save(item.PostId, comments);
         }
      }

      public async Task DeleteAsync(Comment item)
      {
         var items = await this.LoadByPostId(item.PostId);
         var comments = items.ToList();
         var index = comments.RemoveAll(comment => comment.Id == item.Id);
         this.Save(item.PostId, comments);
      }

      public Task DeleteAsync(Guid id)
      {
         return Task.FromResult(0);
      }

      private IEnumerable<Comment> LoadPostCommentsFromPath(Guid id)
      {
         var filePath = this.GetPostCommentsPath(id);
         if (File.Exists(filePath) == false)
         {
            return new Comment[0];
         }

         var json = new JsonSerializer();
         using (var stream = new StreamReader(filePath))
         {
            var items = (List<Comment>)json.Deserialize(stream, typeof(List<Comment>));

            return items;
         }
      }

      private void Save(Guid id, IEnumerable<Comment> items)
      {
         var filePath = this.GetPostCommentsPath(id);
         var json = new JsonSerializer();
         using (var stream = new StreamWriter(filePath))
         {
            json.Serialize(stream, items);
         }
      }
      
      private string GetPostCommentsPath(Guid id)
      {
         var postCommentPath = Path.Combine(this.basePath, "postComments-" + id + ".json");
         return postCommentPath;
      }
   }
}