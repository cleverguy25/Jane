// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostCommentsController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Dynamic;
   using System.IO;
   using System.Linq;
   using System.Net;
   using System.Text;
   using System.Threading;
   using System.Threading.Tasks;
   using System.Web.Mvc;
   using System.Web.Routing;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Newtonsoft.Json;

   public class PostCommentsController : AsyncController
   {
      private readonly ILoadStorage<Post, Guid> postStorage;

      private readonly ICommentStorage commentStorage;

      public PostCommentsController(ILoadStorage<Post, Guid> postStorage, ICommentStorage commentStorage)
      {
         this.postStorage = postStorage;
         this.commentStorage = commentStorage;
      }

      public async Task<ActionResult> Comments(Guid postId)
      {
         ViewBag.PostId = postId;
         var items = await this.commentStorage.LoadByPostId(postId);
         var comments = items.ToList();
         var commentsMap = comments.ToDictionary(
            comment => comment.Id,
            comment => new ThreadedComment() { Comment = comment });

         var groups = comments.OrderBy(comment => comment.CreatedAt).Select(comment => commentsMap[comment.Id]).GroupBy(comment => comment.Comment.ReplyCommentId);

         var rootComments = new List<ThreadedComment>();
         foreach (var group in groups)
         {
            if (group.Key == null)
            {
               rootComments = group.ToList();
               continue;
            }

            commentsMap[group.Key.Value].ChildComments = group.ToList();
         }

         return this.View(rootComments);
      }

      public async Task<ActionResult> AddComment(Guid postId, Guid? replyCommentId)
      {
         var model = new Comment { PostId = postId, ReplyCommentId = replyCommentId };
         await this.FillInLoggedInUserDetails(model);

         return this.View(model);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> AddComment(Comment comment, string captcha)
      {
         if (await VerifyRecaptcha(captcha) == false)
         {
            this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return this.Json(new { Errors = new string[1] { "Recaptcha not valid." } });
         }

         if (ModelState.IsValid == false)
         {
            var errors = ModelState.Values.SelectMany(modelState => modelState.Errors).Select(error => error.ErrorMessage);
            this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return this.Json(new { Errors = errors });
         }

         comment.Id = Guid.NewGuid();
         comment.CreatedAt = DateTime.Now;
         comment.EmailHash = comment.Email.CalculateMd5Hash();
         comment.UserAgent = Request.UserAgent;
         comment.UserHostAddress = Request.UserHostAddress;
         await this.commentStorage.AddAsync(comment);
         return this.Json(new { Message = "Thanks for your comment.  It will be posted soon." });
      }

      private static async Task<bool> VerifyRecaptcha(string captcha)
      {
         var builder = new StringBuilder("https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={response}");
         builder.Replace("{secret}", SiteConfiguration.RecaptchaPrivateKey.Value);
         builder.Replace("{response}", captcha);
         var request = WebRequest.Create(builder.ToString());
         var response = await request.GetResponseAsync();
         if (response == null)
         {
            return false;
         }

         var stream = response.GetResponseStream();
         if (stream == null)
         {
            return false;
         }

         using (var streamReader = new StreamReader(stream))
         using (var jsonTextReader = new JsonTextReader(streamReader))
         {
            var serializer = new JsonSerializer();
            var result = (IDictionary<string, object>)serializer.Deserialize<ExpandoObject>(jsonTextReader);
            return result.ContainsKey("success") && (bool)result["success"];
         }
      }

      private async Task FillInLoggedInUserDetails(Comment comment)
      {
         if (this.User.Identity.IsAuthenticated == false)
         {
            return;
         }

         this.ViewBag.BannedCommenter = this.User.IsInRole("banned_commenter");
         var user = await HttpContext.GetCurrentUser();
         comment.Email = user.Email;
         comment.CommenterId = user.Id;
      }
   }
}