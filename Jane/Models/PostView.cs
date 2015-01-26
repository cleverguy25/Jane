// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostView.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Models
{
   public class PostView
   {
      public Post Post { get; set; }

      public Post PreviousPost { get; set; }

      public Post NextPost { get; set; }
   }
}