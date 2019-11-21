using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialPortal.Web.Helpers
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
                new List<string> { "" };
            }

            return userManager.GetRoles(user.Id);
        }

        /// <summary>
        /// Retrieve the Role with highest Access-Level
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>The role by UserID w/ highest Access-Level</returns>
        public string GetRoleByUserId(string userId)
        {
            var roles = ListUserRolesByUserId(userId);
            var role = roles.FirstOrDefault();
            if (roles.Count > 1)
            {
                role = roles.Contains("Admin") ? "Admin"
                    : roles.Contains("HeadOfHousehold") ? "HeadOfHousehold"
                    : roles.Contains("Member") ? "Member"
                    : role;
            }

            return role;
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
            if (user == null)
            {
                return "";
            }
            if (String.IsNullOrWhiteSpace(user.DisplayName))
            {
                return $"{user.FirstName}";
            }
            return user.DisplayName;
        }

        public IEnumerable<ApplicationUser> UsersInRole(string role)
        {
            ICollection<ApplicationUser> usersInRole = new HashSet<ApplicationUser>();
            if (String.IsNullOrWhiteSpace(role))
            {
                return usersInRole;
            }

            int maxLen = 50;
            if (role.Length > maxLen)
            {
                role = role.Substring(0, maxLen);
            }
            SqlParameter param1 = new SqlParameter("@Role", role);

            var userIds = _db.Database.SqlQuery<string>("GetUsersInRole @Role", param1).ToList();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    var user = userManager.FindById(userId);
                    if (user != null)
                    {
                        usersInRole.Add(user);
                    }
                }
            }

            return usersInRole;
        }
    }
}

