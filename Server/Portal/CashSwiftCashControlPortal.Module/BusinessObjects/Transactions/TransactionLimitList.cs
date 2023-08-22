
//BusinessObjects.Transactions.TransactionLimitList


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
    [FriendlyKeyProperty("name")]
    [DefaultProperty("name")]
    public class TransactionLimitList : XPLiteObject
    {
        private Guid fid;
        private string fname;
        private string fdescription;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
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

        [Size(255)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [DisplayName("Description")]
        public string description
        {
            get => fdescription;
            set => SetPropertyValue(nameof(description), ref fdescription, value);
        }

        [Association("TransactionLimitListItemReferencesTransactionLimitList")]
        public XPCollection<TransactionLimitListItem> TransactionLimitListItems => GetCollection<TransactionLimitListItem>(nameof(TransactionLimitListItems));

        [Association("TransactionTypeListItemReferencesTransactionLimitList")]
        public XPCollection<TransactionTypeListItem> TransactionTypeListItems => GetCollection<TransactionTypeListItem>(nameof(TransactionTypeListItems));

        public TransactionLimitList(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
