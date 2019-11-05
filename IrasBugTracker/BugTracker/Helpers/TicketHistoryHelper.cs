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

        public void RecordHistoricalChanges(Ticket oldTicket, Ticket newTicket)
        {

        }
    }
}
