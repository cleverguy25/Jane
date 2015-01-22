// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonStorage.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.IO;
   using System.Linq;
   using System.Threading.Tasks;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Newtonsoft.Json;

   public class JsonStorage<TModel, TKey> : IStorage<TModel, TKey> where TModel : class 
   {
      private readonly string path;

      private readonly Func<TModel, TKey> getKey;

      public JsonStorage(string path, Func<TModel, TKey> getKey)
      {
         Contract.Requires(getKey != null);

         this.path = path;
         this.getKey = getKey;
      }

      public Task<IEnumerable<TModel>> LoadAsync()
      {
         var json = new JsonSerializer();
         using (var stream = new StreamReader(this.path))
         {
            var items = (List<TModel>)json.Deserialize(stream, typeof(List<TModel>));

            return Task.FromResult(items.AsEnumerable());
         }
      }

      public async Task<TModel> LoadAsync(TKey id)
      {
         if (this.getKey == null)
         {
            return (TModel)null;
         }

         var items = await this.LoadAsync();
         return items.FirstOrDefault(item => this.getKey(item).Equals(id));
      }

      public async Task<TKey> AddAsync(TModel item)
      {
         await this.AddOrUpdate(item);
         return this.getKey(item);
      }

      public async Task UpdateAsync(TModel item)
      {
         await this.AddOrUpdate(item);
      }

      public Task DeleteAsync(TModel item)
      {
         return this.DeleteAsync(this.getKey(item));
      }

      public async Task DeleteAsync(TKey id)
      {
         var result = await this.LoadAsync();
         var items = result.ToList();
         items.RemoveAll(i => this.getKey(i).Equals(id));
         this.Save(items);
      }

      private async Task AddOrUpdate(TModel item)
      {
         var items = await this.GetItems();
         var id = this.getKey(item);
         var index = items.FindIndex(i => this.getKey(i).Equals(id));
         if (index >= 0)
         {
            items[index] = item;
         }
         else
         {
            items.Add(item);
         }

         this.Save(items);
      }

      private void Save(IEnumerable<TModel> items)
      {
         var json = new JsonSerializer();
         using (var stream = new StreamWriter(this.path))
         {
            json.Serialize(stream, items);
         }
      }

      private async Task<List<TModel>> GetItems()
      {
         var result = await this.LoadAsync();
         var items = result.ToList();
         return items;
      }
   }
}