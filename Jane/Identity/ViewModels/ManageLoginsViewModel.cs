// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageLoginsViewModel.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity.ViewModels
{
   using System.Collections.Generic;

   using Microsoft.AspNet.Identity;
   using Microsoft.Owin.Security;

   public class ManageLoginsViewModel
   {
      public IList<UserLoginInfo> CurrentLogins { get; set; }

      public IList<AuthenticationDescription> OtherLogins { get; set; }
   }
}