// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System;
   using System.Data.Metadata.Edm;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Web;
   using System.Web.Mvc;

   using Jane.Identity;
   using Jane.Identity.Models;
   using Jane.Identity.ViewModels;

   using Microsoft.Ajax.Utilities;
   using Microsoft.AspNet.Identity;
   using Microsoft.AspNet.Identity.Owin;
   using Microsoft.Owin.Security;

   [Authorize]
   public class AccountController : AsyncController
   {
      private const string ErrorHasOccurred = "An error has occurred.";

      private const string ExternalLoginWasAdded = "External login was added.";

      private const string ExternalLoginWasRemoved = "The external login was removed.";

      private const string PasswordChanged = "Your password has been changed.";

      private const string PasswordSet = "Your password has been set.";

      public JaneSignInManager SignInManager
      {
         get
         {
            return HttpContext.GetOwinContext().Get<JaneSignInManager>();
         }
      }

      public JaneUserManager UserManager
      {
         get
         {
            return HttpContext.GetOwinContext().Get<JaneUserManager>();
         }
      }

      private IAuthenticationManager AuthenticationManager
      {
         get
         {
            return HttpContext.GetOwinContext().Authentication;
         }
      }

      [AllowAnonymous]
      public PartialViewResult ExternalLogins(string returnUrl)
      {
         var authenticationTypes = HttpContext.GetOwinContext().Authentication.GetExternalAuthenticationTypes();
         var logins = new ExternalLoginListViewModel() { ReturnUrl = returnUrl, AuthenticationTypes = authenticationTypes };
         return this.PartialView(logins);
      }

      [AllowAnonymous]
      [RequireHttps]
      public ActionResult Login(string returnUrl)
      {
         ViewBag.ReturnUrl = returnUrl;
         return this.View();
      }
      
      [HttpPost]
      [AllowAnonymous]
      [RequireHttps]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
      {
         if (!ModelState.IsValid)
         {
            return this.View(model);
         }

         var result = await this.SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
         switch (result)
         {
            case SignInStatus.Success:
               return this.RedirectToLocal(returnUrl);
            case SignInStatus.LockedOut:
               return this.View("Lockout");
            ////case SignInStatus.RequiresVerification:
            ////   return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            default:
               this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
               return this.View(model);
         }
      }
      
      [AllowAnonymous]
      public ActionResult Register()
      {
         return this.View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Register(RegisterViewModel model)
      {
         if (!this.ModelState.IsValid)
         {
            return this.View(model);
         }

         var user = new User { Id = Guid.NewGuid(), Email = model.Email };
         var result = await this.UserManager.CreateAsync(user, model.Password);
         if (result.Succeeded)
         {
            await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return this.RedirectToDefaultPage();
         }

         this.AddErrors(result);

         return this.View(model);
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public ActionResult ExternalLogin(string provider, string returnUrl)
      {
         return new ChallengeResult(this.AuthenticationManager, provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
      }

      [AllowAnonymous]
      public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
      {
         var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
         if (loginInfo == null)
         {
            return this.RedirectToAction("Login");
         }

         // Sign in the user with this external login provider if the user already has a login
         var result = await this.SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
         switch (result)
         {
            case SignInStatus.Success:
               return this.RedirectToLocal(returnUrl);
            case SignInStatus.LockedOut:
               return this.View("Lockout");
            default:
               // If the user does not have an account, then prompt the user to create an account
               ViewBag.ReturnUrl = returnUrl;
               ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
               return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
         }
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
      {
         if (User.Identity.IsAuthenticated)
         {
            return this.RedirectToDefaultPage();
         }

         if (ModelState.IsValid)
         {
            // Get the information about the user from the external login provider
            var info = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
               return this.View("ExternalLoginFailure");
            }

            var user = new User { Id = Guid.NewGuid(), Email = model.Email };
            var result = await this.UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
               result = await this.UserManager.AddLoginAsync(user.Id, info.Login);
               if (result.Succeeded)
               {
                  await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                  return this.RedirectToLocal(returnUrl);
               }
            }

            this.AddErrors(result);
         }

         ViewBag.ReturnUrl = returnUrl;
         return this.View(model);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult LogOff()
      {
         this.AuthenticationManager.SignOut();
         return this.RedirectToDefaultPage();
      }

      [AllowAnonymous]
      public ActionResult ExternalLoginFailure()
      {
         return this.View();
      }

      public async Task<ActionResult> SetPassword()
      {
         var userId = this.ParseUserId();
         var model = new ChangePasswordViewModel() { HasPassword = await this.UserManager.HasPasswordAsync(userId) };
         ViewBag.Title = model.HasPassword ? "Change Password" : "Set Password";
         return this.View(model);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> SetPassword(ChangePasswordViewModel model)
      {
         if (!this.ValidatePasswordModel(model))
         {
            return this.View(model);
         }

         var userId = this.ParseUserId();
         var result = await this.ChangeOrAddPassword(model, userId);

         if (result.Succeeded)
         {
            var user = await this.UserManager.FindByIdAsync(userId);
            if (user != null)
            {
               await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            this.TempData["Message"] = model.HasPassword ? PasswordChanged : PasswordSet;
            return this.RedirectToManageRoute();
         }

         this.AddErrors(result);
         return this.View(model);
      }

      public async Task<ActionResult> Manage()
      {
         var userId = this.ParseUserId();
         var model = new ManageViewModel
         {
            HasPassword = await this.UserManager.HasPasswordAsync(userId),
            Logins = await this.UserManager.GetLoginsAsync(userId)
         };

         return this.View(model);
      }
      
      public async Task<ActionResult> ManageLogins()
      {
         var userId = this.ParseUserId();
         var user = await this.UserManager.FindByIdAsync(userId);
         if (user == null)
         {
            return this.Content(string.Empty);
         }

         var userLogins = await this.UserManager.GetLoginsAsync(userId);
         var otherLogins = this.AuthenticationManager.GetExternalAuthenticationTypes()
                     .Where(auth => userLogins.All(login => auth.AuthenticationType != login.LoginProvider)).ToList();
         var hasPassword = await this.UserManager.HasPasswordAsync(userId);
         ViewBag.ShowRemoveButton = hasPassword || userLogins.Count > 1;
         var model = new ManageLoginsViewModel
         {
            CurrentLogins = userLogins,
            OtherLogins = otherLogins
         };

         return this.PartialView(model);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult LinkLogin(string provider)
      {
         return new ChallengeResult(this.AuthenticationManager, provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
      }

      public async Task<ActionResult> LinkLoginCallback()
      {
         var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(ChallengeResult.XsrfKey, User.Identity.GetUserId());
         if (loginInfo == null)
         {
            this.TempData["Error"] = ErrorHasOccurred;
            return this.RedirectToManageRoute();
         }

         var userId = Guid.Parse(User.Identity.GetUserId());
         var result = await this.UserManager.AddLoginAsync(userId, loginInfo.Login);
         if (result.Succeeded)
         {
            this.TempData["Message"] = ExternalLoginWasAdded;
         }
         else
         {
            this.TempData["Error"] = this.CreateErrorMessage(result);
         }

         return this.RedirectToManageRoute();
      }
      
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
      {
         var userId = this.ParseUserId();
         var result = await this.UserManager.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey));
         if (result.Succeeded)
         {
            var user = await this.UserManager.FindByIdAsync(userId);
            if (user != null)
            {
               await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            this.TempData["Message"] = ExternalLoginWasRemoved;
         }
         else
         {
            this.TempData["Error"] = this.CreateErrorMessage(result);
         }

         return this.RedirectToManageRoute();
      }

      private bool ValidatePasswordModel(ChangePasswordViewModel model)
      {
         if (model.HasPassword == false)
         {
            this.ModelState.Remove("OldPassword");
         }

         return ModelState.IsValid;
      }

      private Guid ParseUserId()
      {
         return Guid.Parse(this.User.Identity.GetUserId());
      }

      private async Task<IdentityResult> ChangeOrAddPassword(ChangePasswordViewModel model, Guid userId)
      {
         IdentityResult result;
         if (model.HasPassword)
         {
            result = await this.UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
         }
         else
         {
            result = await this.UserManager.AddPasswordAsync(userId, model.NewPassword);
         }

         return result;
      }

      private void AddErrors(IdentityResult result)
      {
         foreach (var error in result.Errors)
         {
            this.ModelState.AddModelError(string.Empty, error);
         }
      }

      private string CreateErrorMessage(IdentityResult result)
      {
         var builder = new StringBuilder();
         foreach (var error in result.Errors)
         {
            builder.Append(error);
            builder.Append("<br/>");
         }

         return builder.ToString();
      }
      
      private RedirectToRouteResult RedirectToManageRoute()
      {
         return this.RedirectToAction("Manage", "Account");
      }

      private ActionResult RedirectToLocal(string returnUrl)
      {
         if (Url.IsLocalUrl(returnUrl))
         {
            return this.Redirect(returnUrl);
         }

         return this.RedirectToDefaultPage();
      }

      private RedirectToRouteResult RedirectToDefaultPage()
      {
         return this.RedirectToAction("List", "Blog");
      }

      internal class ChallengeResult : HttpUnauthorizedResult
      {
         internal const string XsrfKey = "XsrfUserId";

         private readonly IAuthenticationManager authenticationManager;

         public ChallengeResult(IAuthenticationManager authenticationManager, string provider, string redirectUri)
            : this(authenticationManager, provider, redirectUri, null)
         {
         }

         public ChallengeResult(IAuthenticationManager authenticationManager, string provider, string redirectUri, string userId)
         {
            this.authenticationManager = authenticationManager;
            this.LoginProvider = provider;
            this.RedirectUri = redirectUri;
            this.UserId = userId;
         }

         public string LoginProvider { get; set; }

         public string RedirectUri { get; set; }

         public string UserId { get; set; }

         public override void ExecuteResult(ControllerContext context)
         {
            var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
            if (this.UserId != null)
            {
               properties.Dictionary[XsrfKey] = this.UserId;
            }

            this.authenticationManager.Challenge(properties, this.LoginProvider);
         }
      }
   }
}