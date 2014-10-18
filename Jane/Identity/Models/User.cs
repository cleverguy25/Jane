// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity.Models
{
   using System;
   using System.Collections.Generic;
   using System.Security.Claims;
   using System.Threading.Tasks;

   using Microsoft.AspNet.Identity;

   public class User : IUser<Guid>
   {
      private string email;

      private string userName;

      public User()
      {
         this.Claims = new HashSet<Claim>();
         this.Logins = new HashSet<UserLoginInfo>();
         this.Roles = new HashSet<Role>();
      }

      public Guid Id { get; set; }

      public string UserName
      {
         get
         {
            return this.userName;
         }
         
         set
         {
            this.userName = value;
            this.email = value;
         }
      }

      public string Email
      {
         get
         {
            return this.email;
         }

         set
         {
            this.email = value;
            this.userName = value;
         }
      }

      public bool EmailConfirmed { get; set; }

      public string PasswordHash { get; set; }

      public string SecurityStamp { get; set; }

      public bool TwoFactorEnabled { get; set; }

      public DateTime? LockoutEndDateUtc { get; set; }

      public bool LockoutEnabled { get; set; }

      public int AccessFailedCount { get; set; }

      public ICollection<Claim> Claims { get; set; }

      public ICollection<UserLoginInfo> Logins { get; set; }

      public ICollection<Role> Roles { get; set; }

      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
         UserManager<User, Guid> manager, 
         string authenticationType)
      {
         var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
         return userIdentity;
      }
   }
}