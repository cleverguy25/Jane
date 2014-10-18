// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Jane.Startup))]

namespace Jane
{
   using Owin;

   public partial class Startup
   {
      public void Configuration(IAppBuilder app)
      {
         this.ConfigureAuth(app);
      }
   }
}