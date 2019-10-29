using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BugTracker.ViewModels;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly RoleHelper roleHelper = new RoleHelper();

        // GET: Admin
        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name");

            var users = new List<ManageRolesViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(
                    new ManageRolesViewModel
                    {
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Role = roleHelper.GetUserCurrentlyAssignedRoles(user.Id).FirstOrDefault()
                    });
            }
            return View(users);
        }

        /// <summary>
        /// Take a list of UserIDs and a Role to assign them to.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string role)
        {
            foreach (var userId in userIds)
            {
                var currentRole = roleHelper.GetUserCurrentlyAssignedRoles(userId).FirstOrDefault();
                if (currentRole != null)
                {
                    roleHelper.RemoveUserFromRole(userId, currentRole);
                }
            }

            if (String.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("ManageRoles", "ManageUsers");
            }
            foreach (var userId in userIds)
            {
                roleHelper.AddUserToRole(userId, role);
            }
            return RedirectToAction("ManageRoles", "ManageUsers");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
