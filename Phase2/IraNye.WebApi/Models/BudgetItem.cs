using System;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    /// <summary>
    /// BudgetItem Data Model
    /// </summary>
    public class BudgetItem
    {
        /// <summary>
        /// BudgetItem PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key to Budget
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// BudgetItem Name
        /// </summary>
        [StringLength(100, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        /// <summary>
        /// BudgetItem Description
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// BudgetItem Creation Date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// BudgetItem Target Amount
        /// </summary>
        public decimal TargetAmount { get; set; }

        /// <summary>
        /// BudgetItem Current Amount
        /// </summary>
        public decimal CurrentAmount { get; set; }
    }
}
