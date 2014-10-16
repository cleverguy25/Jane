// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopNavigationController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System.Threading.Tasks;
   using System.Web.Mvc;

   using Jane.Infrastructure.Interfaces;

   public class TopNavigationController : AsyncController
   {
      private readonly INavigationQueries navigationQueries;

      public TopNavigationController(INavigationQueries navigationQueries)
      {
         this.navigationQueries = navigationQueries;
      }

      public async Task<ActionResult> TopNavigation()
      {
         var items = await this.navigationQueries.GetNavigationItemsAsync();
         return this.PartialView(items);
      }
   }
}