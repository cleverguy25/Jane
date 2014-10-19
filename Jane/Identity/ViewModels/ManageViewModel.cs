// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageViewModel.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity.ViewModels
{
   using System.Collections.Generic;

   using Microsoft.AspNet.Identity;

   public class ManageViewModel
   {
      public bool HasPassword { get; set; }

      public IList<UserLoginInfo> Logins { get; set; }
   }
}