// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalPostContent.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System.IO;

   using Jane.Models;

   public class LocalPostContent : IPostContent
   {
      private readonly Post post;

      private readonly string contentPath;

      private string content;

      public LocalPostContent(Post post, string contentPath)
      {
         this.post = post;
         this.contentPath = contentPath;
      }

      public static IPostContent CreatePostContent(Post post, string contentPath)
      {
         return new LocalPostContent(post, contentPath);
      }

      public string GetContent()
      {
         if (this.content == null)
         {
            var postPath = Path.Combine(this.contentPath, this.post.Link);
            this.content = File.ReadAllText(postPath);
         }

         return this.content;
      }

      public void SetContent(string newContent)
      {
         this.content = newContent;
      }

      public void Save(string fileName)
      {
         var postPath = Path.Combine(this.contentPath, fileName);
         File.WriteAllText(postPath, this.content);
      }
   }
}