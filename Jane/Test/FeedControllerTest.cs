// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeedControllerTest.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//    Feed controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System;
   using System.IO;
   using System.Linq;
   using System.Text;
   using System.Xml.Linq;

   using FluentAssertions;

   using Jane.Controllers;
   using Jane.Infrastructure;
   using Jane.Models;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class FeedControllerTest
   {
      [TestMethod, TestCategory("UnitTest")]
      public void Rss()
      {
         var stringBuilder = new StringBuilder();
         var writer = new StringWriter(stringBuilder);
         var postQueries = new PostQueries(FakePostData.Posts);
         var controller = new FeedController(postQueries);
         controller.SetupControllerContext(writer, "http://localhost/blog/rss");

         controller.Rss();

         controller.Response.ContentType.Should().Be("application/rss+xml");
         var document = XDocument.Parse(stringBuilder.ToString());

         var channel = document.Descendants("channel").First();
         CheckItem(channel, "My Blog", "My Blog Description", "http://localhost/blog");

         var items = document.Descendants("item").ToList();
         CheckItem(items[3], FakePostData.Posts[0]);
         items.Count.Should().Be(6);
      }

      [TestMethod, TestCategory("UnitTest")]
      public void Atom()
      {
         var stringBuilder = new StringBuilder();
         var writer = new StringWriter(stringBuilder);
         var postQueries = new PostQueries(FakePostData.Posts);
         var controller = new FeedController(postQueries);
         controller.SetupControllerContext(writer, "http://localhost/blog/atom");

         controller.Atom();

         controller.Response.ContentType.Should().Be("application/atom+xml");
         var document = XDocument.Parse(stringBuilder.ToString());

         XNamespace a10 = "http://www.w3.org/2005/Atom";
         var entries = document.Descendants(a10 + "entry").ToList();
         entries.Count.Should().Be(6);
      }

      private static void CheckItem(XElement item, Post post)
      {
         CheckItem(item, post.Title, post.Summary, "http://localhost/blog/" + post.Slug, post.UpdatedDate);
      }

      private static void CheckItem(
         XElement item,
         string title,
         string description,
         string link,
         DateTime? updateDateTime = null)
      {
         item.Element("title").Value.Should().Be(title);
         item.Element("description").Value.Should().Be(description);
         item.Element("link").Value.Should().Be(link);

         if (updateDateTime != null)
         {
            XNamespace a10 = "http://www.w3.org/2005/Atom";
            item.Element(a10 + "updated").Value.Should().Be(updateDateTime.Value.ToString("yyy-MM-ddThh:mm:ssZ"));
         }
      }
   }
}