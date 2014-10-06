// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagQueriesTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System.Linq;

   using FluentAssertions;

   using Jane.Infrastructure;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class TagQueriesTest
   {
      [TestMethod, TestCategory("UnitTest")]
      public void GetTagCounts()
      {
         var postQueries = new PostQueries(FakePostData.Posts);
         var tagQueries = new TagQueries(postQueries);

         var counts = tagQueries.GetTagsWithCounts().ToList();

         counts.Should().HaveCount(3);
         counts[0].Item2.Should().Be(3);
         counts[1].Item2.Should().Be(2);
      }
   }
}