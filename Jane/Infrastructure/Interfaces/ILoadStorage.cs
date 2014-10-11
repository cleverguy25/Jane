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

   using Jane.Models;

   public interface ILoadStorage<out T>
   {
      IEnumerable<T> Load();

      T Load(string id);
   }
}