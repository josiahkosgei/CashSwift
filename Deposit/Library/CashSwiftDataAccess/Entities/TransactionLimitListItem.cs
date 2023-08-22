
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TransactionLimitListItem")]
    public partial class TransactionLimitListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid transactionitemlist_id { get; set; }
        [Required]
        [StringLength(3)]
        // [Unicode(false)]
        public string currency_code { get; set; }
        public bool show_funds_source { get; set; }
        public Guid? show_funds_form { get; set; }
        public long funds_source_amount { get; set; }
        public bool prevent_overdeposit { get; set; }
        public long overdeposit_amount { get; set; }
        [Required]
        public bool? prevent_underdeposit { get; set; }
        public long underdeposit_amount { get; set; }
        public bool prevent_overcount { get; set; }
        public int overcount_amount { get; set; }

        [ForeignKey("currency_code")]
        public virtual Currency CurrencyNavigation { get; set; }
        [ForeignKey("transactionitemlist_id")]
        public virtual TransactionLimitList TransactionItemlist { get; set; }
    }
}