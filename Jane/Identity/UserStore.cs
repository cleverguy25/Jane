// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserStore.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Identity
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Security.Claims;
   using System.Threading.Tasks;

   using Jane.Identity.Models;
   using Jane.Infrastructure.Interfaces;

   using Microsoft.AspNet.Identity;

   public class UserStore : IQueryableUserStore<User, Guid>, 
                            IUserPasswordStore<User, Guid>, 
                            IUserLoginStore<User, Guid>, 
                            IUserClaimStore<User, Guid>, 
                            IUserRoleStore<User, Guid>, 
                            IUserSecurityStampStore<User, Guid>, 
                            IUserTwoFactorStore<User, Guid>, 
                            IUserLockoutStore<User, Guid>,
                            IUserEmailStore<User, Guid>
   {
      private readonly ILoadStorage<Role, Guid> roleStorage;

      private readonly IStorage<User, Guid> userStorage;

      public UserStore(IStorage<User, Guid> userStorage, ILoadStorage<Role, Guid> roleStorage)
      {
         this.userStorage = userStorage;
         this.roleStorage = roleStorage;
      }

      public IQueryable<User> Users
      {
         get
         {
            return this.GetUsers().AsQueryable();
         }
      }

      public Task CreateAsync(User user)
      {
         ValidateUser(user);

         return this.userStorage.AddAsync(user);
      }

      public Task UpdateAsync(User user)
      {
         ValidateUser(user);

         return this.userStorage.UpdateAsync(user);
      }

      public Task DeleteAsync(User user)
      {
         ValidateUser(user);

         return this.userStorage.DeleteAsync(user.Id);
      }

      public Task<User> FindByIdAsync(Guid userId)
      {
         return this.userStorage.LoadAsync(userId);
      }

      public async Task<User> FindByNameAsync(string userName)
      {
         var users = await this.GetUsersAsync();
         return users.FirstOrDefault(user => user.Email == userName);
      }

      public void Dispose()
      {
      }

      public Task<IList<Claim>> GetClaimsAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult<IList<Claim>>(user.Claims.ToList());
      }

      public async Task AddClaimAsync(User user, Claim claim)
      {
         ValidateUser(user);
         ValidateClaim(claim);

         user.Claims.Add(claim);
         await this.userStorage.UpdateAsync(user);
      }

      public async Task RemoveClaimAsync(User user, Claim claim)
      {
         ValidateUser(user);
         ValidateClaim(claim);

         var claims = user.Claims.Where(c => c.Type == claim.Type && c.Value == claim.Value);
         foreach (var item in claims)
         {
            user.Claims.Remove(item);
         }

         await this.userStorage.UpdateAsync(user);
      }

      public Task<int> GetAccessFailedCountAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult(user.AccessFailedCount);
      }

      public Task<bool> GetLockoutEnabledAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult(user.LockoutEnabled);
      }

      public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
      {
         ValidateUser(user);

         return
            Task.FromResult(
               user.LockoutEndDateUtc.HasValue
                  ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                  : new DateTimeOffset());
      }

      public async Task<int> IncrementAccessFailedCountAsync(User user)
      {
         ValidateUser(user);

         user.AccessFailedCount++;
         await this.userStorage.UpdateAsync(user);
         return user.AccessFailedCount;
      }

      public async Task ResetAccessFailedCountAsync(User user)
      {
         ValidateUser(user);

         user.AccessFailedCount = 0;
         await this.userStorage.UpdateAsync(user);
      }

      public async Task SetLockoutEnabledAsync(User user, bool enabled)
      {
         ValidateUser(user);

         user.LockoutEnabled = enabled;
         await this.userStorage.UpdateAsync(user);
      }

      public async Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
      {
         ValidateUser(user);

         user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? null : (DateTime?)lockoutEnd.UtcDateTime;
         await this.userStorage.UpdateAsync(user);
      }

      public async Task<User> FindAsync(UserLoginInfo login)
      {
         ValidateLogin(login);

         var users = await this.GetUsersAsync();

         var provider = login.LoginProvider;
         var key = login.ProviderKey;
         return users.SingleOrDefault(user => user.Logins.Any(l => l.LoginProvider == provider && l.ProviderKey == key));
      }

      public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
      {
         ValidateUser(user);

         return
            Task.FromResult<IList<UserLoginInfo>>(
               user.Logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList());
      }

      public async Task AddLoginAsync(User user, UserLoginInfo login)
      {
         ValidateUser(user);
         ValidateLogin(login);

         user.Logins.Add(login);
         await this.userStorage.UpdateAsync(user);
      }

      public async Task RemoveLoginAsync(User user, UserLoginInfo login)
      {
         ValidateUser(user);
         ValidateLogin(login);

         var provider = login.LoginProvider;
         var key = login.ProviderKey;

         var item = user.Logins.SingleOrDefault(l => l.LoginProvider == provider && l.ProviderKey == key);

         if (item != null)
         {
            user.Logins.Remove(item);
         }

         await this.userStorage.UpdateAsync(user);
      }

      public Task<string> GetPasswordHashAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult(user.PasswordHash);
      }

      public Task<bool> HasPasswordAsync(User user)
      {
         return Task.FromResult(user.PasswordHash != null);
      }

      public Task SetPasswordHashAsync(User user, string passwordHash)
      {
         ValidateUser(user);

         user.PasswordHash = passwordHash;
         return Task.FromResult(0);
      }

      public async Task AddToRoleAsync(User user, string roleName)
      {
         ValidateUser(user);
         ValidateRole(roleName);

         var roles = await this.roleStorage.LoadAsync();
         var role = roles.FirstOrDefault(r => r.Name == roleName);
         if (role == null)
         {
            throw new ArgumentException("Role not found.", "roleName");
         }

         user.Roles.Add(role);
         await this.userStorage.UpdateAsync(user);
      }

      public Task<IList<string>> GetRolesAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult<IList<string>>(user.Roles.Select(role => role.Name).ToList());
      }

      public Task<bool> IsInRoleAsync(User user, string roleName)
      {
         ValidateUser(user);
         ValidateRole(roleName);

         return Task.FromResult(user.Roles.Any(r => r.Name == roleName));
      }

      public async Task RemoveFromRoleAsync(User user, string roleName)
      {
         ValidateUser(user);
         ValidateRole(roleName);

         var userRole = user.Roles.SingleOrDefault(r => r.Name == roleName);

         if (userRole != null)
         {
            user.Roles.Remove(userRole);
         }

         await this.userStorage.UpdateAsync(user);
      }

      public Task<string> GetSecurityStampAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult(user.SecurityStamp);
      }

      public async Task SetSecurityStampAsync(User user, string stamp)
      {
         ValidateUser(user);

         user.SecurityStamp = stamp;
         await this.userStorage.UpdateAsync(user);
      }

      public Task<bool> GetTwoFactorEnabledAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult(user.TwoFactorEnabled);
      }

      public async Task SetTwoFactorEnabledAsync(User user, bool enabled)
      {
         ValidateUser(user);

         user.TwoFactorEnabled = enabled;
         await this.userStorage.UpdateAsync(user);
      }
      
      public async Task SetEmailAsync(User user, string email)
      {
         ValidateUser(user);

         user.Email = email;
         await this.userStorage.UpdateAsync(user);
      }

      public Task<string> GetEmailAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult(user.Email);
      }

      public Task<bool> GetEmailConfirmedAsync(User user)
      {
         ValidateUser(user);

         return Task.FromResult(user.EmailConfirmed);
      }

      public async Task SetEmailConfirmedAsync(User user, bool confirmed)
      {
         ValidateUser(user);

         user.EmailConfirmed = confirmed;
         await this.userStorage.UpdateAsync(user);
      }

      public async Task<User> FindByEmailAsync(string email)
      {
         var users = await this.GetUsersAsync();
         return users.FirstOrDefault(user => user.Email == email);
      }

      private static void ValidateUser(User user)
      {
         if (user == null)
         {
            throw new ArgumentNullException("user");
         }
      }

      private static void ValidateLogin(UserLoginInfo login)
      {
         if (login == null)
         {
            throw new ArgumentNullException("login");
         }
      }

      private static void ValidateClaim(Claim claim)
      {
         if (claim == null)
         {
            throw new ArgumentNullException("claim");
         }
      }

      private static void ValidateRole(string roleName)
      {
         if (string.IsNullOrWhiteSpace(roleName))
         {
            throw new ArgumentException("Value cannot be null or empty.", "roleName");
         }
      }

      private IEnumerable<User> GetUsers()
      {
         var task = this.userStorage.LoadAsync();
         task.Wait();
         return task.Result;
      }

      private async Task<IEnumerable<User>> GetUsersAsync()
      {
         return await this.userStorage.LoadAsync();
      }
   }
}