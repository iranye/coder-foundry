using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public string AssigneeId { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public int StatusId { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }

        //TODO: Need Ticket Type

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
