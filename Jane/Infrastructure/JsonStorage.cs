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

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Newtonsoft.Json;

   public class JsonStorage<TModel> : IStorage<TModel>
   {
      private readonly string path;

      public JsonStorage(string path)
      {
         this.path = path;
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

      public void Save(IEnumerable<TModel> data)
      {
         throw new NotImplementedException();
      }
   }
}