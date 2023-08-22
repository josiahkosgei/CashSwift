
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TransactionType")]
    public partial class TransactionType
    {
        public TransactionType()
        {
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public Guid code { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public bool enabled { get; set; }

        // [InverseProperty("tx_typeNavigation")]
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}