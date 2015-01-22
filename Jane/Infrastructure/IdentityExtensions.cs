// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityExtensions.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System;
   using System.Security.Principal;
   using System.Threading.Tasks;
   using System.Web;

   using Jane.Identity;
   using Jane.Identity.Models;

   using Microsoft.Ajax.Utilities;
   using Microsoft.AspNet.Identity;
   using Microsoft.AspNet.Identity.Owin;

   public static class IdentityExtensions
   {
      public static async Task<User> GetCurrentUser(this HttpContextBase context)
      {
         var identity = context.User.Identity;
         if (identity.IsAuthenticated)
         {
            return null;
         }

         var id = context.User.Identity.GetUserGuid();
         var userManager = context.GetOwinContext().Get<JaneUserManager>();
         var user = await userManager.FindByIdAsync(id);
         return user;
      }

      public static Guid GetUserGuid(this IIdentity identity)
      {
         var id = identity.GetUserId();
         Guid userId;
         if (Guid.TryParse(id, out userId))
         {
            return userId;
         }

         id = id ?? "NULL";
         throw new ArgumentException("User ID [" + id + "] was not a guid.");
      }

      public static Guid? TryGetUserGuid(this IIdentity identity)
      {
         var id = identity.GetUserId();
         Guid userId;
         if (Guid.TryParse(id, out userId))
         {
            return userId;
         }

         return null;
      }
   }
}