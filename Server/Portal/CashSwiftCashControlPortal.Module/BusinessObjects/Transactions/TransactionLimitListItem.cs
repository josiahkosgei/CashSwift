
//BusinessObjects.Transactions.TransactionLimitListItem


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [NavigationItem("Application Management")]
    [FriendlyKeyProperty("currency_code")]
    [DefaultProperty("TransactionLimitList")]
    public class TransactionLimitListItem : XPLiteObject
    {
        private Guid fid;
        private TransactionLimitList ftransactionitemlist_id;
        private ApplicationConfiguration.Currency fcurrency_code;
        private bool fshow_funds_source;
        private Guid fshow_funds_form;
        private long ffunds_source_amount;
        private bool fprevent_overdeposit;
        private long foverdeposit_amount;
        private bool fprevent_underdeposit;
        private long funderdeposit_amount;
        private bool fprevent_overcount;
        private int fovercount_amount;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed("currency_code", Name = "UX_TransactionLimitListItem", Unique = true)]
        [Association("TransactionLimitListItemReferencesTransactionLimitList")]
        [Persistent("transactionitemlist_id")]
        public TransactionLimitList TransactionLimitList
        {
            get => ftransactionitemlist_id;
            set => SetPropertyValue(nameof(TransactionLimitList), ref ftransactionitemlist_id, value);
        }

        [Size(3)]
        [Association("TransactionLimitListItemReferencesCurrency")]
        public ApplicationConfiguration.Currency currency_code
        {
            get => fcurrency_code;
            set => SetPropertyValue(nameof(currency_code), ref fcurrency_code, value);
        }

        public bool show_funds_source
        {
            get => fshow_funds_source;
            set => SetPropertyValue(nameof(show_funds_source), ref fshow_funds_source, value);
        }

        public Guid show_funds_form
        {
            get => fshow_funds_form;
            set => SetPropertyValue(nameof(show_funds_form), ref fshow_funds_form, value);
        }

        public long funds_source_amount
        {
            get => ffunds_source_amount;
            set => SetPropertyValue(nameof(funds_source_amount), ref ffunds_source_amount, value);
        }

        public bool prevent_overdeposit
        {
            get => fprevent_overdeposit;
            set => SetPropertyValue(nameof(prevent_overdeposit), ref fprevent_overdeposit, value);
        }

        public long overdeposit_amount
        {
            get => foverdeposit_amount;
            set => SetPropertyValue(nameof(overdeposit_amount), ref foverdeposit_amount, value);
        }

        public bool prevent_underdeposit
        {
            get => fprevent_underdeposit;
            set => SetPropertyValue(nameof(prevent_underdeposit), ref fprevent_underdeposit, value);
        }

        public long underdeposit_amount
        {
            get => funderdeposit_amount;
            set => SetPropertyValue(nameof(underdeposit_amount), ref funderdeposit_amount, value);
        }

        public bool prevent_overcount
        {
            get => fprevent_overcount;
            set => SetPropertyValue(nameof(prevent_overcount), ref fprevent_overcount, value);
        }

        [RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "overdeposit_amount", ParametersMode.Expression, CustomMessageTemplate = "overcount_amount must be greater than or equal to overdeposit_amount when prevent_overcount and prevent_overdeposit is true", Name = "OverCount gt OverAmount", SkipNullOrEmptyValues = true, TargetCriteria = "[prevent_overcount]&&[prevent_overdeposit]")]
        public int overcount_amount
        {
            get => fovercount_amount;
            set => SetPropertyValue<int>(nameof(overcount_amount), ref fovercount_amount, value);
        }

        public TransactionLimitListItem(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
