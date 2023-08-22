
//BusinessObjects.ApplicationConfiguration.LanguageList


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
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class LanguageList : XPLiteObject
    {
        private int fid;
        private string fname;
        private string fdescription;
        private bool fenabled;
        private Language fdefault_language;

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

        [RuleRequiredField(DefaultContexts.Save)]
        [Size(5)]
        [DisplayName("Default Language")]
        [Persistent("default_language")]
        [Association("LanguageListReferencesLanguage")]
        public Language default_language
        {
            get => fdefault_language;
            set => SetPropertyValue(nameof(default_language), ref fdefault_language, value);
        }

        [Association("LanguageList_LanguageReferencesLanguageList")]
        public XPCollection<LanguageList_Language> LanguageList_Languages => GetCollection<LanguageList_Language>(nameof(LanguageList_Languages));

        [Association("DeviceReferencesLanguageList")]
        public XPCollection<Device> Devices => GetCollection<Device>(nameof(Devices));

        public LanguageList(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
