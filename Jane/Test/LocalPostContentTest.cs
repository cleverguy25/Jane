// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalPostContentTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System.IO;
   using System.Reflection;

   using FluentAssertions;

   using Jane.Infrastructure;
   using Jane.Models;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class LocalPostContentTest
   {
      [TestMethod, TestCategory("UnitTest")]
      public void GetContent()
      {
         string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         var post = new Post { Link = "test.html" };
         IPostContent postContent = LocalPostContent.CreatePostContent(post, Path.Combine(basePath, @"data\"));

         string content = postContent.GetContent();
         content.Should().Be("This is a test.");
      }
   }
}