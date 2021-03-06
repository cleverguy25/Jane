﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostContent.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Models
{
   public interface IPostContent
   {
      string GetContent();

      void SetContent(string content);

      void Save(string fileName);
   }
}