// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopNavigationController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System.Web.Mvc;

   using Jane.Infrastructure.Interfaces;

   public class TopNavigationController : Controller
   {
      private readonly INavigationQueries navigationQueries;

      public TopNavigationController(INavigationQueries navigationQueries)
      {
         this.navigationQueries = navigationQueries;
      }

      public ActionResult TopNavigation()
      {
         var items = this.navigationQueries.GetNavigationItems();
         return this.PartialView(items);
      }
   }
}