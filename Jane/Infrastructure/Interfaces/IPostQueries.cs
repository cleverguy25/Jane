﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the IPostQueries type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure.Interfaces
{
   using System.Collections.Generic;

   using Jane.Models;

   public interface IPostQueries
   {
      IEnumerable<Post> GetAllPosts();

      IEnumerable<Post> GetRecentPosts();

      Post GetPostBySlug(string slug);

      IEnumerable<Post> GetPostsByTag(string tag);
   }
}