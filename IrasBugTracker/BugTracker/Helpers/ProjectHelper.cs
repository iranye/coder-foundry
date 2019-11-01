using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly RoleHelper _roleHelper = new RoleHelper();

        public bool UserIsOnProject(string userId, int projectId)
        {
            bool ret = false;
            var project = _db.Projects.Find(projectId);
            if (project != null)
            {
                return project.Members.Any(u => u.Id == userId);
            }

            return ret;
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = _db.Users.Find(userId);
            return user == null ? null : user.Projects.ToList();
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (UserIsOnProject(userId, projectId))
            {
                return;
            }

            var project = _db.Projects.Find(projectId);
            if (project != null)
            {
                ApplicationUser user = _db.Users.Find(userId);
                if (user != null)
                {
                    project.Members.Add(user);
                    _db.SaveChanges();
                }
            }
        }

        public void AddUserToProjectByEmail(string email, int projectId)
        {
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return;
            }
            if (UserIsOnProject(user.Id, projectId))
            {
                return;
            }

            var project = _db.Projects.Find(projectId);
            if (project != null)
            {
                project.Members.Add(user);
                _db.SaveChanges();
            }
        }

        public void RemoveUserFromProjectByEmail(string email, int projectId)
        {
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return;
            }
            if (!UserIsOnProject(user.Id, projectId))
            {
                return;
            }

            var project = _db.Projects.Find(projectId);
            if (project != null)
            {
                project.Members.Remove(user);
                _db.Entry(project).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (!UserIsOnProject(userId, projectId))
            {
                return;
            }

            var project = _db.Projects.Find(projectId);
            if (project != null)
            {
                ApplicationUser user = _db.Users.Find(userId);
                if (user != null)
                {
                    project.Members.Remove(user);
                    _db.Entry(project).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
        }

        public ICollection<ApplicationUser> AllUsersOnProject(int projectId)
        {
            var project = _db.Projects.Find(projectId);
            if (project == null)
            {
                return new HashSet<ApplicationUser>();
            }
            return project.Members;
        }

        public ICollection<ApplicationUser> AllUsersNotOnProject(int projectId)
        {
            var project = _db.Projects.Find(projectId);
            if (project == null)
            {
                return null;
            }
            return project.Members.Where(m => m.Projects.All(p => p.Id != projectId)).ToList();
        }

        public List<string> ListUsersOnProjectInRole(int projectId, string roleName)
        {
            var userInSpecifiedRole = new List<string>();
            foreach (var user in AllUsersOnProject(projectId))
            {
                if(_roleHelper.UserIsInRole(user.Id, roleName))
                {
                    userInSpecifiedRole.Add(user.Id);
                }
            }

            return userInSpecifiedRole;
        }

        public IEnumerable UsersOnProject(int projectId)
        {
            throw new NotImplementedException();
        }
    }
}
