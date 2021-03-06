﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeedController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.ServiceModel.Syndication;
   using System.Threading.Tasks;
   using System.Web.Mvc;
   using System.Xml;
   using System.Xml.Linq;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class FeedController : AsyncController
   {
      private readonly IPostQueries postQueries;
      
      public FeedController(IPostQueries postQueries)
      {
         this.postQueries = postQueries;
      }

      public ActionResult Rsd()
      {
         var ns = XNamespace.Get("http://archipelago.phrasewise.com/rsd");
         var uri = new Uri(this.Url.Action("List", "Blog", null, this.Request.Url.Scheme));
         var apiUri = new Uri(this.Request.Url.Scheme + "://" + this.Request.Url.Authority + "/metaweblogapi");
         return
            new XmlResult(
               new XDocument(
                  new XElement(
                     ns + "service",
                     new XElement(ns + "engineName", "Jane"),
                     new XElement(ns + "homePageLink", uri.ToString()),
                     new XElement(
                        ns + "apis",
                        new XElement(
                           ns + "api",
                           new XAttribute("name", "MetaWeblog"),
                           new XAttribute("preferred", "true"),
                           new XAttribute("blogID", "1"),
                           new XAttribute("apiLink", apiUri.ToString()))))),
               "{24A16C89-1865-4F40-802B-C717DEE14306}");
      }

      public async Task<ActionResult> Rss()
      {
         var posts = await this.postQueries.GetAllPostsAsync();

         this.GenerateFeed(posts, (feed) => new Rss20FeedFormatter(feed), "application/rss+xml");
         return new ContentResult();
      }

      public async Task<ActionResult> RssByTag(string tag)
      {
         var posts = await this.postQueries.GetPostsByTagAsync(tag);

         this.GenerateFeed(posts, (feed) => new Rss20FeedFormatter(feed), "application/rss+xml");
         return new ContentResult();
      }

      public async Task<ActionResult> Atom()
      {
         var posts = await this.postQueries.GetAllPostsAsync();

         this.GenerateFeed(posts, (feed) => new Atom10FeedFormatter(feed), "application/atom+xml");
         return new ContentResult();
      }

      public async Task<ActionResult> AtomByTag(string tag)
      {
         var posts = await this.postQueries.GetPostsByTagAsync(tag);

         this.GenerateFeed(posts, (feed) => new Atom10FeedFormatter(feed), "application/atom+xml");
         return new ContentResult();
      }

      private void GenerateFeed(IEnumerable<Post> posts, Func<SyndicationFeed, SyndicationFeedFormatter> getFormatter, string contentType)
      {
         var feed = this.ConvertToSyndicationFeed(posts);

         this.WriteFeedToResponse(getFormatter(feed));

         this.Response.ContentType = contentType;
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
      
      private void WriteFeedToResponse(SyndicationFeedFormatter formatter)
      {
         using (var writer = new XmlTextWriter(this.Response.Output))
         {
            formatter.WriteTo(writer);
         }
      }
   }
}