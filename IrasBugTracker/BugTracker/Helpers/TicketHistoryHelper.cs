using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    // Need 1 History Record for each Property that Changed
    public class TicketHistoryHelper
    {
        public List<TicketHistory> GetChanges(string userId, Ticket oldTicket, Ticket newTicket)
        {
            var changes = new List<TicketHistory>();

            var updateDateTime = newTicket.Updated.GetValueOrDefault();

            if (updateDateTime == DateTime.MinValue)
            {
                updateDateTime = DateTime.Now;
            }

            if (oldTicket.Title != newTicket.Title)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Title", oldTicket.Title, newTicket.Title, updateDateTime, userId);
                changes.Add(changeRecord);
            }

            if (oldTicket.Description != newTicket.Description)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Description", oldTicket.Description, newTicket.Description, updateDateTime, userId);
                changes.Add(changeRecord);
            }

            if (oldTicket.ProjectId != newTicket.ProjectId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Project", oldTicket.Project.Name, newTicket.Project.Name, updateDateTime, userId);
                changes.Add(changeRecord);
            }

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Priority", oldTicket.TicketPriority.Name, newTicket.TicketPriority.Name, updateDateTime, userId);
                changes.Add(changeRecord);
            }

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Status", oldTicket.TicketStatus.Name, newTicket.TicketStatus.Name, updateDateTime, userId);
                changes.Add(changeRecord);
            }

            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Type", oldTicket.TicketType.Name, newTicket.TicketType.Name, updateDateTime, userId);
                changes.Add(changeRecord);
            }

            if (oldTicket.OwnerId != newTicket.OwnerId)
            {
                var changeRecord = GetChangeRecord(newTicket.Id, "Owner", oldTicket.Owner.DisplayName, newTicket.Owner.DisplayName, updateDateTime, userId);
                changes.Add(changeRecord);
            }

            if (oldTicket.AssignedToId != newTicket.AssignedToId)
            {
                var oldAssignee = oldTicket.AssignedToId == null ? "Unassigned" : oldTicket.AssignedTo.DisplayName;
                var changeRecord = GetChangeRecord(newTicket.Id, "Assignee", oldAssignee, newTicket.AssignedTo.DisplayName, updateDateTime, userId);
                changes.Add(changeRecord);
            }

            return changes;
        }

        private TicketHistory GetChangeRecord(int ticketId, string propName, string oldValue, string newValue, DateTime updateDateTime, string changedByUserId)
        {
            return new TicketHistory
            {
                TicketId = ticketId,
                Property = propName,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedDateTime = updateDateTime,
                ChangedById = changedByUserId
            };
        }
    }
}
