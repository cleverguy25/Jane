// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JaneUserManager.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity
{
   using System;
   using System.Diagnostics.Contracts;

   using Jane.Identity.Models;

   using Microsoft.AspNet.Identity;
   using Microsoft.AspNet.Identity.Owin;
   using Microsoft.Owin;

   public class JaneUserManager : UserManager<User, Guid>
   {
      public JaneUserManager(IUserStore<User, Guid> store)
         : base(store)
      {
      }

      public static JaneUserManager Create(IdentityFactoryOptions<JaneUserManager> options, IOwinContext context)
      {
         Contract.Requires(options != null);

         var manager = new JaneUserManager(context.Get<UserStore>());
         manager.UserValidator = new UserValidator<User, Guid>(manager)
         {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
         };

         manager.PasswordValidator = new PasswordValidator
         {
            RequiredLength = 8,
            RequireNonLetterOrDigit = true,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
         };

         manager.UserLockoutEnabledByDefault = true;
         manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
         manager.MaxFailedAccessAttemptsBeforeLockout = 5;

         var dataProtectionProvider = options.DataProtectionProvider;
         if (dataProtectionProvider != null)
         {
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<User, Guid>(dataProtectionProvider.Create("Jane Identity"));
         }

         return manager;
      }
   }
}