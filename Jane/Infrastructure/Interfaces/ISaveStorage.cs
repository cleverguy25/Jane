// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISaveStorage.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure.Interfaces
{
   using System.Threading.Tasks;

   public interface ISaveStorage<T, TKey>
   {
      Task<TKey> AddAsync(T item);

      Task UpdateAsync(T item);

      Task DeleteAsync(TKey id);
   }
}