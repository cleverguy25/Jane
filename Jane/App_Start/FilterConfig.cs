// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the FilterConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System.Web.Mvc;

   using Jane.Infrastructure;

   public class FilterConfig
   {
      public static void RegisterGlobalFilters(GlobalFilterCollection filters)
      {
         filters.Add(new HandleAndLogErrorAttribute());
      }
   }
}