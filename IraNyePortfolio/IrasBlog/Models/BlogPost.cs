using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IrasBlog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public string Abstract { get; set; }

        [StringLength(255)]
        public string Slug { get; set; }

        [Required]
        [Display(Name="Blog Body")]
        [AllowHtml]
        public string BlogPostBody { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string BlogPostBodyPreview {
            get
            {
                if (!String.IsNullOrWhiteSpace(BlogPostBody))
                {
                    int defaultLength = 60;
                    int usableLength = defaultLength;
                    string ellipses = "...";
                    if (BlogPostBody.Length < defaultLength)
                    {
                        ellipses = "";
                        usableLength = BlogPostBody.Length;
                    }
                    return BlogPostBody.Substring(0, usableLength) + ellipses;
                }

                return String.Empty;
            }
        }

        public string ImagePath { get; set; }
        public bool Published { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public BlogPost()
        {
            Comments = new HashSet<Comment>();
        }
    }
}
