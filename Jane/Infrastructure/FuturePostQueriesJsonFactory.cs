// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuturePostQueriesJsonFactory.cs" company="Jane OSS">
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

   public static class FuturePostQueriesJsonFactory
   {
      public static IFuturePostQueries Create(string path)
      {
         Contract.Requires<ArgumentNullException>(string.IsNullOrEmpty(path) == false, "path");

         var json = new JsonSerializer();
         using (var stream = new StreamReader(path))
         {
            var items = (List<FuturePost>)json.Deserialize(stream, typeof(List<FuturePost>));

            return new FuturePostQueries(items);
         }
      }
   }
}