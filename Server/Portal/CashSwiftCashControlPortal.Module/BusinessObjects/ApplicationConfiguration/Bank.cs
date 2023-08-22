
//BusinessObjects.ApplicationConfiguration.Bank


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration
{
    [NavigationItem("Application Management")]
    [VisibleInReports]
    [VisibleInDashboards]
    [FriendlyKeyProperty("country_code")]
    [DefaultProperty("name")]
    public class Bank : XPLiteObject
    {
        private Guid fid;
        private string fname;
        private string fdescription;
        private string fbank_code;
        private Country fcountry_code;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The name was already registered within the system.")]
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

        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "Value already exists")]
        [RuleRequiredField(DefaultContexts.Save)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Bank Code")]
        [Persistent("bank_code")]
        public string bank_code
        {
            get => fbank_code;
            set => SetPropertyValue(nameof(bank_code), ref fbank_code, value);
        }

        [Size(2)]
        [Association("BankReferencesCountry")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Country")]
        [Persistent("country_code")]
        public Country country_code
        {
            get => fcountry_code;
            set => SetPropertyValue(nameof(country_code), ref fcountry_code, value);
        }

        [Association("BranchReferencesBank")]
        public XPCollection<Branch> Branches => GetCollection<Branch>(nameof(Branches));

        public Bank(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
