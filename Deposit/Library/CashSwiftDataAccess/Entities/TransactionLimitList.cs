
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CashSwiftDataAccess.Entities
{
    [Table("TransactionLimitList")]
    public partial class TransactionLimitList
    {
        public TransactionLimitList()
        {
            TransactionLimitListItems = new HashSet<TransactionLimitListItem>();
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }

        public virtual ICollection<TransactionLimitListItem> TransactionLimitListItems { get; set; }
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
        public bool Get_prevent_overdeposit(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.CurrencyNavigation == currency);
            return transactionLimitListItem != null && transactionLimitListItem.prevent_overdeposit;
        }

        public long Get_overdeposit_amount(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.CurrencyNavigation == currency);
            return transactionLimitListItem == null ? 0L : transactionLimitListItem.overdeposit_amount;
        }

        public bool Get_prevent_underdeposit(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.CurrencyNavigation == currency);
            return transactionLimitListItem != null && (bool)transactionLimitListItem.prevent_underdeposit;
        }

        public long Get_underdeposit_amount(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.CurrencyNavigation == currency);
            return transactionLimitListItem == null ? 0L : transactionLimitListItem.underdeposit_amount;
        }

        public bool Get_prevent_overcount(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.CurrencyNavigation == currency);
            return transactionLimitListItem != null && transactionLimitListItem.prevent_overcount;
        }

        public long Get_overcount_amount(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.CurrencyNavigation == currency);
            return transactionLimitListItem != null ? transactionLimitListItem.overcount_amount : 0L;
        }

    }
}