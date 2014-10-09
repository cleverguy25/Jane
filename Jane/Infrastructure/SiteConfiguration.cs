// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteConfiguration.cs" company="Jane">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System;
   using System.Configuration;

   public static class SiteConfiguration
   {
      public static readonly Lazy<string> SiteName =
         new Lazy<string>(() => LoadConfigurationString("jane:SiteName", true));

      public static readonly Lazy<string> Description =
         new Lazy<string>(() => LoadConfigurationString("jane:Description", true));

      public static readonly Lazy<string> Image = new Lazy<string>(() => LoadConfigurationString("jane:Image", true));

      public static readonly Lazy<string> TileColor = new Lazy<string>(() => LoadConfigurationString("jane:TileColor"));

      private static string LoadConfigurationString(string key, bool required = false)
      {
         var value = ConfigurationManager.AppSettings.Get(key);

         if (required && string.IsNullOrEmpty(value))
         {
            throw new ConfigurationErrorsException("App setting key '" + key + "' is required but is empty.");
         }

         return value;
      }
   }
}