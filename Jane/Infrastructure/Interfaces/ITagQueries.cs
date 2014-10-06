// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITagQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure.Interfaces
{
   using System;
   using System.Collections.Generic;

   public interface ITagQueries
   {
      IEnumerable<Tuple<string, int>> GetTagsWithCounts();
   }
}