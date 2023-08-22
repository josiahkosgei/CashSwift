using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TransactionTypeList")]
    public partial class TransactionTypeList
    {
        public TransactionTypeList()
        {
            Devices = new HashSet<Device>();
            TransactionTypeListTransactionTypeListItems = new HashSet<TransactionTypeListTransactionTypeListItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public bool enabled { get; set; }

        // [InverseProperty("transaction_type_listNavigation")]
        public virtual ICollection<Device> Devices { get; set; }
        // [InverseProperty("txtype_listNavigation")]
        public virtual ICollection<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }
    }
}