// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationQueriesJsonFactory.cs" company="Jane OSS">
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

   public static class NavigationQueriesJsonFactory
   {
      public static INavigationQueries Create(string path)
      {
         Contract.Requires<ArgumentNullException>(string.IsNullOrEmpty(path) == false, "path");

         var json = new JsonSerializer();
         using (var stream = new StreamReader(path))
         {
            var items = (List<NavigationItem>)json.Deserialize(stream, typeof(List<NavigationItem>));

            return new NavigationQueries(items);
         }
      }
   }
}