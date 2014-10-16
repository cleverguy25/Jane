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

   public class JsonStorage<TModel, TKey> : ILoadStorage<TModel, TKey> where TModel : class 
   {
      private readonly string path;

      private readonly Func<TKey, TModel, bool> findItem;

      public JsonStorage(string path, Func<TKey, TModel, bool> findItem)
      {
         this.path = path;
         this.findItem = findItem;
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
         if (this.findItem == null)
         {
            return (TModel)null;
         }

         var items = await this.LoadAsync();
         return items.FirstOrDefault(item => this.findItem(id, item));
      }
   }
}