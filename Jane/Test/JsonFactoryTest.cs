// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonFactoryTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System;
   using System.IO;
   using System.Linq;
   using System.Reflection;
   using System.Runtime.Remoting.Messaging;

   using FluentAssertions;

   using Jane.Infrastructure;
   using Jane.Models;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using Moq;

   using Newtonsoft.Json;

   [TestClass]
   public class JsonFactoryTest
   {
      public static string GetPath(string fileName)
      {
         var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         var path = Path.Combine(basePath, @"data\" + fileName);

         return path;
      }

      [TestMethod, TestCategory("UnitTest")]
      public void LoadPostJsonFile()
      {
         var postContent = new Mock<IPostContent>();
         var path = GetPath("posts.json");
         var postQueries = PostQueriesJsonFactory.Create(path, (post) => postContent.Object);

         var posts = postQueries.GetRecentPosts().ToList();
         posts.Should().HaveCount(3);

         // Test random properties
         posts[0].PublishedDate.ToShortDateString().Should().Be("5/14/2014");
         posts[1].Slug.Should().Be("second-post");
      }

      [TestMethod, TestCategory("UnitTest")]
      public void LoadFuturePostJsonFile()
      {
         var path = GetPath("future.json");
         var postQueries = FuturePostQueriesJsonFactory.Create(path);

         var posts = postQueries.GetFuturePosts().ToList();
         posts.Should().HaveCount(2);

         // Test random properties
         posts[0].PublishDate.ToShortDateString().Should().Be("5/16/2014");
         posts[0].GetExpectedWait(new DateTime(2014, 5, 14)).Should().Be("about 2 days");
         posts[1].Title.Should().Be("The Future freaks me out");
      }

      [TestMethod, TestCategory("UnitTest")]
      public void LoadNavigationJsonFile()
      {
         var path = GetPath("topnav.json");
         var queries = NavigationQueriesJsonFactory.Create(path);

         var posts = queries.GetNavigationItems().ToList();
         posts.Should().HaveCount(4);

         // Test random properties
         posts[0].Name.Should().Be("Home");
         posts[1].IconClass.Should().Be("fi-social-twitter");
         posts[3].Url.Should().Be("~/blog/rss");
      }

      [TestMethod, TestCategory("UnitTest")]
      public void DisposesStreamReaderCorrectlyAfterException()
      {
         var path = GetPath("postserror.json");
         try
         {
            PostQueriesJsonFactory.Create(path, (post) => null);
         }
         catch (JsonReaderException)
         {
         }

         File.Delete(path);
      }
   }
}