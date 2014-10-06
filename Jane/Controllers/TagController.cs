// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System.Web.Mvc;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class TagController : Controller
   {
      private readonly ITagQueries tagQueries;

      public TagController(ITagQueries tagQueries)
      {
         this.tagQueries = tagQueries;
      }

      public ActionResult TagCloud()
      {
         var counts = this.tagQueries.GetTagsWithCounts();

         return this.PartialView(counts);
      }
   }
}