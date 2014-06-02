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
      private readonly Post[] posts =
         {
            new Post
               {
                  Slug = "post1", 
                  Title = "Post 1", 
                  PublishedDate = new DateTime(2014, 5, 12)
               }, 
            new Post
               {
                  Slug = "post2", 
                  Title = "Post 2", 
                  PublishedDate = new DateTime(2014, 5, 15)
               }, 
            new Post
               {
                  Slug = "post3", 
                  Title = "Post 3", 
                  PublishedDate = new DateTime(2014, 5, 16)
               }, 
            new Post
               {
                  Slug = "post4", 
                  Title = "Post 4", 
                  PublishedDate = new DateTime(2014, 5, 3)
               }, 
            new Post
               {
                  Slug = "post5", 
                  Title = "Post 5", 
                  PublishedDate = new DateTime(2014, 5, 5)
               }, 
            new Post
               {
                  Slug = "post6", 
                  Title = "Post 6", 
                  PublishedDate = new DateTime(2014, 5, 21)
               }
         };

      private IPostQueries postQuery;

      private BlogController blogController;

      [TestInitialize]
      public void Intialize()
      {
         this.postQuery = new PostQueries(this.posts);
         this.blogController = new BlogController(this.postQuery);
      }

      [TestMethod]
      public void GetPostBySlug()
      {
         var result = this.blogController.GetBySlug("post2") as ViewResult;
         var post = (Post)result.Model;
         post.Title.Should().Be("Post 2");
      }

      [TestMethod]
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

      [TestMethod]
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
   }
}