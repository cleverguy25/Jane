﻿// --------------------------------------------------------------------------------------------------------------------
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

      public NavigationQueries(IEnumerable<NavigationItem> items)
      {
         Contract.Requires(items != null);
         this.items = items.ToList();
      }

      public IEnumerable<NavigationItem> GetNavigationItems()
      {
         return this.items;
      }
   }
}