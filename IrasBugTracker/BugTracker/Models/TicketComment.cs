using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        [Required]
        public string CommentBody { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string AuthorId { get; set; }

        // Navs
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}
