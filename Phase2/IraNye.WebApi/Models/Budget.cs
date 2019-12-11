using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    /// <summary>
    /// Budget Data Model
    /// </summary>
    public class Budget
    {
        /// <summary>
        /// Budget PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key to Household
        /// </summary>
        public int HouseholdId { get; set; }

        /// <summary>
        /// Budget Name
        /// </summary>
        [StringLength(120, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Budget Description
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Budget Creation Date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Foreign Key to Budget Owner
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Budget Target Amount
        /// </summary>
        public decimal TargetAmount { get; set; }

        /// <summary>
        /// Budget Current Amount
        /// </summary>
        public decimal CurrentAmount { get; set; }
    }
}
