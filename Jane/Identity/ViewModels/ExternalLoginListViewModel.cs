// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalLoginListViewModel.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity.ViewModels
{
   using System.Collections.Generic;

   using Microsoft.Owin.Security;

   public class ExternalLoginListViewModel
   {
      public IEnumerable<AuthenticationDescription> AuthenticationTypes { get; set; }

      public string ReturnUrl { get; set; }
   }
}