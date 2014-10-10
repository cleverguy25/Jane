// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorage.cs" company="Jane OSS">
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

   public interface IStorage<T>
   {
      IEnumerable<T> Load();

      void Save(IEnumerable<T> data);
   }
}