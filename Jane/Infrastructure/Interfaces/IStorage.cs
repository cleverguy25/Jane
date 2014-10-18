// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorage.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure.Interfaces
{
   public interface IStorage<T, TKey> : ILoadStorage<T, TKey>, ISaveStorage<T, TKey>
   {
   }
}