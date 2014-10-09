// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuturePost.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Models
{
   using System;

   public class FuturePost
   {
      public string Title { get; set; }

      public DateTime PublishDate { get; set; }

      public string GetExpectedWait(DateTime date)
      {
         var days = (this.PublishDate - date).Days;

         return "about " + days + " day" + (days == 1 ? string.Empty : "s");
      }
   }
}