// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class NavigationQueries : INavigationQueries
   {
      private readonly List<NavigationItem> items;

      public NavigationQueries(ILoadStorage<NavigationItem> storage)
      {
         Contract.Requires(storage != null);

         this.items = storage.Load().ToList();
      }

      public IEnumerable<NavigationItem> GetNavigationItems()
      {
         return this.items;
      }
   }
}