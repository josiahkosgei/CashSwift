
//BusinessObjects.ApplicationConfiguration.LanguageList_Language


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration
{
    [FriendlyKeyProperty("language_item.name")]
    [DefaultProperty("language_list.name")]
    public class LanguageList_Language : XPLiteObject
    {
        private Guid fid;
        private LanguageList flanguage_list;
        private Language flanguage_item;
        private int flanguage_order;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("LanguageList_LanguageReferencesLanguageList")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Language List")]
        [Persistent("language_list")]
        public LanguageList language_list
        {
            get => flanguage_list;
            set => SetPropertyValue(nameof(language_list), ref flanguage_list, value);
        }

        [Indexed("language_list", Name = "UX_LanguageList_Language_LanguageItem", Unique = true)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Size(5)]
        [DisplayName("Language Item")]
        [Persistent("language_item")]
        [Association("LanguageList_LanguageReferencesLanguage")]
        public Language language_item
        {
            get => flanguage_item;
            set => SetPropertyValue(nameof(language_item), ref flanguage_item, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Order")]
        [Persistent("language_order")]
        public int language_order
        {
            get => flanguage_order;
            set => SetPropertyValue<int>(nameof(language_order), ref flanguage_order, value);
        }

        public LanguageList_Language(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
