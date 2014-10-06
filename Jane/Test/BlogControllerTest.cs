// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogControllerTest.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the BlogControllerTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Web;
   using System.Web.Mvc;

   using FluentAssertions;

   using Jane.Controllers;
   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using TestStack.FluentMVCTesting;

   [TestClass]
   public class BlogControllerTest
   {
      private IPostQueries postQuery;

      private BlogController blogController;

      [TestInitialize]
      public void Intialize()
      {
         this.postQuery = new PostQueries(FakePostData.Posts);
         this.blogController = new BlogController(this.postQuery);
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostBySlug()
      {
         var result = this.blogController.GetBySlug("post2") as ViewResult;
         var post = (Post)result.Model;
         post.Title.Should().Be("Post 2");
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostBySlugNotFound()
      {
         try
         {
            this.blogController.GetBySlug("foo");
         }
         catch (HttpException error)
         {
            error.GetHttpCode().Should().Be(404);
         }
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetPostByTag()
      {
         var result = this.blogController.GetByTag("foo") as ViewResult;
         var posts = ((IEnumerable<Post>)result.Model).ToList();
         posts.Count.Should().Be(2);
         posts[0].Slug.Should().Be("post2");
         posts[1].Slug.Should().Be("post1");
      }

      [TestMethod, TestCategory("UnitTest")]
      public void List()
      {
         var viewResult = this.blogController.List() as ViewResult;
         var results = viewResult.Model as IEnumerable<Post>;
         var posts = results.ToList();
         posts[0].Title.Should().Be("Post 6");
         posts[1].Title.Should().Be("Post 3");
         posts[2].Title.Should().Be("Post 2");
         posts[3].Title.Should().Be("Post 1");
         posts[4].Title.Should().Be("Post 5");
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetRelatedBlogPostsOneMatchTag()
      {
         var viewResult = this.blogController.Related("post4") as PartialViewResult;
         var posts = viewResult.Model as IEnumerable<Post>;

         posts.Should().HaveCount(1);
      }

      [TestMethod, TestCategory("UnitTest")]
      public void GetRelatedBlogPostsMoreThanOneMatchTag()
      {
         var viewResult = this.blogController.Related("post3") as PartialViewResult;
         var posts = viewResult.Model as IEnumerable<Post>;

         var groups = posts.GroupBy(p => p.Title).ToList();
         groups.Should().HaveCount(3);
      }
   }
}