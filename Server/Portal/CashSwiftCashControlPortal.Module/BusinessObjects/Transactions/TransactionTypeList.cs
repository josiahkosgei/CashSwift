
//BusinessObjects.Transactions.TransactionTypeList


using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [NavigationItem("Application Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [VisibleInReports]
    [VisibleInDashboards]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class TransactionTypeList : XPLiteObject
    {
        private int fid;
        private string fname;
        private string fdescription;
        private bool fenabled;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public int id
        {
            get => fid;
            set => SetPropertyValue<int>(nameof(id), ref fid, value);
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

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [Association("TransactionTypeList_TransactionTypeListItemReferencesTransactionTypeList")]
        public XPCollection<TransactionTypeList_TransactionTypeListItem> TransactionTypeList_TransactionTypeListItems => GetCollection<TransactionTypeList_TransactionTypeListItem>(nameof(TransactionTypeList_TransactionTypeListItems));

        [Association("DeviceReferencesTransactionTypeList")]
        public XPCollection<Device> Devices => GetCollection<Device>(nameof(Devices));

        public TransactionTypeList(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
