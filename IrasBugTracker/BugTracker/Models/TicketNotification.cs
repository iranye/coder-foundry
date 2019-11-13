using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        [NotMapped]
        public string FormattedCreatedDateTime => Created.ToString("g");

        public bool IsRead { get; set; }

        [StringLength(255)]
        public string Subject { get; set; }
        
        [StringLength(255)] 
        public string NotificationBody { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string RecipientId { get; set; }

        // Navs
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}
