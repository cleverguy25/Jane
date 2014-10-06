// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogController.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the BlogController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Controllers
{
   using System.Collections.Generic;
   using System.Linq;
   using System.Net;
   using System.Web;
   using System.Web.Mvc;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class BlogController : Controller
   {
      private readonly IPostQueries postQueries;

      public BlogController(IPostQueries postQueries)
      {
         this.postQueries = postQueries;
      }

      public ActionResult List()
      {
         var posts = this.postQueries.GetRecentPosts();
         return this.View(posts);
      }

      public ActionResult GetBySlug(string slug)
      {
         var post = this.postQueries.GetPostBySlug(slug);
         if (post == null)
         {
            throw new HttpException((int)HttpStatusCode.NotFound, "Slug not found.");
         }

         ViewBag.Title = post.Title;
         return this.View(post);
      }

      public ActionResult GetByTag(string tag)
      {
         var posts = this.postQueries.GetPostsByTag(tag).ToList();

         if (posts.Count == 0)
         {
            this.ViewBag.Message = "No results found.";
         }

         ViewBag.Title = "Tagged with " + tag;
         this.ViewBag.Tag = tag;
         return this.View(posts);
      }

      public ActionResult Recent()
      {
         var posts = this.postQueries.GetRecentPosts().Take(3);

         this.ViewBag.Header = "Recent";
         return this.PartialView("PostListShort", posts);
      }

      public ActionResult Related(string slug)
      {
         var post = this.postQueries.GetPostBySlug(slug);

         var posts = post == null ? new List<Post>() : this.postQueries.GetRelatedPosts(post).ToList();

         if (posts.Any() == false)
         {
            this.ViewBag.Message = "No related posts.";
         }

         this.ViewBag.Header = "Related";
         return this.PartialView("PostListShort", posts);
      }
   }
}