using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    /// <summary>
    /// TransactionType Class
    /// </summary>
    public class TransactionType
    {
        /// <summary>
        /// TransactionType PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Transaction Type
        /// </summary>
        [StringLength(80)]
        public string Type { get; set; }
    }
}
