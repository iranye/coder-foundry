﻿using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin, DemoAdmin")]
    public class ManageUsersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly RoleHelper _roleHelper = new RoleHelper();

        public ActionResult ManageRoles()
        {
            ViewBag.Role = new SelectList(_db.Roles, "Name", "Name");

            var users = new List<ManageRolesViewModel>();
            foreach (var user in _db.Users.ToList())
            {
                users.Add(
                    new ManageRolesViewModel
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Role = _roleHelper.ListUserRolesByUserId(user.Id).FirstOrDefault()
                    });
            }
            ViewBag.UserIds = new MultiSelectList(users.OrderBy(u => u.FullName), "UserId", "FullName");
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
            var currentUserId = User.Identity.GetUserId();
            var currentUserRoles = _roleHelper.ListUserRolesByUserId(currentUserId);
            
            if (!currentUserRoles.Contains("Admin"))
            {
                return RedirectToAction("ManageRoles", "ManageUsers");
            }
            foreach (var userId in userIds)
            {
                foreach (var currentlyAssignedRole in _roleHelper.ListUserRolesByUserId(userId))
                {
                    _roleHelper.RemoveUserFromRole(userId, currentlyAssignedRole);
                }
            }

            if (String.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("ManageRoles", "ManageUsers");
            }
            foreach (var userId in userIds)
            {
                _roleHelper.AddUserToRole(userId, role);
            }
            return RedirectToAction("ManageRoles", "ManageUsers");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
