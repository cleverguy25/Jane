// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadedComment.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Models
{
   using System.Collections.Generic;

   public class ThreadedComment
   {
      public Comment Comment { get; set; }

      public string GravatarUrl
      {
         get
         {
            return "http://s.gravatar.com/avatar/" + this.Comment.EmailHash.ToLowerInvariant() + "?s=50";
         }
      }

      public List<ThreadedComment> ChildComments { get; set; }
   }
}