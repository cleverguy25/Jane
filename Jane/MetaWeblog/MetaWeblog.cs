// --------------------------------------------------------------------------------------------------------------------
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
   using System.Web;
   using System.Web.Hosting;

   using CookComputing.XmlRpc;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;

   public class MetaWeblog : XmlRpcService, IMetaWeblog
   {
      private readonly ILoadStorage<Models.Post> loadStorage;

      private readonly ISaveStorage<Models.Post> saveStorage;

      public MetaWeblog(ILoadStorage<Models.Post> loadStorage, ISaveStorage<Models.Post> saveStorage)
      {
         this.loadStorage = loadStorage;
         this.saveStorage = saveStorage;
      }

      public static string ContentPath { get; set; }

      string IMetaWeblog.AddPost(string blogid, string username, string password, Post newPost, bool publish)
      {
         if (string.IsNullOrEmpty(newPost.wp_slug))
         {
            throw new XmlRpcFaultException(0, "Slug cannot be empty.");
         }

         if (this.loadStorage.Load().Any(p => p.Slug == newPost.wp_slug))
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

         this.saveStorage.Add(post);
         return post.Guid.ToString();
      }

      bool IMetaWeblog.UpdatePost(string postid, string username, string password, Post updatedPost, bool publish)
      {
         if (string.IsNullOrEmpty(updatedPost.wp_slug))
         {
            throw new XmlRpcFaultException(0, "Slug cannot be empty.");
         }

         VerifyGuid(postid);
         var post = this.loadStorage.Load(postid);

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
         this.saveStorage.Update(post);

         return true;
      }

      bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
      {
         VerifyGuid(key);
         this.saveStorage.Delete(key);
         return true;
      }

      Post IMetaWeblog.GetPost(string postid, string username, string password)
      {
         VerifyGuid(postid);
         var post = this.loadStorage.Load(postid);

         if (post == null)
         {
            throw new XmlRpcFaultException(0, "No post found with id [" + postid + "].");
         }

         return MapPostToRpcObject(post);
      }

      Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
      {
         var posts = from post in this.loadStorage.Load()
                     orderby post.PublishedDate descending
                     select MapPostToRpcObject(post);

         return posts.Take(numberOfPosts).ToArray();
      }

      object[] IMetaWeblog.GetCategories(string blogid, string username, string password)
      {
         var list = new List<object>();
         var categories = this.loadStorage.Load().SelectMany(p => p.Tags);

         foreach (var category in categories.Distinct())
         {
            list.Add(new { title = category });
         }

         return list.ToArray();
      }

      object IMetaWeblog.NewMediaObject(string blogid, string username, string password, MediaObject media)
      {
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

      private static void VerifyGuid(string postid)
      {
         Guid postGuid;
         if (Guid.TryParse(postid, out postGuid) == false)
         {
            throw new XmlRpcFaultException(0, "Post ID should be a Guid.");
         }
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
   }
}