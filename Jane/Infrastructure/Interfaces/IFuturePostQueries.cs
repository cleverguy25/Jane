// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFuturePostQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure.Interfaces
{
   using System.Collections.Generic;

   using Jane.Models;

   public interface IFuturePostQueries
   {
      IEnumerable<FuturePost> GetFuturePosts();
   }
}