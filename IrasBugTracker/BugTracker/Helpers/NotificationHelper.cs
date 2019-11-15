using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        private static ApplicationDbContext _db = new ApplicationDbContext();

        public static List<TicketNotification> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return _db.TicketNotifications.Where(n => !n.IsRead && n.RecipientId == currentUserId).ToList();
        }

        public static int GetUnreadNotificationsCount()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return _db.TicketNotifications.Count(n => !n.IsRead && n.RecipientId == currentUserId);
        }

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
                        Subject = $"Ticket Assignment Notification - TicketID {newTicket.DisplayableId}",
                        IsRead = false,
                        RecipientId = newTicket.AssignedToId,
                        NotificationBody =
                            $"Ticket {newTicket.DisplayableId} has been assigned to you - '{newTicket.Title}' for Project {newTicket.Project.Name}"
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

         public List<TicketNotification> GetTicketChangeNotifications(List<TicketHistory> ticketChanges, Ticket ticket)
         {
             var notifications = new List<TicketNotification>();

             var notificationCreationDateTime = ticket.Updated.GetValueOrDefault();

             if (notificationCreationDateTime == DateTime.MinValue)
             {
                 notificationCreationDateTime = DateTime.Now;
             }

             foreach (var change in ticketChanges)
             {
                 var notification = new TicketNotification
                 {
                     TicketId = ticket.Id,
                     Created = notificationCreationDateTime,
                     Subject =
                         $"A ticket you're Assigned to has been updated - {ticket.Title}",
                     IsRead = false,
                     RecipientId = ticket.AssignedToId,
                     NotificationBody =
                         $"Ticket '{ticket.Title}' ({ticket.DisplayableId}): {change.Property} has been updated from {change.OldValue} to {change.NewValue} by {change.ChangedBy.DisplayName}"
                 };
                 notifications.Add(notification);
             }
             return notifications;
         }
    }
}
