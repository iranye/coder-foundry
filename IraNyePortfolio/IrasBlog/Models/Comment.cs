using System;
using System.ComponentModel.DataAnnotations;

namespace IrasBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public int BlogPostId { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [Display(Name = "Comment Body")]
        [StringLength(512)]
        public string CommentBody { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdateReason { get; set; }
        
        public virtual ApplicationUser Author { get; set; }

        public virtual BlogPost BlogPost { get; set; }
    }
}
