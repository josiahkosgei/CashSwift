
//BusinessObjects.ApplicationConfiguration.Config


using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
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
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class Config : XPLiteObject
    {
        private string fname;
        private string fdescription;
        private string fdefault_value;
        private ConfigCategory fcategory_id;

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The name was already registered within the system.")]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Name")]
        [Persistent("name")]
        [Key]
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

        [Size(-1)]
        [DisplayName("Default Value")]
        public string default_value
        {
            get => fdefault_value;
            set => SetPropertyValue(nameof(default_value), ref fdefault_value, value);
        }

        [Association("ConfigReferencesConfigCategory")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Category")]
        [Persistent("category_id")]
        public ConfigCategory category_id
        {
            get => fcategory_id;
            set => SetPropertyValue(nameof(category_id), ref fcategory_id, value);
        }

        [Association("DeviceConfigReferencesConfig")]
        public XPCollection<DeviceConfig> DeviceConfigs => GetCollection<DeviceConfig>(nameof(DeviceConfigs));

        public Config(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
