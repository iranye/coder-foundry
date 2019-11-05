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
        private ApplicationDbContext _db = new ApplicationDbContext();

        public void RecordHistoricalChanges(string userId, Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var updateDateTime = newTicket.Updated.GetValueOrDefault();
                if (updateDateTime == DateTime.MinValue)
                {
                    updateDateTime = DateTime.Now;
                }

                var record = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "TicketStatus",
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name,
                    ChangedDateTime = updateDateTime,
                    ChangedById = userId
                };
                _db.TicketHistorys.Add(record);
                _db.SaveChanges();
            }
        }
    }
}
