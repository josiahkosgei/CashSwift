
//BusinessObjects.Translations.UserTextItem


using CashSwiftCashControlPortal.Module.BusinessObjects.Screens;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using CashSwiftCashControlPortal.Module.BusinessObjects.Validation;
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
    [FriendlyKeyProperty("ShortString")]
    [DefaultProperty("name")]
    [Persistent("xlns.TextItem")]
    public class UserTextItem : XPLiteObject
    {
        private Guid fid;
        private string fname;
        private string fdescription;
        private string fDefaultTranslation;
        private UserTextItemCategory fCategory;
        private UserTextItemType fTextItemTypeID;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
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

        [Association("UserTextItemReferencesUserTextItemCategory")]
        public UserTextItemCategory Category
        {
            get => fCategory;
            set => SetPropertyValue(nameof(Category), ref fCategory, value);
        }

        [Association("UserTextItemReferencesUserTextItemType")]
        public UserTextItemType TextItemTypeID
        {
            get => fTextItemTypeID;
            set => SetPropertyValue(nameof(TextItemTypeID), ref fTextItemTypeID, value);
        }

        public string ShortString => DefaultTranslation?.Substring(0, Math.Min(DefaultTranslation.Length, 100)) ?? name;

        [Browsable(false)]
        [Association("GUIScreenText_ScreenTitleReferencesUserTextItem")]
        public XPCollection<GUIScreenText> GUIScreenText_ScreenTitles => GetCollection<GUIScreenText>(nameof(GUIScreenText_ScreenTitles));

        [Browsable(false)]
        [Association("GUIScreenText_BackButtonCaptionReferencesUserTextItem")]
        public XPCollection<GUIScreenText> GUIScreenText_BackButtonCaptions => GetCollection<GUIScreenText>(nameof(GUIScreenText_BackButtonCaptions));

        [Browsable(false)]
        [Association("GUIScreenText_CancelButtonCaptionReferencesUserTextItem")]
        public XPCollection<GUIScreenText> GUIScreenText_CancelButtonCaptions => GetCollection<GUIScreenText>(nameof(GUIScreenText_CancelButtonCaptions));

        [Browsable(false)]
        [Association("GUIScreenText_FullInstructionsReferencesUserTextItem")]
        public XPCollection<GUIScreenText> GUIScreenText_FullInstructions => GetCollection<GUIScreenText>(nameof(GUIScreenText_FullInstructions));

        [Browsable(false)]
        [Association("GUIScreenText_AcceptButtonCaptionReferencesUserTextItem")]
        public XPCollection<GUIScreenText> GUIScreenText_AcceptButtonCaptions => GetCollection<GUIScreenText>(nameof(GUIScreenText_AcceptButtonCaptions));

        [Browsable(false)]
        [Association("GUIScreenText_ScreenTitleInstructionReferencesUserTextItem")]
        public XPCollection<GUIScreenText> GUIScreenText_ScreenTitleInstructions => GetCollection<GUIScreenText>(nameof(GUIScreenText_ScreenTitleInstructions));

        [Browsable(false)]
        [Association("TransactionText_AccountNameCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_AccountNameCaptions => GetCollection<TransactionText>(nameof(TransactionText_AccountNameCaptions));

        [Browsable(false)]
        [Association("TransactionText_AccountNumberCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_AccountNumberCaptions => GetCollection<TransactionText>(nameof(TransactionText_AccountNumberCaptions));

        [Browsable(false)]
        [Association("TransactionText_AliasAccountNameCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_AliasAccountNameCaptions => GetCollection<TransactionText>(nameof(TransactionText_AliasAccountNameCaptions));

        [Browsable(false)]
        [Association("TransactionText_AliasAccountNumberCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_AliasAccountNumberCaptions => GetCollection<TransactionText>(nameof(TransactionText_AliasAccountNumberCaptions));

        [Browsable(false)]
        [Association("TransactionText_DepositorNameCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_DepositorNameCaptions => GetCollection<TransactionText>(nameof(TransactionText_DepositorNameCaptions));

        [Browsable(false)]
        [Association("TransactionText_DisclaimerReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_Disclaimers => GetCollection<TransactionText>(nameof(TransactionText_Disclaimers));

        [Browsable(false)]
        [Association("TransactionText_IDNumberCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_IDNumberCaptions => GetCollection<TransactionText>(nameof(TransactionText_IDNumberCaptions));

        [Browsable(false)]
        [Association("TransactionText_ListItemCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_ListItemCaptions => GetCollection<TransactionText>(nameof(TransactionText_ListItemCaptions));

        [Browsable(false)]
        [Association("TransactionText_FullInstructionsReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_FullInstructions => GetCollection<TransactionText>(nameof(TransactionText_FullInstructions));

        [Browsable(false)]
        [Association("TransactionText_NarrationCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_NarrationCaptions => GetCollection<TransactionText>(nameof(TransactionText_NarrationCaptions));

        [Browsable(false)]
        [Association("TransactionText_PhoneNumberCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_PhoneNumberCaptions => GetCollection<TransactionText>(nameof(TransactionText_PhoneNumberCaptions));

        [Browsable(false)]
        [Association("TransactionText_ReceiptTemplateReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_ReceiptTemplates => GetCollection<TransactionText>(nameof(TransactionText_ReceiptTemplates));

        [Browsable(false)]
        [Association("TransactionText_ReferenceAccountNameCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_ReferenceAccountNameCaptions => GetCollection<TransactionText>(nameof(TransactionText_ReferenceAccountNameCaptions));

        [Browsable(false)]
        [Association("TransactionText_ReferenceAccountNumberCaptionReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_ReferenceAccountNumberCaptions => GetCollection<TransactionText>(nameof(TransactionText_ReferenceAccountNumberCaptions));

        [Browsable(false)]
        [Association("TransactionText_TermsAndConditionsReferencesUserTextItem")]
        public XPCollection<TransactionText> TransactionText_TermsAndConditions => GetCollection<TransactionText>(nameof(TransactionText_TermsAndConditions));

        [Association("TransactionTextReferencesxlns_TextItemFundsSource_caption")]
        [Browsable(false)]
        public XPCollection<TransactionText> TransactionText_FundsSource_caption => GetCollection<TransactionText>(nameof(TransactionText_FundsSource_caption));

        [Browsable(false)]
        [Association("ValidationText_ErrorMessageReferencesUserTextItem")]
        public XPCollection<ValidationText> ValidationText_ErrorMessages => GetCollection<ValidationText>(nameof(ValidationText_ErrorMessages));

        [Browsable(false)]
        [Association("ValidationText_SuccessMessageReferencesUserTextItem")]
        public XPCollection<ValidationText> ValidationText_SuccessMessages => GetCollection<ValidationText>(nameof(ValidationText_SuccessMessages));

        [Browsable(false)]
        [Association("GUIPrepopItem_ValueReferencesUserTextItem")]
        public XPCollection<GUIPrepopItem> GUIPrepopItem_Value => GetCollection<GUIPrepopItem>(nameof(GUIPrepopItem_Value));

        [Association("UserTextTranslationReferencesUserTextItem")]
        public XPCollection<UserTextTranslation> UserTextTranslations => GetCollection<UserTextTranslation>(nameof(UserTextTranslations));

        public UserTextItem(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
