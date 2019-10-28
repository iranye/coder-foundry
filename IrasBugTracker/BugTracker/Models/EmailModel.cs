using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class EmailModel
    {
        [Required, Display(Name = "Name")]
        public string FromName { get; set; }

        [Required, Display(Name = "Email")]
        public string FromEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required, Display(Name = "Your Message")]
        public string Body { get; set; }
    }
}
