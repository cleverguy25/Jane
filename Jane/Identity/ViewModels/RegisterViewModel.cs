﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterViewModel.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity.ViewModels
{
   using System;
   using System.ComponentModel.DataAnnotations;

   public class RegisterViewModel
   {
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

      [Display(Name = "Birthdate")]
      public DateTime BirthDate { get; set; }

      [Display(Name = "CreateDate")]
      public DateTime CreateDate { get; set; }
   }
}