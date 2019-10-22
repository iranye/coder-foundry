using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class ProjectStatus
    {
        public byte Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Status { get; set; }
    }
}
