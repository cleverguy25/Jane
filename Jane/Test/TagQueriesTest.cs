// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagQueriesTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System.Linq;
   using System.Threading.Tasks;

   using FluentAssertions;

   using Jane.Infrastructure;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class TagQueriesTest
   {
      [TestMethod, TestCategory("UnitTest")]
      public async Task GetTagCounts()
      {
         var postQueries = new PostQueries(FakePostData.GetStorage());
         var tagQueries = new TagQueries(postQueries);

         var tags = await tagQueries.GetTagsWithCountsAsync();
         var counts = tags.ToList();

         counts.Should().HaveCount(3);
         counts[0].Item2.Should().Be(3);
         counts[1].Item2.Should().Be(2);
      }
   }
}