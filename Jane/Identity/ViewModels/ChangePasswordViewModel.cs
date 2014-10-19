﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangePasswordViewModel.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity.ViewModels
{
   using System.ComponentModel.DataAnnotations;

   public class ChangePasswordViewModel
   {
      public bool HasPassword { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Current password")]
      public string OldPassword { get; set; }
      
      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
      [DataType(DataType.Password)]
      [Display(Name = "New password")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm new password")]
      [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
   }
}