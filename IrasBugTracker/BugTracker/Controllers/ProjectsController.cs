using System;
using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly RoleHelper _roleHelper = new RoleHelper();
        private readonly ProjectHelper _projectHelper = new ProjectHelper();

        [Authorize(Roles = "Admin, ProjectManager, DemoAdmin, DemoProjectManager")]
        public ActionResult Index()
        {
            return View(_db.Projects.ToList());
        }

        public ActionResult AssignedIndex()
        {
            var currentUserId = User.Identity.GetUserId();
            var projectsAssigned = _db.Projects.Where(p => p.Members.Any(m => m.Id == currentUserId));
            return View(projectsAssigned.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        [Authorize(Roles = "Admin, ProjectManager, DemoAdmin, DemoProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin, ProjectManager, DemoAdmin, DemoProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                _db.Projects.Add(project);
                _db.SaveChanges();

                var userId = User.Identity.GetUserId();
                var role = _roleHelper.GetRoleByUserId(userId);
                if (role.Contains("ProjectManager"))
                {
                    var pmUser = _db.Users.Find(userId);
                    project.Members.Add(pmUser);
                    _db.SaveChanges();
                    return RedirectToAction("Details", new {id=project.Id});
                }
                return RedirectToAction("Index");
            }

            return View(project);
        }

        [Authorize(Roles = "Admin, ProjectManager, DemoAdmin, DemoProjectManager")]
        public ActionResult ManageMembers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _db.Projects.Find(id);
            if (project == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ProjectMembersViewModel projectMembersViewModel = new ProjectMembersViewModel(project);

            ViewBag.ProjectId = id;

            var users = new List<ManageMembersViewModel>();
            foreach (var user in _db.Users.ToList())
            {
                if (project.Members.Contains(user))
                {
                    projectMembersViewModel.Members.Add(
                        new ManageMembersViewModel
                        {
                            Id = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}",
                            Email = user.Email,
                            Role = _roleHelper.GetRoleByUserId(user.Id),
                            IsMember = true
                        });
                }
                else
                {
                    projectMembersViewModel.NonMembers.Add(
                        new ManageMembersViewModel
                        {
                            Id = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}",
                            Email = user.Email,
                            Role = _roleHelper.GetRoleByUserId(user.Id),
                            IsMember = false
                        });
                }
            }
            ViewBag.MembersToAdd = new SelectList(new List<string>());
            ViewBag.MembersToRemove = new SelectList(new List<string>());

            return View(projectMembersViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager, DemoAdmin, DemoProjectManager")]
        public ActionResult ManageMembers(List<string> membersToRemove, List<string> membersToAdd, int projectId = 0)
        {
            Project project = _db.Projects.Find(projectId);
            if (project == null)
            {
                return RedirectToAction("Index", "Projects");
            }

            // Remove Action
            if (membersToRemove != null)
            {
                foreach (var email in membersToRemove)
                {
                    _projectHelper.RemoveUserFromProjectByEmail(email, projectId);
                }
            }

            // Add Action
            if (membersToAdd != null)
            {
                foreach (var email in membersToAdd)
                {
                    _projectHelper.AddUserToProjectByEmail(email, projectId);
                }
            }

            return RedirectToAction("ManageMembers", "Projects", new {id = projectId});
        }

        [Authorize(Roles = "Admin, ProjectManager, DemoAdmin, DemoProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [Authorize(Roles = "Admin, ProjectManager, DemoAdmin, DemoProjectManager")]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(project).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
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
