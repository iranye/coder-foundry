using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private readonly ProjectHelper _projectHelper = new ProjectHelper();
        private readonly RoleHelper _roleHelper = new RoleHelper();

        public int SetDefaultTicketStatus()
        {
            int ret = 0;

            if (_db.TicketStatuses != null)
            {
                ret = _db.TicketStatuses.FirstOrDefault(ts => ts.Name == "Open").Id;
            }
            return ret;
        }

        public List<Ticket> ListMyTickets()
        {
            List<Ticket> myTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = _db.Users.Find(userId);

            var currentRoleByUserId = _roleHelper.GetRoleByUserId(userId);

            switch (currentRoleByUserId)
            {
                case "Admin":
                case "DemoAdmin":
                    myTickets.AddRange(_db.Tickets);
                    break;
                case "ProjectManager":
                case "DemoProjectManager":
                    myTickets.AddRange(user.Projects.SelectMany(p => p.Tickets));
                    break;
                case "Developer":
                case "DemoDeveloper":
                    myTickets.AddRange(_db.Tickets.Where(t => t.AssignedToId == userId));
                    break;
                case "Submitter":
                case "DemoSubmitter":
                    myTickets.AddRange(_db.Tickets.Where(t => t.OwnerId == userId));
                    break;
                default:
                    break;
            }

            return myTickets;
        }

        /// <summary>
        /// Determine if User has permission to add Content (e.g., Comments or Attachments) to a Ticket.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public bool CanAddContent(string userId, Ticket ticket)
        {
            bool ret = false;
            ICollection<string> currentRolesByUserId = _roleHelper.ListUserRolesByUserId(userId);
            ret = currentRolesByUserId.Contains("Admin") || currentRolesByUserId.Contains("DemoAdmin");

            if (!ret)
            {
                ret = (currentRolesByUserId.Contains("ProjectManager") || currentRolesByUserId.Contains("DemoProjectManager"))
                      && _projectHelper.UserIsOnProject(userId, ticket.ProjectId);
            }
            if (!ret)
            {
                ret = userId == ticket.AssignedToId || userId == ticket.OwnerId;
            }

            return ret;
        }

        public bool CanEdit(string userId, Ticket ticket)
        {
            bool ret = false;
            ICollection<string> currentRolesByUserId = _roleHelper.ListUserRolesByUserId(userId);
            ret = currentRolesByUserId.Contains("Admin") || currentRolesByUserId.Contains("DemoAdmin");

            if (!ret)
            {
                ret = (currentRolesByUserId.Contains("ProjectManager") || currentRolesByUserId.Contains("DemoProjectManager"))
                      && _projectHelper.UserIsOnProject(userId, ticket.ProjectId);
            }
            if (!ret)
            {
                ret = userId == ticket.AssignedToId || userId == ticket.OwnerId;
            }

            return ret;
        }

        public bool CanChangeAssignment(string userId, Ticket ticket)
        {
            bool ret = false;
            ICollection<string> currentRolesByUserId = _roleHelper.ListUserRolesByUserId(userId);
            ret = currentRolesByUserId.Contains("Admin") || currentRolesByUserId.Contains("DemoAdmin");

            if (!ret)
            {
                ret = (currentRolesByUserId.Contains("ProjectManager") || currentRolesByUserId.Contains("DemoProjectManager"))
                      && _projectHelper.UserIsOnProject(userId, ticket.ProjectId);
            }

            return ret;
        }

        public bool CanChangeStatus(string userId, Ticket ticket)
        {
            bool ret = CanChangeAssignment(userId, ticket);
            if (!ret)
            {
                if (ticket.AssignedToId == userId)
                {
                    ret = true;
                }
            }

            return ret;
        }
    }
}
