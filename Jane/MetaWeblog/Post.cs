// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Post.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.MetaWeblog
{
   using System;

   using CookComputing.XmlRpc;

   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct Post
   {
      public string[] categories;

      public DateTime? dateCreated;

      public string description;

      public string mt_excerpt;

      public string permalink;

      public object postid;

      public string title;

      public string userid;

      public string wp_slug;
   }
}