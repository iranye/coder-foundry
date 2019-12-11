using System;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    /// <summary>
    /// Household Data Model
    /// </summary>
    public class Household
    {
        /// <summary>
        /// Household PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Household Name
        /// </summary>
        [Required]
        [StringLength(255, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Household Greeting
        /// </summary>
        [StringLength(255)]
        public string Greeting { get; set; }

        /// <summary>
        /// Household Creation Date
        /// </summary>
        [Required]
        public DateTime Created { get; set; }
    }
}
