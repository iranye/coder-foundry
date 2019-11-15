using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helpers
{
    // Need 1 History Record for each Property that Changed
    public class TicketHistoryHelper
    {
        public List<TicketHistory> GetChanges(ApplicationUser user, Ticket oldTicket, Ticket newTicket)
        {
            var changes = new List<TicketHistory>();

            var updateDateTime = newTicket.Updated.GetValueOrDefault();

            if (updateDateTime == DateTime.MinValue)
            {
                updateDateTime = DateTime.Now;
            }

            if (oldTicket.Title != newTicket.Title)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Title", oldTicket.Title, newTicket.Title, updateDateTime, user);
                changes.Add(changeRecord);
            }

            if (oldTicket.Description != newTicket.Description)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Description", oldTicket.Description, newTicket.Description, updateDateTime, user);
                changes.Add(changeRecord);
            }

            if (oldTicket.ProjectId != newTicket.ProjectId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Project", oldTicket.Project.Name, newTicket.Project.Name, updateDateTime, user);
                changes.Add(changeRecord);
            }

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Priority", oldTicket.TicketPriority.Name, newTicket.TicketPriority.Name, updateDateTime, user);
                changes.Add(changeRecord);
            }

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Status", oldTicket.TicketStatus.Name, newTicket.TicketStatus.Name, updateDateTime, user);
                changes.Add(changeRecord);
            }

            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Type", oldTicket.TicketType.Name, newTicket.TicketType.Name, updateDateTime, user);
                changes.Add(changeRecord);
            }

            if (oldTicket.OwnerId != newTicket.OwnerId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Owner", oldTicket.Owner.DisplayName, newTicket.Owner.DisplayName, updateDateTime, user);
                changes.Add(changeRecord);
            }

            if (oldTicket.AssignedToId != newTicket.AssignedToId)
            {
                var oldAssignee = oldTicket.AssignedToId == null ? "Unassigned" : oldTicket.AssignedTo.DisplayName;
                var newAssignee = newTicket.AssignedToId == null ? "Unassigned" : newTicket.AssignedTo.DisplayName;
                var changeRecord = GetChangeRecord(newTicket.Id, "Assignee", oldAssignee, newAssignee, updateDateTime, user);
                changes.Add(changeRecord);
            }

            return changes;
        }

        private TicketHistory GetChangeRecord(int ticketId, string propName, string oldValue, string newValue, DateTime updateDateTime, ApplicationUser changedByUser)
        {
            return new TicketHistory
            {
                TicketId = ticketId,
                Property = propName,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedDateTime = updateDateTime,
                ChangedById = changedByUser.Id,
                ChangedBy = changedByUser
            };
        }
    }
}
