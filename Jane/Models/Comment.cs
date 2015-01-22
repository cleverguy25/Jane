// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Comment.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Models
{
   using System;
   using System.ComponentModel.DataAnnotations;
   using System.Web.Mvc;

   public class Comment
   {
      public Guid PostId { get; set; }

      public Guid? ReplyCommentId { get; set; }

      public Guid Id { get; set; }

      public Guid? CommenterId { get; set; }

      [AllowHtml]
      [Required]
      [DataType(DataType.MultilineText)]
      [Display(Name = "Comment")]
      public string Body { get; set; }

      [Required]
      public string Name { get; set; }

      [Required]
      [EmailAddress]
      public string Email { get; set; }

      public string EmailHash { get; set; }

      [Url]
      public string Url { get; set; }

      public bool IsSpam { get; set; }

      public DateTimeOffset CreatedAt { get; set; }

      public string UserHostAddress { get; set; }

      public string UserAgent { get; set; }
   }
}