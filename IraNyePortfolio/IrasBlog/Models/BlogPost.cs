using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrasBlog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }

        [Display(Name="Blog Body")]
        public string BlogPostBody { get; set; }
        public string ImagePath { get; set; }
        public bool Published { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public BlogPost()
        {
            Comments = new HashSet<Comment>();
        }
    }
}
