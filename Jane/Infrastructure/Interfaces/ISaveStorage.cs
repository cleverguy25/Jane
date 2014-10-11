// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISaveStorage.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure.Interfaces
{
   public interface ISaveStorage<in T>
   {
      void Add(T item);

      void Update(T item);

      void Delete(string id);
   }
}