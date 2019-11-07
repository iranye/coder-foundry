using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public string MediaPath { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime CreatedDateTime { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string CreatedById { get; set; }

        // Navs
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
    }
}
