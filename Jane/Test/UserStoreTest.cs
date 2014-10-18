// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserStoreTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System.Threading.Tasks;

   using FluentAssertions;

   using Jane.Identity;

   using Microsoft.AspNet.Identity;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class UserStoreTest
   {
      private UserStore store;

      [TestInitialize]
      public void Initialize()
      {
         this.store = new UserStore(new FakeUserData(), FakeUserData.GetRoleStorage());
      }

      [TestMethod]
      [TestCategory("UnitTest")]
      public async Task UserStoreFindByNameAsync()
      {
         var item = await this.store.FindByNameAsync("user1@foo.com");
         item.Email.Should().Be("user1@foo.com");
      }

      [TestMethod]
      [TestCategory("UnitTest")]
      public async Task UserStoreFindByEmailAsync()
      {
         var item = await this.store.FindByEmailAsync("user2@bar.com");
         item.Email.Should().Be("user2@bar.com");
      }

      [TestMethod]
      [TestCategory("UnitTest")]
      public async Task UserStoreFindByLoginAsync()
      {
         var item = await this.store.FindAsync(new UserLoginInfo("facebook", "facebookkey"));
         item.Email.Should().Be("user2@bar.com");
      }

      [TestMethod]
      [TestCategory("UnitTest")]
      public async Task UserStoreFindByIdAsync()
      {
         var id = FakeUserData.Users[1].Id;
         var item = await this.store.FindByIdAsync(id);
         item.Email.Should().Be("user2@bar.com");
      }
   }
}