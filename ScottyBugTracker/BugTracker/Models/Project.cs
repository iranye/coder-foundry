using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        // Navs
        public virtual ICollection<Ticket> Tickets { get; set; }

        public Project()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}
