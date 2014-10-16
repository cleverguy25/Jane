// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System.Threading.Tasks;
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

      public async Task<ActionResult> TagCloud()
      {
         var counts = await this.tagQueries.GetTagsWithCountsAsync();

         return this.PartialView(counts);
      }
   }
}