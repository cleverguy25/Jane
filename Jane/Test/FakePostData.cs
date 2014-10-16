// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakePostData.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;

   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Moq;

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
                  Summary = "Summary of Post 1",
                  Tags = new List<string>() { "foo" }
               }, 
            new Post
               {
                  Slug = "post2", 
                  Title = "Post 2", 
                  PublishedDate = new DateTime(2014, 5, 15),
                  UpdatedDate = new DateTime(2014, 5, 15, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 2",
                  Tags = new List<string>() { "bar", "foo" }
               }, 
            new Post
               {
                  Slug = "post3", 
                  Title = "Post 3", 
                  PublishedDate = new DateTime(2014, 5, 16),
                  UpdatedDate = new DateTime(2014, 5, 16, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 3",
                  Tags = new List<string>() { "bar" }
               }, 
            new Post
               {
                  Slug = "post4", 
                  Title = "Post 4", 
                  PublishedDate = new DateTime(2014, 5, 3),
                  UpdatedDate = new DateTime(2014, 5, 3, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 4",
                  Tags = new List<string>() { "fubar" }
               }, 
            new Post
               {
                  Slug = "post5", 
                  Title = "Post 5", 
                  PublishedDate = new DateTime(2014, 5, 5),
                  UpdatedDate = new DateTime(2014, 5, 5, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 5",
                  Tags = new List<string>() { "bar" }
               }, 
            new Post
               {
                  Slug = "post6", 
                  Title = "Post 6", 
                  PublishedDate = new DateTime(2014, 5, 21),
                  UpdatedDate = new DateTime(2014, 5, 21, 1, 2, 3, DateTimeKind.Utc),
                  Author = "author",
                  Guid = Guid.NewGuid(),
                  Summary = "Summary of Post 6",
                  Tags = new List<string>() { }
               }
         };

      public static ILoadStorage<Post, Guid> GetStorage()
      {
         var mock = new Mock<ILoadStorage<Post, Guid>>();
         mock.Setup(storage => storage.LoadAsync()).Returns(Task.FromResult(Posts.AsEnumerable()));
         return mock.Object;
      }
   }
}