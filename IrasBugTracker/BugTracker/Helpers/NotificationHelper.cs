using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public void ManageNotifications(string userId, Ticket oldTicket, Ticket newTicket)
        {
            var ticketHasBeenAssigned = oldTicket.AssignedToId == null && newTicket.AssignedToId != null;
            var ticketHasBeenUnAssigned = oldTicket.AssignedToId != null && newTicket.AssignedToId == null;
            var ticketHasBeenReAssigned = oldTicket.AssignedToId != null && newTicket.AssignedToId != oldTicket.AssignedToId;

        }

        private void AddAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                //IsRead = false
                RecipientId = newTicket.AssignedToId,
                NotificationBody =
                    $"A Ticket has been assigned to You {newTicket.Id} '{newTicket.Title}' for Project {newTicket.Project.Name}"
            };

            _db.TicketNotifications.Add(notification);
            _db.SaveChanges();
        }
    }
}
