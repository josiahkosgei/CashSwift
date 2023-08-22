
//BusinessObjects.Translations.SystemTextItem


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Translations
{
    [NavigationItem("Language")]
    [FriendlyKeyProperty("name")]
    [DefaultProperty("ShortString")]
    [Persistent("xlns.sysTextItem")]
    public class SystemTextItem : XPLiteObject
    {
        private Guid fid;
        private string fToken;
        private string fname;
        private string fdescription;
        private string fDefaultTranslation;
        private SystemTextItemCategory fCategory;
        private SystemTextItemType fTextItemTypeID;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed(Name = "UX_SysTextItem_name", Unique = true)]
        [Size(50)]
        public string Token
        {
            get => fToken;
            set => SetPropertyValue(nameof(Token), ref fToken, value);
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

        [Size(-1)]
        public string DefaultTranslation
        {
            get => fDefaultTranslation;
            set => SetPropertyValue(nameof(DefaultTranslation), ref fDefaultTranslation, value);
        }

        [Association("SystemTextItemReferencesSystemTextItemCategory")]
        public SystemTextItemCategory Category
        {
            get => fCategory;
            set => SetPropertyValue(nameof(Category), ref fCategory, value);
        }

        [Association("SystemTextItemReferencesSystemTextItemType")]
        public SystemTextItemType TextItemTypeID
        {
            get => fTextItemTypeID;
            set => SetPropertyValue(nameof(TextItemTypeID), ref fTextItemTypeID, value);
        }

        public string ShortString => DefaultTranslation.Substring(0, Math.Min(DefaultTranslation.Length, 50));

        [Association("SystemTextTranslationReferencesSystemTextItem")]
        public XPCollection<SystemTextTranslation> SystemTextTranslations => GetCollection<SystemTextTranslation>(nameof(SystemTextTranslations));

        public SystemTextItem(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
