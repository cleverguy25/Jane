// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Net;
   using System.Threading.Tasks;
   using System.Web;
   using System.Web.Mvc;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class BlogController : AsyncController
   {
      private readonly IPostQueries postQueries;

      private readonly IFuturePostQueries futureQueries;

      public BlogController(IPostQueries postQueries, IFuturePostQueries futureQueries)
      {
         this.postQueries = postQueries;
         this.futureQueries = futureQueries;
      }

      public async Task<ActionResult> List()
      {
         var posts = await this.postQueries.GetRecentPostsAsync();
         return this.View(posts);
      }

      public async Task<ActionResult> ArchiveList()
      {
         var archiveSummary = await this.postQueries.GetArchiveSummaryGroupedByYearAndMonth();
         return this.PartialView(archiveSummary);
      }

      public async Task<ActionResult> ArchiveByYearMonth(int year, int month)
      {
         this.ViewBag.Year = year;
         this.ViewBag.Month = month;
         this.ViewBag.MonthDisplay = new DateTime(year, month, 1).ToString("MMMM");
         var archiveSummary = await this.postQueries.GetPostsByYearAndMonth(year, month);
         return this.View("Archive", archiveSummary);
      }

      public async Task<ActionResult> ArchiveByYear(int year)
      {
         this.ViewBag.Year = year;
         var archiveSummary = await this.postQueries.GetPostsByYear(year);
         return this.View("Archive", archiveSummary);
      }

      public async Task<ActionResult> GetBySlug(string slug)
      {
         var post = await this.postQueries.GetPostBySlugAsync(slug);
         if (post == null)
         {
            throw new HttpException((int)HttpStatusCode.NotFound, "Slug not found.");
         }

         ViewBag.Title = post.Post.Title;
         return this.View(post);
      }

      public async Task<ActionResult> GetByTag(string tag)
      {
         var posts = await this.postQueries.GetPostsByTagAsync(tag);

         if (posts.Any())
         {
            this.ViewBag.Message = "No results found.";
         }

         ViewBag.Title = "Tagged with " + tag;
         this.ViewBag.Tag = tag;
         return this.View(posts);
      }

      public async Task<ActionResult> Recent()
      {
         var posts = await this.postQueries.GetRecentPostsAsync();

         posts = posts.Take(3);

         this.ViewBag.Header = "Recent";
         return this.PartialView("PostListShort", posts);
      }

      public async Task<ActionResult> Related(string slug)
      {
         var post = await this.postQueries.GetPostBySlugAsync(slug);

         var posts = post == null ? new List<Post>() : await this.postQueries.GetRelatedPostsAsync(post.Post);

         if (posts.Any() == false)
         {
            this.ViewBag.Message = "No related posts.";
         }

         this.ViewBag.Header = "Related";
         return this.PartialView("PostListShort", posts);
      }

      public async Task<ActionResult> Future()
      {
         var posts = await this.futureQueries.GetFuturePostsAsync();

         if (posts.Any())
         {
            this.ViewBag.Header = "Future";
         }

         return this.PartialView(posts);
      }
   }
}