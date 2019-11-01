using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Property { get; set; }

        [StringLength(255)]
        public string OldValue { get; set; }

        [StringLength(255)]
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
