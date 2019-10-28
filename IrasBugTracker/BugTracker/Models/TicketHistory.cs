using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangedDateTime { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string ChangedById { get; set; }

        // Navs
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser ChangedBy{ get; set; }
    }
}
