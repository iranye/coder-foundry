using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string UserNotifiedId { get; set; }

        // Navs
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser UserNotified { get; set; }
    }
}
