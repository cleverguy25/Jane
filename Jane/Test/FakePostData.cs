// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakePostData.cs" company="Jane">
//   Copyright (c) Jane Contributors
// </copyright>
// <summary>
//   Defines the FakePostData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System;

   using Jane.Models;

   public class FakePostData
   {
      public static readonly Post[] Posts =
         {
            new Post
               {
                  Slug = "post1", 
                  Title = "Post 1", 
                  PublishedDate = new DateTime(2014, 5, 12),
                  UpdatedDate = new DateTime(2014, 5, 12, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 1"
               }, 
            new Post
               {
                  Slug = "post2", 
                  Title = "Post 2", 
                  PublishedDate = new DateTime(2014, 5, 15),
                  UpdatedDate = new DateTime(2014, 5, 15, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 2"
               }, 
            new Post
               {
                  Slug = "post3", 
                  Title = "Post 3", 
                  PublishedDate = new DateTime(2014, 5, 16),
                  UpdatedDate = new DateTime(2014, 5, 16, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 3"
               }, 
            new Post
               {
                  Slug = "post4", 
                  Title = "Post 4", 
                  PublishedDate = new DateTime(2014, 5, 3),
                  UpdatedDate = new DateTime(2014, 5, 3, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 4"
               }, 
            new Post
               {
                  Slug = "post5", 
                  Title = "Post 5", 
                  PublishedDate = new DateTime(2014, 5, 5),
                  UpdatedDate = new DateTime(2014, 5, 5, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 5"
               }, 
            new Post
               {
                  Slug = "post6", 
                  Title = "Post 6", 
                  PublishedDate = new DateTime(2014, 5, 21),
                  UpdatedDate = new DateTime(2014, 5, 21, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 6"
               }
         };
   }
}