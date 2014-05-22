// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostQueriesJsonFactory.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the PostQueriesJsonFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System.Collections.Generic;
   using System.IO;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Newtonsoft.Json;

   public static class PostQueriesJsonFactory
   {
      public static IPostQueries Create(string path)
      {
         var json = new JsonSerializer();
         using (var stream = new StreamReader(path))
         {
            var posts = (List<Post>)json.Deserialize(stream, typeof(List<Post>));
            return new PostQueries(posts);
         }
      }
   }
}