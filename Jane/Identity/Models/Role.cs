// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Role.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity.Models
{
   using System;
   using System.Collections.Generic;

   using Microsoft.AspNet.Identity;

   public class Role : IRole<Guid>
   {
      public Guid Id { get; set; }

      public string Name { get; set; }
   }
}