// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Threading.Tasks;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class NavigationQueries : INavigationQueries
   {
      private readonly ILoadStorage<NavigationItem, string> storage;

      private List<NavigationItem> items;

      public NavigationQueries(ILoadStorage<NavigationItem, string> storage)
      {
         Contract.Requires(storage != null);
         this.storage = storage;
      }

      public async Task<IEnumerable<NavigationItem>> GetNavigationItemsAsync()
      {
         if (this.items == null)
         {
            var navItems = await this.storage.LoadAsync();
            this.items = navItems.ToList();
         }
         
         return this.items;
      }
   }
}