// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostQueries.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure.Interfaces
{
   using System.Collections.Generic;
   using System.Threading.Tasks;

   using Jane.Models;

   public interface IPostQueries
   {
      Task<IEnumerable<Post>> GetAllPostsAsync();

      Task<IEnumerable<Post>> GetRecentPostsAsync();

      Task<IEnumerable<Post>> GetRelatedPostsAsync(Post post);

      Task<PostView> GetPostBySlugAsync(string slug);

      Task<IEnumerable<Post>> GetPostsByTagAsync(string tag);
   }
}