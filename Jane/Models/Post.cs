// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Post.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>

namespace Jane.Models
{
   using System;
   using System.Collections.Generic;

   public class Post
   {
      public Guid Guid { get; set; }

      public string Title { get; set; }

      public string Slug { get; set; }

      public string Author { get; set; }

      public string Link { get; set; }

      public DateTime PublishedDate { get; set; }

      public DateTime UpdatedDate { get; set; }

      public bool IsPublished { get; set; }

      public List<string> Tags { get; set; }

      public string Summary { get; set; }

      public IPostContent Content { get; set; }
   }
}