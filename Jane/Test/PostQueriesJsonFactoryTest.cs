// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostQueriesJsonFactoryTest.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the PostQueryJsonFactoryTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System.IO;
   using System.Linq;
   using System.Reflection;

   using FluentAssertions;

   using Jane.Infrastructure;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using Newtonsoft.Json;

   [TestClass]
   public class PostQueriesJsonFactoryTest
   {
      public static string GetPath(string fileName)
      {
         var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         var path = Path.Combine(basePath, @"data\" + fileName);

         return path;
      }

      [TestMethod]
      public void LoadJsonFile()
      {
         var path = GetPath("posts.json");
         var postQueries = PostQueriesJsonFactory.Create(path);

         var posts = postQueries.GetRecentPosts().ToList();
         posts.Should().HaveCount(3);

         // Test random properties
         posts[0].PublishedDate.ToShortDateString().Should().Be("5/14/2014");
         posts[1].Slug.Should().Be("second-post");
      }
      
      [TestMethod]
      public void DisposesStreamReaderCorrectlyAfterException()
      {
         var path = GetPath("postserror.json");
         try
         {
            PostQueriesJsonFactory.Create(path);
         }
         catch (JsonReaderException)
         {
         }

         File.Delete(path);
      }
   }
}