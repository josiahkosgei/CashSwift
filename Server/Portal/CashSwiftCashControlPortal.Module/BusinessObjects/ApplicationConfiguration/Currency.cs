
//BusinessObjects.ApplicationConfiguration.Currency


using CashSwiftCashControlPortal.Module.BusinessObjects.CITs;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration
{
    [FriendlyKeyProperty("code")]
    [DefaultProperty("name")]
    public class Currency : XPLiteObject
    {
        private string fcode;
        private string fname;
        private int fminor;
        private string fflag;
        private bool fenabled;

        [Key]
        [Size(3)]
        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Code")]
        public string code
        {
            get => fcode;
            set => SetPropertyValue(nameof(code), ref fcode, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The name was already registered within the system.")]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Name")]
        [Persistent("name")]
        public string name
        {
            get => fname;
            set => SetPropertyValue(nameof(name), ref fname, value);
        }

        [DisplayName("Minor Unit")]
        [Persistent("minor")]
        public int minor
        {
            get => fminor;
            set => SetPropertyValue<int>(nameof(minor), ref fminor, value);
        }

        [Size(50)]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Flag")]
        [Persistent("flag")]
        public string flag
        {
            get => fflag;
            set => SetPropertyValue(nameof(flag), ref fflag, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [Association("CITDenominationsReferencesCurrency")]
        public XPCollection<CITDenominations> CITDenominationsCollection => GetCollection<CITDenominations>(nameof(CITDenominationsCollection));

        [Association("CurrencyListReferencesCurrency")]
        public XPCollection<CurrencyList> CurrencyListDefaults => GetCollection<CurrencyList>(nameof(CurrencyListDefaults));

        [Association("TransactionReferencesCurrency")]
        public XPCollection<Transaction> Transactions => GetCollection<Transaction>(nameof(Transactions));

        [Association("DeviceSuspenseAccountReferencesCurrency")]
        public XPCollection<DeviceSuspenseAccount> DeviceSuspenseAccounts => GetCollection<DeviceSuspenseAccount>(nameof(DeviceSuspenseAccounts));

        [Association("DeviceCITSuspenseAccountReferencesCurrency")]
        public XPCollection<DeviceCITSuspenseAccount> DeviceCITSuspenseAccounts => GetCollection<DeviceCITSuspenseAccount>(nameof(DeviceCITSuspenseAccounts));

        [Association("CurrencyList_CurrencyReferencesCurrency")]
        public XPCollection<CurrencyList_Currency> CurrencyList_Currencys => GetCollection<CurrencyList_Currency>(nameof(CurrencyList_Currencys));

        [Association("TransactionTypeListItemReferencesCurrency")]
        public XPCollection<TransactionTypeListItem> TransactionTypeListItems => GetCollection<TransactionTypeListItem>(nameof(TransactionTypeListItems));

        [Association("TransactionLimitListItemReferencesCurrency")]
        public XPCollection<TransactionLimitListItem> TransactionLimitListItems => GetCollection<TransactionLimitListItem>(nameof(TransactionLimitListItems));

        public Currency(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
