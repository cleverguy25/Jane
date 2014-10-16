// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITagQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure.Interfaces
{
   using System;
   using System.Collections.Generic;
   using System.Threading.Tasks;

   public interface ITagQueries
   {
      Task<IEnumerable<Tuple<string, int>>> GetTagsWithCountsAsync();
   }
}