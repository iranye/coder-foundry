using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Helpers
{
    public class RoleHelper
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public void AddUserToRole(string userId, string role)
        {
            string message = "";
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));

            var user = userManager.FindById(userId);
            if (user == null)
            {
                message = $"User '{userId}' NOT FOUND";
                throw new Exception(message);
            }

            var roleFromDb = _db.Roles.FirstOrDefault(r => r.Name == role);
            if (roleFromDb == null)
            {
                message = $"Invalid Role '{role}'";
                throw new Exception(message);
            }
            userManager.AddToRole(user.Id, role);
        }

        public void RemoveUserFromRole(string userId, string roleToRemove)
        {
            string message = "";
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));

            var user = userManager.FindById(userId);
            if (user == null)
            {
                message = $"User '{userId}' NOT FOUND";
                throw new Exception(message);
            }

            IdentityRole roleFromDb = _db.Roles.FirstOrDefault(r => r.Name == roleToRemove);
            if (roleFromDb == null)
            {
                message = $"Invalid Role '{roleToRemove}'";
                throw new Exception(message);
            }

            userManager.RemoveFromRole(user.Id, roleToRemove);
        }

        /// <summary>
        /// Get a User's Currently Assigned Role(s)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ICollection<string> ListUserRolesByUserId(string userId)
        {
            string message = "";
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));

            var user = userManager.FindById(userId);
            if (user == null)
            {
                message = $"User '{userId}' NOT FOUND";
                throw new Exception(message);
            }

            return userManager.GetRoles(user.Id);
        }

        public bool UserIsInRole(string userId, string roleName)
        {
            var currentRolesByUserId = ListUserRolesByUserId(userId);
            return currentRolesByUserId.Contains(roleName);
        }

        public string GetDisplayName(string userId)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            var user = userManager.FindById(userId);
            if (String.IsNullOrWhiteSpace(user.DisplayName))
            {
                return $"{user.FirstName}";
            }
            return user.DisplayName;
        }
    }
}
