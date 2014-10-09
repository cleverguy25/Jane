// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostQueriesJsonFactory.cs" company="Jane OSS">
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

   public static class PostQueriesJsonFactory
   {
      public static IPostQueries Create(string path, Func<Post, IPostContent> postContentFactory)
      {
         Contract.Requires<ArgumentNullException>(string.IsNullOrEmpty(path) == false, "path");
         Contract.Requires<ArgumentNullException>(postContentFactory != null, "postContentFactory");

         var json = new JsonSerializer();
         using (var stream = new StreamReader(path))
         {
            var posts = (List<Post>)json.Deserialize(stream, typeof(List<Post>));
            foreach (var post in posts)
            {
               post.Content = postContentFactory(post);
            }

            return new PostQueries(posts);
         }
      }
   }
}