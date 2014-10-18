// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JaneSignInManager.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity
{
   using System;
   using System.Diagnostics.Contracts;
   using System.Security.Claims;
   using System.Threading.Tasks;
   using System.Web;

   using Jane.Identity.Models;

   using Microsoft.AspNet.Identity.Owin;
   using Microsoft.Owin;
   using Microsoft.Owin.Security;

   public class JaneSignInManager : SignInManager<User, Guid>
   {
      public JaneSignInManager(JaneUserManager userManager, IAuthenticationManager authenticationManager)
         : base(userManager, authenticationManager)
      {
      }
      
      public static JaneSignInManager Create(IdentityFactoryOptions<JaneSignInManager> options, IOwinContext context)
      {
         Contract.Requires(options != null);
         Contract.Requires(context != null);

         return new JaneSignInManager(context.GetUserManager<JaneUserManager>(), context.Authentication);
      }

      public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
      {
         return user.GenerateUserIdentityAsync(this.UserManager, this.AuthenticationType);
      }
   }
}