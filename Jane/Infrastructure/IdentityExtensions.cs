// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityExtensions.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System;
   using System.Security.Principal;

   using Microsoft.AspNet.Identity;

   public static class IdentityExtensions
   {
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