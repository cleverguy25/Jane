// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeUserData.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Test
{
   using System;
   using System.Collections.Generic;
   using System.Collections.ObjectModel;
   using System.Linq;
   using System.Security.Claims;
   using System.Threading.Tasks;

   using Jane.Identity.Models;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   using Microsoft.AspNet.Identity;

   using Moq;

   public class FakeUserData : IStorage<User, Guid>
   {
      public static readonly User[] Users =
         {
            new User()
               {
                  Id = Guid.NewGuid(),
                  Email = "user1@foo.com",
                  Logins =
                     new Collection<UserLoginInfo>
                        {
                           new UserLoginInfo("google", "googlekey")
                        },
                  Claims =
                     new Collection<Claim>() { new Claim("role", "admin") }
               },
            new User()
               {
                  Id = Guid.NewGuid(),
                  Email = "user2@bar.com",
                  Logins =
                     new Collection<UserLoginInfo>
                        {
                           new UserLoginInfo("facebook", "facebookkey")
                        },
                  Claims =
                     new Collection<Claim>() { new Claim("role", "user") }
               },
            new User()
               {
                  Id = Guid.NewGuid(),
                  Email = "user3@fubar.com",
                  Logins =
                     new Collection<UserLoginInfo>
                        {
                           new UserLoginInfo("twitter", "twitterkey")
                        },
                  Claims =
                     new Collection<Claim>()
                        {
                           new Claim("role", "admin"),
                           new Claim("role", "superadmin")
                        }
               }
         };

      public static readonly Role[] Roles = new Role[]
         {
            new Role() { Id = Guid.NewGuid(), Name = "admin" },
            new Role() { Id = Guid.NewGuid(), Name = "user" }
         };

      private readonly List<User> userData = new List<User>(Users);
      
      public static ILoadStorage<Role, Guid> GetRoleStorage()
      {
         var mock = new Mock<ILoadStorage<Role, Guid>>();
         mock.Setup(storage => storage.LoadAsync()).Returns(Task.FromResult(Roles.AsEnumerable()));
         return mock.Object;
      }

      public Task<IEnumerable<User>> LoadAsync()
      {
         return Task.FromResult(this.userData.AsEnumerable());
      }

      public Task<User> LoadAsync(Guid id)
      {
         return Task.FromResult(this.userData.Single(user => user.Id == id));
      }

      public Task<Guid> AddAsync(User item)
      {
         this.userData.Add(item);
         return Task.FromResult(item.Id);
      }

      public Task UpdateAsync(User item)
      {
         var index = this.userData.FindIndex(user => user.Id == item.Id);
         if (index >= 0)
         {
            this.userData[index] = item;
         }

         return Task.FromResult(0);
      }

      public Task DeleteAsync(User item)
      {
         return this.DeleteAsync(item.Id);
      }

      public Task DeleteAsync(Guid id)
      {
         var index = this.userData.FindIndex(user => user.Id == id);
         if (index >= 0)
         {
            this.userData.RemoveAt(index);
         }

         return Task.FromResult(0);
      }
   }
}