
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TransactionTypeListItem")]
    public partial class TransactionTypeListItem
    {
        public TransactionTypeListItem()
        {
            TransactionTypeListTransactionTypeListItems = new HashSet<TransactionTypeListTransactionTypeListItem>();
            Transactions = new HashSet<Transaction>();
            TransactionTexts = new HashSet<TransactionText>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public bool validate_reference_account { get; set; }
        [StringLength(50)]
        public string default_account { get; set; }
        [StringLength(50)]
        public string default_account_name { get; set; }
        [Required]
        [StringLength(3)]
        // [Unicode(false)]
        public string default_account_currency { get; set; }
        public bool validate_default_account { get; set; }
        [Required]
        public bool? enabled { get; set; }
        public int tx_type { get; set; }
        public int tx_type_guiscreenlist { get; set; }
        [StringLength(50)]
        public string cb_tx_type { get; set; }
        [StringLength(50)]
        public string username { get; set; }
        public string password { get; set; }
        public byte[] Icon { get; set; }
        public Guid? tx_limit_list { get; set; }
        public Guid? tx_text { get; set; }
        public Guid? account_permission { get; set; }
        public bool init_user_required { get; set; }
        public bool auth_user_required { get; set; }

        [ForeignKey("default_account_currency")]
        public virtual Currency DefaultAccountCurrency { get; set; }
        [ForeignKey("tx_limit_list")]
        public virtual TransactionLimitList TransactionLimitList { get; set; }
        [ForeignKey("tx_text")]
        public virtual TransactionText TransactionText { get; set; }
        [ForeignKey("tx_type")]
        public virtual TransactionType TransactionType { get; set; }
        [ForeignKey("tx_type_guiscreenlist")]
        public virtual GUIScreenList GUIScreenList { get; set; }
        public virtual ICollection<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }
        public virtual ICollection<TransactionText> TransactionTexts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}