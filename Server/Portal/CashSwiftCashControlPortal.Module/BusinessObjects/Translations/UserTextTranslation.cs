
//BusinessObjects.Translations.UserTextTranslation


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
    [Persistent("xlns.TextTranslation")]
    public class UserTextTranslation : XPLiteObject
    {
        private Guid fid;
        private UserTextItem fTextItemID;
        private Language fLanguageCode;
        private string fTranslationText;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("UserTextTranslationReferencesUserTextItem")]
        public UserTextItem TextItemID
        {
            get => fTextItemID;
            set => SetPropertyValue(nameof(TextItemID), ref fTextItemID, value);
        }

        [Indexed("TextItemID", Name = "UX_UI_Translation_Language_Pair", Unique = true)]
        [Size(5)]
        [Association("UserTextTranslationReferencesLanguage")]
        public Language LanguageCode
        {
            get => fLanguageCode;
            set => SetPropertyValue(nameof(LanguageCode), ref fLanguageCode, value);
        }

        [Size(-1)]
        public string TranslationText
        {
            get => fTranslationText;
            set => SetPropertyValue(nameof(TranslationText), ref fTranslationText, value);
        }

        public string ShortString => TranslationText?.Substring(0, Math.Min(TranslationText.Length, 50));

        public UserTextTranslation(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
