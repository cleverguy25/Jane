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

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Newtonsoft.Json;

   public class JsonStorage<TModel> : ILoadStorage<TModel> where TModel : class 
   {
      private readonly string path;

      private readonly Func<string, TModel, bool> findItem;

      public JsonStorage(string path, Func<string, TModel, bool> findItem)
      {
         this.path = path;
         this.findItem = findItem;
      }

      public IEnumerable<TModel> Load()
      {
         var json = new JsonSerializer();
         using (var stream = new StreamReader(this.path))
         {
            var items = (List<TModel>)json.Deserialize(stream, typeof(List<TModel>));

            return items;
         }
      }

      public TModel Load(string id)
      {
         if (this.findItem == null)
         {
            return (TModel)null;
         }

         return this.Load().FirstOrDefault(item => this.findItem(id, item));
      }
   }
}