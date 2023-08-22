using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Currency")]
    public partial class Currency
    {
        public Currency()
        {
            CITDenominations = new HashSet<CITDenomination>();
            CurrencyListCurrencies = new HashSet<CurrencyListCurrency>();
            CurrencyLists = new HashSet<CurrencyList>();
            DeviceCITSuspenseAccounts = new HashSet<DeviceCITSuspenseAccount>();
            DeviceSuspenseAccounts = new HashSet<DeviceSuspenseAccount>();
            TransactionLimitListItems = new HashSet<TransactionLimitListItem>();
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [StringLength(3)]
        // [Unicode(false)]
        public string code { get; set; }
        [Required]
        [StringLength(50)]
        // [Unicode(false)]
        public string name { get; set; }
        public int minor { get; set; }
        [Required]
        [StringLength(2)]
        // [Unicode(false)]
        public string flag { get; set; }
        public bool enabled { get; set; }
        [StringLength(3)]
        // [Unicode(false)]
        public string ISO_3_Numeric_Code { get; set; }

        // [InverseProperty("currency")]
        public virtual ICollection<CITDenomination> CITDenominations { get; set; }
        // [InverseProperty("currency_itemNavigation")]
        public virtual ICollection<CurrencyListCurrency> CurrencyListCurrencies { get; set; }
        // [InverseProperty("default_currencyNavigation")]
        public virtual ICollection<CurrencyList> CurrencyLists { get; set; }
        // [InverseProperty("currency_codeNavigation")]
        public virtual ICollection<DeviceCITSuspenseAccount> DeviceCITSuspenseAccounts { get; set; }
        // [InverseProperty("currency_codeNavigation")]
        public virtual ICollection<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }
        // [InverseProperty("currency_codeNavigation")]
        public virtual ICollection<TransactionLimitListItem> TransactionLimitListItems { get; set; }
        // [InverseProperty("default_account_currencyNavigation")]
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
        // [InverseProperty("tx_currencyNavigation")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}