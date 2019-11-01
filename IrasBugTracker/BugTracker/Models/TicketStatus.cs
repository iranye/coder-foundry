﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }

        [DisplayName("Status")]
        public string Name { get; set; }

        // Navs
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
