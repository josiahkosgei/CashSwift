
//BusinessObjects.Translations.SystemTextTranslation


using CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Translations
{
    [NavigationItem("Language")]
    [FriendlyKeyProperty("LanguageCode")]
    [DefaultProperty("ShortString")]
    [Persistent("xlns.sysTextTranslation")]
    public class SystemTextTranslation : XPLiteObject
    {
        private Guid fid;
        private SystemTextItem fSysTextItemID;
        private Language fLanguageCode;
        private string fTranslationSysText;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("SystemTextTranslationReferencesSystemTextItem")]
        public SystemTextItem SysTextItemID
        {
            get => fSysTextItemID;
            set => SetPropertyValue(nameof(SysTextItemID), ref fSysTextItemID, value);
        }

        [Indexed("SysTextItemID", Name = "UX_Translation_Language_Pair", Unique = true)]
        [Size(5)]
        [Association("SystemTextTranslationReferencesLanguage")]
        public Language LanguageCode
        {
            get => fLanguageCode;
            set => SetPropertyValue(nameof(LanguageCode), ref fLanguageCode, value);
        }

        [Size(-1)]
        public string TranslationSysText
        {
            get => fTranslationSysText;
            set => SetPropertyValue(nameof(TranslationSysText), ref fTranslationSysText, value);
        }

        public string ShortString => TranslationSysText?.Substring(0, Math.Min(TranslationSysText.Length, 50));

        public SystemTextTranslation(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
