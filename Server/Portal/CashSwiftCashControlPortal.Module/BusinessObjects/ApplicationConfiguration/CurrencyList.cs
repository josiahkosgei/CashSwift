
//BusinessObjects.ApplicationConfiguration.CurrencyList


using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration
{
    [NavigationItem("Application Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    public class CurrencyList : XPLiteObject
    {
        private int fid;
        private string fname;
        private string fdescription;
        private Currency fdefault_currency;

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

        [Size(3)]
        [Association("CurrencyListReferencesCurrency")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Default Currency")]
        [Persistent("default_currency")]
        public Currency default_currency
        {
            get => fdefault_currency;
            set => SetPropertyValue(nameof(default_currency), ref fdefault_currency, value);
        }

        [Association("DeviceReferencesCurrencyList")]
        public XPCollection<Device> Devices => GetCollection<Device>(nameof(Devices));

        [Association("CurrencyList_CurrencyReferencesCurrencyList")]
        public XPCollection<CurrencyList_Currency> CurrencyList_Currencys => GetCollection<CurrencyList_Currency>(nameof(CurrencyList_Currencys));

        public CurrencyList(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
