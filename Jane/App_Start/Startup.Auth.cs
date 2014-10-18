// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.Auth.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Threading.Tasks;
   using System.Web.Hosting;
   using System.Web.Mvc;

   using Jane.Identity;
   using Jane.Identity.Models;
   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.MetaWeblog;
   using Jane.Models;

   using Microsoft.AspNet.Identity;
   using Microsoft.AspNet.Identity.Owin;
   using Microsoft.Owin;
   using Microsoft.Owin.Security.Cookies;
   using Microsoft.Owin.Security.Google;

   using Newtonsoft.Json;

   using Owin;

   using SimpleInjector.Integration.Web.Mvc;

   public partial class Startup
   {
      public void ConfigureAuth(IAppBuilder app)
      {
         var container = JaneDependencies.SetUpContainer();
         var userStorage = container.GetInstance<IStorage<User, Guid>>();
         var roleStorage = container.GetInstance<ILoadStorage<Role, Guid>>();
         app.CreatePerOwinContext(() => new UserStore(userStorage, roleStorage));
         app.CreatePerOwinContext<JaneUserManager>(JaneUserManager.Create);
         app.CreatePerOwinContext<JaneSignInManager>(JaneSignInManager.Create);
         DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

         // Enable the application to use a cookie to store information for the signed in user
         app.UseCookieAuthentication(new CookieAuthenticationOptions
         {
            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            LoginPath = new PathString("/Account/Login"),
            Provider = new CookieAuthenticationProvider
            {
               // Enables the application to validate the security stamp when the user logs in.
               // This is a security feature which is used when you change a password or add an external login to your account.  
               OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<JaneUserManager, User, Guid>(
                   validateInterval: TimeSpan.FromMinutes(30),
                   regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie),
                   getUserIdCallback: (claimsIdentity) =>
                      {
                         string id = claimsIdentity.GetUserId();
                         Guid guid;
                         if (Guid.TryParse(id, out guid))
                         {
                            throw new InvalidCastException("Id [" + id + "needs to be of type guid.");
                         }

                         return guid;
                      })
            }
         });

         app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

         var providers = this.GetSocialProviders();

         AddMicrosoftAccountSocialLogin(app, providers);
         AddTwitterSocialLogin(app, providers);
         AddFacebookSocialLogin(app, providers);
         AddGoogleSocialLogin(app, providers);
      }

      private static void AddGoogleSocialLogin(IAppBuilder app, Dictionary<string, SocialLoginProvider> providers)
      {
         SocialLoginProvider google;
         if (providers.TryGetValue("google", out google))
         {
            app.UseGoogleAuthentication(
               new GoogleOAuth2AuthenticationOptions() { ClientId = google.ClientId, ClientSecret = google.Secret });
         }
      }

      private static void AddFacebookSocialLogin(IAppBuilder app, Dictionary<string, SocialLoginProvider> providers)
      {
         SocialLoginProvider facebook;
         if (providers.TryGetValue("facebook", out facebook))
         {
            app.UseFacebookAuthentication(appId: facebook.ClientId, appSecret: facebook.Secret);
         }
      }

      private static void AddTwitterSocialLogin(IAppBuilder app, Dictionary<string, SocialLoginProvider> providers)
      {
         SocialLoginProvider twitter;
         if (providers.TryGetValue("twitter", out twitter))
         {
            app.UseTwitterAuthentication(consumerKey: twitter.ClientId, consumerSecret: twitter.Secret);
         }
      }

      private static void AddMicrosoftAccountSocialLogin(IAppBuilder app, Dictionary<string, SocialLoginProvider> providers)
      {
         SocialLoginProvider microsoft;
         if (providers.TryGetValue("microsoft", out microsoft))
         {
            app.UseMicrosoftAccountAuthentication(clientId: microsoft.ClientId, clientSecret: microsoft.Secret);
         }
      }

      private Dictionary<string, SocialLoginProvider> GetSocialProviders()
      {
         try
         {
            var path = HostingEnvironment.MapPath(@"~\App_Data\socialproviders.json");
            if (path == null)
            {
               return new Dictionary<string, SocialLoginProvider>();
            }

            var json = new JsonSerializer();
            using (var stream = new StreamReader(path))
            {
               return (Dictionary<string, SocialLoginProvider>)
                  json.Deserialize(stream, typeof(Dictionary<string, SocialLoginProvider>));
            }
         }
         catch (Exception error)
         {
            Tracing.Log.LoadSocialProvidersError(error.Message, error.ToString());
            return new Dictionary<string, SocialLoginProvider>();
         }
      }
   }
}