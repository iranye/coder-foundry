using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Helpers;
using BugTracker.Models;

namespace BugTracker.ViewModels
{
    public class MainDashboardViewModel
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly RoleHelper _roleHelper = new RoleHelper();
        private readonly NotificationHelper _notificationHelper = new NotificationHelper();
        private readonly TicketHelper _ticketHelper = new TicketHelper();
        private readonly TicketHistoryHelper _ticketHistoryHelper = new TicketHistoryHelper();

        public int OpenTickets
        {
            get
            {
                return _db.Tickets.Count(t => t.TicketStatus.Name == "Open");
            }
        }

        public List<Ticket> AllTickets
        {
            get
            {
                return _db.Tickets.ToList();

            }
        }

        public int TotalTickets
        {
            get
            {
                return _db.Tickets.Count();
            }
        }

        public int TotalProjects
        {
            get
            {
                return _db.Projects.Count();
            }
        }

        public int TotalNotifications
        {
            get
            {
                return _db.TicketNotifications.Count();
            }
        }
    }
}
