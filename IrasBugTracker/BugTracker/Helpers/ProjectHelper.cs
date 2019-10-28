using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        public bool UserIsOnProject(string userId, int projectId)
        {
            bool ret = false;
            var project = db.Projects.Find(projectId);
            if (project != null)
            {
                return project.Members.Any(u => u.Id == userId);
            }

            return ret;
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            return user == null ? null : user.Projects.ToList();
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (UserIsOnProject(userId, projectId))
            {
                return;
            }

            var project = db.Projects.Find(projectId);
            if (project != null)
            {
                ApplicationUser user = db.Users.Find(userId);
                if (user != null)
                {
                    project.Members.Add(user);
                    db.SaveChanges();
                }
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (!UserIsOnProject(userId, projectId))
            {
                return;
            }

            var project = db.Projects.Find(projectId);
            if (project != null)
            {
                ApplicationUser user = db.Users.Find(userId);
                if (user != null)
                {
                    project.Members.Remove(user);
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public ICollection<ApplicationUser> AllUsersOnProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
            {
                return null;
            }
            return project.Members;
        }

        public ICollection<ApplicationUser> AllUsersNotOnProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
            {
                return null;
            }
            return project.Members.Where(m => m.Projects.All(p => p.Id != projectId)).ToList();
        }


    }
}
