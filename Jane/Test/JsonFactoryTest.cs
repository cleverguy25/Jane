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
   using System.Runtime.InteropServices;
   using System.Runtime.Remoting.Messaging;
   using System.Threading.Tasks;

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
      public async Task LoadPostJsonFile()
      {
         var postContent = new Mock<IPostContent>();
         var path = GetPath("posts.json");
         var postQueries = new PostQueries(new PostJsonStorage(path, (post) => postContent.Object));

         var items = await postQueries.GetRecentPostsAsync();
         var posts = items.ToList();
         posts.Should().HaveCount(3);

         // Test random properties
         posts[0].PublishedDate.ToShortDateString().Should().Be("5/14/2014");
         posts[1].Slug.Should().Be("second-post");
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task LoadFuturePostJsonFile()
      {
         var path = GetPath("future.json");
         var postQueries = new FuturePostQueries(new JsonStorage<FuturePost, string>(path, null));

         var items = await postQueries.GetFuturePostsAsync();
         var posts = items.ToList();
         posts.Should().HaveCount(2);

         // Test random properties
         posts[0].PublishDate.ToShortDateString().Should().Be("5/16/2014");
         posts[0].GetExpectedWait(new DateTime(2014, 5, 14)).Should().Be("about 2 days");
         posts[1].Title.Should().Be("The Future freaks me out");
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task LoadNavigationJsonFile()
      {
         var path = GetPath("topnav.json");
         var queries = new NavigationQueries(new JsonStorage<NavigationItem, string>(path, null));

         var items = await queries.GetNavigationItemsAsync();
         var posts = items.ToList();

         posts.Should().HaveCount(4);

         // Test random properties
         posts[0].Name.Should().Be("Home");
         posts[1].IconClass.Should().Be("fi-social-twitter");
         posts[3].Url.Should().Be("~/blog/rss");
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task DisposesStreamReaderCorrectlyAfterException()
      {
         var path = GetPath("postserror.json");
         try
         {
            await new PostJsonStorage(path, (post) => null).LoadAsync();
         }
         catch (JsonReaderException)
         {
         }

         File.Delete(path);
      }
   }
}