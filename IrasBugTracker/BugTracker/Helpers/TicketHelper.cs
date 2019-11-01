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

            var myRoles = _roleHelper.ListUserRolesByUserId(userId);
            var myRole = myRoles.FirstOrDefault();
            if (myRoles.Count > 1)
            {
                myRole = myRoles.Contains("Admin") ? "Admin"
                    : myRoles.Contains("ProjectManager") ? "ProjectManager"
                    : myRoles.Contains("Developer") ? "Developer"
                    : myRole;
            }

            switch (myRole)
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
    }
}
