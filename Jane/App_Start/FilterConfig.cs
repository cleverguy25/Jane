// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane
{
   using System.Diagnostics.Contracts;
   using System.Web.Mvc;

   using Jane.Infrastructure;

   public class FilterConfig
   {
      public static void RegisterGlobalFilters(GlobalFilterCollection filters)
      {
         Contract.Requires(filters != null);

         filters.Add(new HandleAndLogErrorAttribute());
      }
   }
}