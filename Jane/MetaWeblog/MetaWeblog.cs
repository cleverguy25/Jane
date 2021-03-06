﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaWeblog.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.MetaWeblog
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.IO;
   using System.Linq;
   using System.Runtime.Remoting.Channels;
   using System.Security.Policy;
   using System.ServiceModel.Security;
   using System.Web;
   using System.Web.Hosting;

   using CookComputing.XmlRpc;

   using Jane.Identity;
   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;

   using Microsoft.Ajax.Utilities;
   using Microsoft.AspNet.Identity;
   using Microsoft.AspNet.Identity.Owin;
   using Microsoft.Owin.Security;

   public class MetaWeblog : XmlRpcService, IMetaWeblog
   {
      private readonly IStorage<Models.Post, Guid> storage;

      public MetaWeblog(IStorage<Models.Post, Guid> storage)
      {
         this.storage = storage;
      }

      public static string ContentPath { get; set; }

      public JaneSignInManager SignInManager
      {
         get
         {
            var context = new HttpContextWrapper(HttpContext.Current);
            return context.GetOwinContext().Get<JaneSignInManager>();
         }
      }
      
      string IMetaWeblog.AddPost(string blogid, string username, string password, Post newPost, bool publish)
      {
         if (string.IsNullOrEmpty(newPost.wp_slug))
         {
            throw new XmlRpcFaultException(0, "Slug cannot be empty.");
         }

         this.ValidateSignIn(username, password);

         var task = this.storage.LoadAsync();
         task.Wait();
         var posts = task.Result;
         if (posts.Any(p => p.Slug == newPost.wp_slug))
         {
            throw new XmlRpcFaultException(0, "That slug has already been used, please pick a new one.");
         }

         var post = new Models.Post
                       {
                          Guid = Guid.NewGuid(), 
                          Title = newPost.title, 
                          Summary = newPost.mt_excerpt, 
                          Author = newPost.userid, 
                          IsPublished = publish, 
                          PublishedDate = newPost.dateCreated.GetValueOrDefault(DateTime.Today), 
                          UpdatedDate = DateTime.Today, 
                          Slug = newPost.wp_slug, 
                          Tags = newPost.categories.ToList()
                       };
         post.Content = new LocalPostContent(post, ContentPath);
         post.Content.SetContent(newPost.description);

         var addTask = this.storage.AddAsync(post);
         addTask.Wait();
         return post.Guid.ToString();
      }

      bool IMetaWeblog.UpdatePost(string postid, string username, string password, Post updatedPost, bool publish)
      {
         if (string.IsNullOrEmpty(updatedPost.wp_slug))
         {
            throw new XmlRpcFaultException(0, "Slug cannot be empty.");
         }

         this.ValidateSignIn(username, password);

         var postGuid = VerifyGuid(postid);
         var task = this.storage.LoadAsync(postGuid);
         task.Wait();
         var post = task.Result;

         if (post == null)
         {
            return false;
         }

         post.Title = updatedPost.title;
         post.Tags = updatedPost.categories.ToList();
         post.Summary = updatedPost.mt_excerpt;
         post.Author = updatedPost.userid;
         post.IsPublished = publish;
         post.PublishedDate = updatedPost.dateCreated.GetValueOrDefault(DateTime.Today);
         post.UpdatedDate = DateTime.Today;
         post.Content.SetContent(updatedPost.description);
         var updateTask = this.storage.UpdateAsync(post);
         updateTask.Wait();

         return true;
      }

      bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
      {
         this.ValidateSignIn(username, password);

         var postGuid = VerifyGuid(key);
         var task = this.storage.DeleteAsync(postGuid);
         task.Wait();
         return true;
      }

      Post IMetaWeblog.GetPost(string postid, string username, string password)
      {
         this.ValidateSignIn(username, password);

         var postGuid = VerifyGuid(postid);
         var task = this.storage.LoadAsync(postGuid);
         task.Wait();
         var post = task.Result;

         if (post == null)
         {
            throw new XmlRpcFaultException(0, "No post found with id [" + postid + "].");
         }

         return MapPostToRpcObject(post);
      }

      Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
      {
         this.ValidateSignIn(username, password);

         var task = this.storage.LoadAsync();
         task.Wait();
         var posts = from post in task.Result
                     orderby post.PublishedDate descending
                     select MapPostToRpcObject(post);

         return posts.Take(numberOfPosts).ToArray();
      }

      object[] IMetaWeblog.GetCategories(string blogid, string username, string password)
      {
         this.ValidateSignIn(username, password);

         var list = new List<object>();
         var task = this.storage.LoadAsync();
         task.Wait();
         var categories = task.Result.SelectMany(p => p.Tags);

         foreach (var category in categories.Distinct())
         {
            list.Add(new { title = category });
         }

         return list.ToArray();
      }

      object IMetaWeblog.NewMediaObject(string blogid, string username, string password, MediaObject media)
      {
         this.ValidateSignIn(username, password);

         var extension = Path.GetExtension(media.name);
         if (extension == null)
         {
            throw new XmlRpcFaultException(0, "Cannot determine image extension.");
         }

         var relativePath = "~/content/posts/images/" + Guid.NewGuid() + "." + extension.Trim('.');
         var path = HostingEnvironment.MapPath(relativePath);
         if (path == null)
         {
            throw new XmlRpcFaultException(0, "Error saving file.");
         }

         File.WriteAllBytes(path, media.bits);
         var absolutePath = this.Context.Request.Url.Scheme + "://" + this.Context.Request.Url.Authority + relativePath.Trim('~');
         return new { url = absolutePath };
      }

      object[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
      {
         this.ValidateSignIn(username, password);

         return new object[]
                   {
                      new
                         {
                            blogid = "1", 
                            blogName = SiteConfiguration.SiteName.Value, 
                            url = this.Context.Request.Url.Scheme + "://" + this.Context.Request.Url.Authority
                         }
                   };
      }

      private static Guid VerifyGuid(string postid)
      {
         Guid postGuid;
         if (Guid.TryParse(postid, out postGuid) == false)
         {
            throw new XmlRpcFaultException(0, "Post ID should be a Guid.");
         }

         return postGuid;
      }

      private static Post MapPostToRpcObject(Models.Post post)
      {
         return new Post
                   {
                      description = post.Content.GetContent(), 
                      title = post.Title, 
                      dateCreated = post.PublishedDate, 
                      wp_slug = post.Slug, 
                      postid = post.Guid.ToString(), 
                      categories = post.Tags.ToArray()
                   };
      }

      private void ValidateSignIn(string userName, string password)
      {
         if (HttpContext.Current.Request.IsSecureConnection == false)
         {
            throw new SecurityAccessDeniedException("Requires HTTPS.");
         }

         var task = this.SignInManager.PasswordSignInAsync(userName, password, false, shouldLockout: false);
         task.Wait();
         if (task.Result != SignInStatus.Success)
         {
            throw new SecurityAccessDeniedException("Invalid UserName and/or Password");
         }
      }
   }
}