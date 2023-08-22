
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    /// <summary>
    /// Exceptions encountered during execution
    /// </summary>
    [Table("TransactionException", Schema = "exp")]
    public partial class TransactionException
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime datetime { get; set; }
        public Guid transaction_id { get; set; }
        public int code { get; set; }
        public int level { get; set; }
        [StringLength(255)]
        public string additional_info { get; set; }
        [StringLength(255)]
        public string message { get; set; }
        [Required]
        [StringLength(50)]
        public string machine_name { get; set; }
    }
}