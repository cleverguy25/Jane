// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoadStorage.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure.Interfaces
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Data.Objects;
   using System.Threading.Tasks;

   using Jane.Models;

   public interface ILoadStorage<T, in TKey>
   {
      Task<IEnumerable<T>> LoadAsync();

      Task<T> LoadAsync(TKey id);
   }
}