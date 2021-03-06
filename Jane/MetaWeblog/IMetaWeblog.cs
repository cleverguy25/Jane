﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetaWeblog.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.MetaWeblog
{
   using CookComputing.XmlRpc;

   public interface IMetaWeblog
   {
      #region MetaWeblog API

      [XmlRpcMethod("metaWeblog.newPost")]
      string AddPost(string blogid, string username, string password, Post post, bool publish);

      [XmlRpcMethod("metaWeblog.editPost")]
      bool UpdatePost(string postid, string username, string password, Post post, bool publish);

      [XmlRpcMethod("metaWeblog.getPost")]
      Post GetPost(string postid, string username, string password);

      [XmlRpcMethod("metaWeblog.getCategories")]
      object[] GetCategories(string blogid, string username, string password);

      [XmlRpcMethod("metaWeblog.getRecentPosts")]
      Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);

      [XmlRpcMethod("metaWeblog.newMediaObject")]
      object NewMediaObject(string blogid, string username, string password, MediaObject mediaObject);

      #endregion

      #region Blogger API

      [XmlRpcMethod("blogger.deletePost")]
      [return: XmlRpcReturnValue(Description = "Returns true.")]
      bool DeletePost(string key, string postid, string username, string password, bool publish);

      [XmlRpcMethod("blogger.getUsersBlogs")]
      object[] GetUsersBlogs(string key, string username, string password);

      #endregion
   }
}