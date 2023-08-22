
//BusinessObjects.ApplicationConfiguration.Language


using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration
{
    [NavigationItem("Application Management")]
    [FriendlyKeyProperty("code")]
    [DefaultProperty("name")]
    public class Language : XPLiteObject
    {
        private string fcode;
        private string fname;
        private string fflag;
        private bool fenabled;

        [Key]
        [Size(5)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
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

        [Size(50)]
        [DisplayName("Flag")]
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

        [Association("DepositorSessionReferencesLanguage")]
        public XPCollection<DepositorSession> DepositorSessions => GetCollection<DepositorSession>(nameof(DepositorSessions));

        [Association("LanguageListReferencesLanguage")]
        public XPCollection<LanguageList> LanguageLists => GetCollection<LanguageList>(nameof(LanguageLists));

        [Association("LanguageList_LanguageReferencesLanguage")]
        public XPCollection<LanguageList_Language> LanguageList_Languages => GetCollection<LanguageList_Language>(nameof(LanguageList_Languages));

        [Association("SystemTextTranslationReferencesLanguage")]
        public XPCollection<SystemTextTranslation> SystemTextTranslations => GetCollection<SystemTextTranslation>(nameof(SystemTextTranslations));

        [Association("UserTextTranslationReferencesLanguage")]
        public XPCollection<UserTextTranslation> UserTextTranslations => GetCollection<UserTextTranslation>(nameof(UserTextTranslations));

        public Language(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
