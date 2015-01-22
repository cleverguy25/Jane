// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommentStorage.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure.Interfaces
{
   using System;
   using System.Collections.Generic;
   using System.Threading.Tasks;

   using Jane.Models;

   public interface ICommentStorage : IStorage<Comment, Guid>
   {
      Task<IEnumerable<Comment>> LoadByPostId(Guid postId);
   }
}