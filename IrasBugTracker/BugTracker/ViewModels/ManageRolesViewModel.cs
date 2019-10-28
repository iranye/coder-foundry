using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.ViewModels
{
    public class ManageRolesViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
