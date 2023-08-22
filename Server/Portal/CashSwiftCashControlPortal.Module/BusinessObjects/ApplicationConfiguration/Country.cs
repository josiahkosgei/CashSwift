
//BusinessObjects.ApplicationConfiguration.Country


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration
{
    [FriendlyKeyProperty("country_code")]
    [DefaultProperty("country_name")]
    [VisibleInReports]
    [VisibleInDashboards]
    public class Country : XPLiteObject
    {
        private string fcountry_code;
        private string fcountry_name;

        [Key]
        [Size(2)]
        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Code")]
        public string country_code
        {
            get => fcountry_code;
            set => SetPropertyValue(nameof(country_code), ref fcountry_code, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The name was already registered within the system.")]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Name")]
        [Persistent("country_name")]
        public string country_name
        {
            get => fcountry_name;
            set => SetPropertyValue(nameof(country_name), ref fcountry_name, value);
        }

        [Association("BankReferencesCountry")]
        public XPCollection<Bank> Banks => GetCollection<Bank>(nameof(Banks));

        public Country(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
