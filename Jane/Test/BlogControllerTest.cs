// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogControllerTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using System.Web;
   using System.Web.Mvc;

   using FluentAssertions;

   using Jane.Controllers;
   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using Moq;

   using TestStack.FluentMVCTesting;

   [TestClass]
   public class BlogControllerTest
   {
      private IPostQueries postQuery;

      private BlogController blogController;

      [TestInitialize]
      public void Intialize()
      {
         this.postQuery = new PostQueries(FakePostData.GetStorage());
         var futurePostStorage = new Mock<ILoadStorage<FuturePost, string>>();
         futurePostStorage.Setup(storage => storage.LoadAsync())
               .Returns(Task.FromResult(new[] { new FuturePost() { PublishDate = DateTime.Today.AddDays(1), Title = "Future" } }.AsEnumerable())); 
         var futureQuery = new FuturePostQueries(futurePostStorage.Object);
         this.blogController = new BlogController(this.postQuery, futureQuery);
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task GetPostBySlug()
      {
         var result = await this.blogController.GetBySlug("post2") as ViewResult;
         var post = (PostView)result.Model;
         post.Post.Title.Should().Be("Post 2");
         post.NextPost.Title.Should().Be("Post 1");
         post.PreviousPost.Title.Should().Be("Post 3");
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task GetPostBySlugNotFound()
      {
         try
         {
            await this.blogController.GetBySlug("foo");
         }
         catch (HttpException error)
         {
            error.GetHttpCode().Should().Be(404);
         }
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task GetPostByTag()
      {
         var result = await this.blogController.GetByTag("foo") as ViewResult;
         var posts = ((IEnumerable<Post>)result.Model).ToList();
         posts.Count.Should().Be(2);
         posts[0].Slug.Should().Be("post1");
         posts[1].Slug.Should().Be("post2");
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task List()
      {
         var viewResult = await this.blogController.List() as ViewResult;
         var results = viewResult.Model as IEnumerable<Post>;
         var posts = results.ToList();
         posts[0].Title.Should().Be("Post 1");
         posts[1].Title.Should().Be("Post 2");
         posts[2].Title.Should().Be("Post 3");
         posts[3].Title.Should().Be("Post 5");
         posts[4].Title.Should().Be("Post 4");
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task GetRelatedBlogPostsOneMatchTag()
      {
         var viewResult = await this.blogController.Related("post4") as PartialViewResult;
         var posts = viewResult.Model as IEnumerable<Post>;

         posts.Should().HaveCount(1);
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task GetRelatedBlogPostsMoreThanOneMatchTag()
      {
         var viewResult = await this.blogController.Related("post3") as PartialViewResult;
         var posts = viewResult.Model as IEnumerable<Post>;

         var groups = posts.GroupBy(p => p.Title).ToList();
         groups.Should().HaveCount(3);
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task FutureQuery()
      {
         var viewResult = await this.blogController.Future() as PartialViewResult;
         var posts = viewResult.Model as IEnumerable<FuturePost>;

         posts.Should().HaveCount(1);
         posts.ToList()[0].GetExpectedWait(DateTime.Today).Should().Be("about 1 day");
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task ArchiveByYear()
      {
         var viewResult = await this.blogController.ArchiveByYear(2015) as ViewResult;
         var posts = viewResult.Model as IEnumerable<Post>;

         posts.Should().HaveCount(2);
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task ArchiveByYearMonth()
      {
         var viewResult = await this.blogController.ArchiveByYearMonth(2014, 4) as ViewResult;
         var posts = viewResult.Model as IEnumerable<Post>;

         posts.Should().HaveCount(1);
      }

      [TestMethod, TestCategory("UnitTest")]
      public async Task ArchiveList()
      {
         var viewResult = await this.blogController.ArchiveList() as PartialViewResult;
         var summaries = viewResult.Model as IEnumerable<ArchiveSummary>;

         summaries.Should().HaveCount(4);
         summaries.ToList()[2].Count.Should().Be(3);
      }
   }
}