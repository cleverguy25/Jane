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
   using System.Web.Mvc;

   using Jane.Infrastructure.Interfaces;

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
   }
}