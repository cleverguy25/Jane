// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INavigationQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure.Interfaces
{
   using System.Collections.Generic;
   using System.Threading.Tasks;

   using Jane.Models;

   public interface INavigationQueries
   {
      Task<IEnumerable<NavigationItem>> GetNavigationItemsAsync();
   }
}