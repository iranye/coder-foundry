using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    public enum TicketAssignmentChange
    {
        Unknown = 0,
        Assigned,
        UnAssigned,
        ReAssigned
    }

    public class NotificationHelper
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        private TicketAssignmentChange GetTicketAssignmentChange(string assignedToIdOld, string assignedToIdNew)
        {
            TicketAssignmentChange ticketAssignmentChange = TicketAssignmentChange.Unknown;

            if (assignedToIdOld == null && assignedToIdNew != null)
            {
                ticketAssignmentChange = TicketAssignmentChange.Assigned;
            }
            if (assignedToIdOld != null && assignedToIdNew == null)
            {
                ticketAssignmentChange = TicketAssignmentChange.UnAssigned;
            }
            if (assignedToIdOld != null && assignedToIdNew != null 
                                               && assignedToIdOld != assignedToIdNew)
            {
                ticketAssignmentChange = TicketAssignmentChange.ReAssigned;
            }

            return ticketAssignmentChange;
        }

         public List<TicketNotification> GetAssignmentNotifications(Ticket oldTicket, Ticket newTicket)
         {
            var notifications = new List<TicketNotification>();

            var notificationCreationDateTime = newTicket.Updated.GetValueOrDefault();

            if (notificationCreationDateTime == DateTime.MinValue)
            {
                notificationCreationDateTime = DateTime.Now;
            }

            switch (GetTicketAssignmentChange(oldTicket.AssignedToId, newTicket.AssignedToId))
            {
                case TicketAssignmentChange.Assigned:
                    var notificationAssigned = new TicketNotification
                    {
                        TicketId = newTicket.Id,
                        Created = notificationCreationDateTime,
                        Subject = "Ticket Assignment Notification",
                        IsRead = false,
                        RecipientId = newTicket.AssignedToId,
                        NotificationBody =
                            $"A Ticket has been assigned to You: TicketID={newTicket.Id} '{newTicket.Title}' for Project {newTicket.Project.Name}"
                    };
                    notifications.Add(notificationAssigned);
                    break;

                case TicketAssignmentChange.UnAssigned:
                    var notificationUnAssigned = new TicketNotification
                    {
                        TicketId = newTicket.Id,
                        Created = notificationCreationDateTime,
                        Subject = "Ticket Assignment Notification",
                        IsRead = false,
                        RecipientId = oldTicket.AssignedToId,
                        NotificationBody =
                            $"A Ticket has been un-assigned from You: TicketID={newTicket.Id} '{newTicket.Title}' for Project {newTicket.Project.Name}"
                    };
                    notifications.Add(notificationUnAssigned);
                    break;

                case TicketAssignmentChange.ReAssigned:
                    var notificationUnAssgnd = new TicketNotification
                    {
                        TicketId = newTicket.Id,
                        Created = notificationCreationDateTime,
                        Subject= "Ticket Assignment Notification",
                        IsRead = false,
                        RecipientId = oldTicket.AssignedToId,
                        NotificationBody =
                            $"A Ticket has been un-assigned from You: TicketID={newTicket.Id} '{newTicket.Title}' for Project {newTicket.Project.Name}"
                    };
                    notifications.Add(notificationUnAssgnd);

                    var notificationAssgned = new TicketNotification
                    {
                        TicketId = newTicket.Id,
                        Created = notificationCreationDateTime,
                        Subject = "Ticket Assignment Notification",
                        IsRead = false,
                        RecipientId = newTicket.AssignedToId,
                        NotificationBody =
                            $"A Ticket has been assigned to You: TicketID={newTicket.Id} '{newTicket.Title}' for Project {newTicket.Project.Name}"
                    };
                    notifications.Add(notificationAssgned);

                    break;
                default:
                    break;
            }
            return notifications;
        }
    }
}
