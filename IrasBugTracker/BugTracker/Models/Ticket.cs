using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [NotMapped]
        public string DisplayableId
        {
            get { return Id.ToString("000"); }
        }

        [Required]
        [StringLength(255, ErrorMessage = "Title must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        // FKs
        public int ProjectId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketTypeId { get; set; }

        public string OwnerId { get; set; }
        public string AssignedToId { get; set; }

        // Navs
        public virtual Project Project { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketType TicketType { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }

        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
        public virtual ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
        public virtual ICollection<TicketHistory> Events { get; set; } = new List<TicketHistory>();
        public virtual ICollection<TicketNotification> Notifications { get; set; } = new List<TicketNotification>();
    }
}
