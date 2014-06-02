// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeedController.cs" company="Jane">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.ServiceModel.Syndication;
   using System.Web.Mvc;
   using System.Xml;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class FeedController : Controller
   {
      private readonly IPostQueries postQueries;
      
      public FeedController(IPostQueries postQueries)
      {
         this.postQueries = postQueries;
      }

      public ActionResult Rss()
      {
         var posts = this.postQueries.GetAllPosts();

         var feed = this.ConvertToSyndicationFeed(posts);
         var formatter = new Rss20FeedFormatter(feed);
         this.GenerateFeed(formatter);

         this.Response.ContentType = "application/rss+xml";
         return new ContentResult();
      }

      private SyndicationFeed ConvertToSyndicationFeed(IEnumerable<Post> posts)
      {
         var uri = new Uri(this.Url.Action("List", "Blog", null, this.Request.Url.Scheme));
         var feed = new SyndicationFeed(
            SiteConfiguration.SiteName.Value,
            SiteConfiguration.Description.Value,
            uri,
            this.ConvertToItems(posts));

         feed.Links.Add(new SyndicationLink(feed.BaseUri));

         return feed;
      }

      private IEnumerable<SyndicationItem> ConvertToItems(IEnumerable<Post> posts)
      {
         foreach (var post in posts)
         {
            var url = this.Url.Action("GetBySlug", "Blog", new { slug = post.Slug }, this.Request.Url.Scheme);
            var uri = new Uri(url);
            var item = new SyndicationItem(post.Title, post.Summary, uri, post.Guid.ToString(), post.UpdatedDate);

            item.Authors.Add(new SyndicationPerson(string.Empty, post.Author, string.Empty));
            yield return item;
         }
      }
      
      private void GenerateFeed(SyndicationFeedFormatter formatter)
      {
         using (var writer = new XmlTextWriter(this.Response.Output))
         {
            formatter.WriteTo(writer);
         }
      }
   }
}