// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogSpec.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the BlogSpec type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Net.Mime;

   using Coypu;
   using Coypu.Drivers;
   using Coypu.Drivers.Selenium;

   using FluentAssertions;

   using TechTalk.SpecFlow;
   
   [Binding]
   public class BlogSpec
   {
      private BrowserSession browser;

      [BeforeScenario]
      public void BeforeScenario()
      {
         var session = new SessionConfiguration()
         {
            Driver = typeof(SeleniumWebDriver),
            Browser = Browser.PhantomJS
         };
         this.browser = new BrowserSession(session);
      }

      [When("I navigate to '(.*)'")]
      public void Navigate(string url)
      {
         this.browser.Visit(url);
      }

      [Then("I should see the following posts")]
      public void ThenTheResultShouldBe(Table postsTable)
      {
         var expectedPosts = ExtractPosts(postsTable);

         var posts = this.browser.FindAllXPath(".//article[@itemprop='blogPost']").ToList();
         posts.Count.Should().Be(expectedPosts.Count);
         foreach (var post in posts)
         {
            var heading = post.FindXPath(".//header[@role='heading']");
            var title = heading.FindXPath(".//*[@itemprop='headline name']").Text;

            Tuple<string, string> expectedPost;
            if (expectedPosts.TryGetValue(title, out expectedPost))
            {
               var author = heading.FindXPath(".//*[@itemprop='author']");
               var name = author.FindXPath(".//*[@itemprop='name']").Text;
               name.Should().Be(expectedPost.Item1);
               var pubDate = heading.FindXPath(".//*[@itemprop='datePublished']").Text;
               pubDate.Should().Be(expectedPost.Item2);
            }

            var content = post.FindXPath(".//*[@itemprop='articleBody']");
            content.InnerHTML.Should().NotBeEmpty();
         }
      }
      
      [AfterScenario]
      public void AfterScenario()
      {
         this.browser.Dispose();
      }

      private static Dictionary<string, Tuple<string, string>> ExtractPosts(Table postsTable)
      {
         var posts = new Dictionary<string, Tuple<string, string>>();
         foreach (var postRow in postsTable.Rows)
         {
            var title = postRow["Title"];
            posts[title] = new Tuple<string, string>(postRow["Author"], postRow["PubDate"]);
         }

         return posts;
      }
   }
}