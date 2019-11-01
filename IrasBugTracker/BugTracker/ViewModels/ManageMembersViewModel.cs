using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Models;

namespace BugTracker.ViewModels
{
    public class ManageMembersViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsMember { get; set; }
    }

    /// <summary>
    /// Used in the Manage Members Interface to add or remove users to or from a Project
    /// </summary>
    public class ProjectMembersViewModel
    {
        public Project Project { get; set; }
        public ICollection<ManageMembersViewModel> Members { get; set; } = new HashSet<ManageMembersViewModel>();
        public ICollection<ManageMembersViewModel> NonMembers { get; set; } = new HashSet<ManageMembersViewModel>();

        public ICollection<ManageMembersViewModel> AllUsers
        {
            get { return Members.Union(NonMembers).ToList(); }
        }

        public ProjectMembersViewModel()
        {}

        public ProjectMembersViewModel(Project project)
        {
            Project = project;
        }
    }
}
