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

      [Then("should have the following posts")]
      public void ThenTheResultShouldBe(Table postsTable)
      {
         var posts = new Dictionary<string, Tuple<string, string>>();
         foreach (var postRow in postsTable.Rows)
         {
            var title = postRow["Title"];
            posts[title] = new Tuple<string, string>(postRow["Author"], postRow["PubDate"]);
         }

         var headings = this.browser.FindAllXPath("//header[@role='heading']").ToList();
         headings.Count.Should().Be(3);
         foreach (var heading in headings)
         {
            var title = heading.FindXPath(".//*[@itemprop='headline name']").Text;

            Tuple<string, string> post;
            if (posts.TryGetValue(title, out post))
            {
               var author = heading.FindXPath(".//*[@itemprop='author']");
               var name = author.FindXPath(".//*[@itemprop='name']").Text;
               name.Should().Be(post.Item1);
               var pubDate = heading.FindXPath(".//*[@itemprop='datePublished']").Text;
               pubDate.Should().Be(post.Item2);
            }
         }
      }

      [AfterScenario]
      public void AfterScenario()
      {
         this.browser.Dispose();
      }
   }
}