using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Project Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        [DisplayName("Project Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        // Navs
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }

        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Members = new HashSet<ApplicationUser>();
        }
    }
}
