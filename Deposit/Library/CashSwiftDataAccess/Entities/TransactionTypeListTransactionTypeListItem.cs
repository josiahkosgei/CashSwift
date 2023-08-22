
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TransactionTypeList_TransactionTypeListItem")]
    public partial class TransactionTypeListTransactionTypeListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int txtype_list_item { get; set; }
        public int txtype_list { get; set; }
        public int list_order { get; set; }

        [ForeignKey("txtype_list")]
        public virtual TransactionTypeList TransactionTypeList { get; set; }
        [ForeignKey("txtype_list_item")]
        public virtual TransactionTypeListItem TransactionTypeListItem { get; set; }
    }
}