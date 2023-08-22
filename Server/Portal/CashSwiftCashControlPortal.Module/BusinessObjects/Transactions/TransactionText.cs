
//BusinessObjects.Transactions.TransactionText


using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [FriendlyKeyProperty("tx_item.name")]
    [DefaultProperty("tx_item.name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class TransactionText : XPLiteObject
    {
        private Guid fid;
        private TransactionTypeListItem ftx_item;
        private UserTextItem fdisclaimer;
        private UserTextItem fterms;
        private UserTextItem ffull_instructions;
        private UserTextItem flistItem_caption;
        private UserTextItem faccount_number_caption;
        private UserTextItem faccount_name_caption;
        private UserTextItem freference_account_number_caption;
        private UserTextItem freference_account_name_caption;
        private UserTextItem fnarration_caption;
        private UserTextItem falias_account_number_caption;
        private UserTextItem falias_account_name_caption;
        private UserTextItem fdepositor_name_caption;
        private UserTextItem fphone_number_caption;
        private UserTextItem fid_number_caption;
        private UserTextItem freceipt_template;
        private UserTextItem fFundsSource_caption;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed(Name = "UX_TransactionText_tx_item", Unique = true)]
        [Persistent("tx_item")]
        [Browsable(false)]
        public TransactionTypeListItem TransactionTypeListItem
        {
            get => ftx_item;
            set
            {
                if (ftx_item == value)
                    return;
                TransactionTypeListItem ftxItem = ftx_item;
                ftx_item = value;
                if (IsLoading)
                    return;
                if (ftxItem != null && ftxItem.TransactionText == this)
                    ftxItem.TransactionText =  null;
                if (ftx_item != null)
                    ftx_item.TransactionText = this;
                OnChanged(nameof(TransactionTypeListItem));
            }
        }

        [Association("TransactionText_DisclaimerReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.disclaimer'")]
        [Persistent("disclaimer")]
        public UserTextItem Disclaimer
        {
            get => fdisclaimer;
            set => SetPropertyValue(nameof(Disclaimer), ref fdisclaimer, value);
        }

        [Association("TransactionText_TermsAndConditionsReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.terms'")]
        [Persistent("terms")]
        public UserTextItem TermsAndConditions
        {
            get => fterms;
            set => SetPropertyValue(nameof(TermsAndConditions), ref fterms, value);
        }

        [Association("TransactionText_FullInstructionsReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.full_instructions'")]
        [Persistent("full_instructions")]
        public UserTextItem FullInstructions
        {
            get => ffull_instructions;
            set => SetPropertyValue(nameof(FullInstructions), ref ffull_instructions, value);
        }

        [Association("TransactionText_ListItemCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.listItem_caption'")]
        [Persistent("listItem_caption")]
        public UserTextItem ListItemCaption
        {
            get => flistItem_caption;
            set => SetPropertyValue(nameof(ListItemCaption), ref flistItem_caption, value);
        }

        [Association("TransactionText_AccountNumberCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.account_number_caption'")]
        [Persistent("account_number_caption")]
        public UserTextItem AccountNumberCaption
        {
            get => faccount_number_caption;
            set => SetPropertyValue(nameof(AccountNumberCaption), ref faccount_number_caption, value);
        }

        [Association("TransactionText_AccountNameCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.account_name_caption'")]
        [Persistent("account_name_caption")]
        public UserTextItem AccountNameCaption
        {
            get => faccount_name_caption;
            set => SetPropertyValue(nameof(AccountNameCaption), ref faccount_name_caption, value);
        }

        [Association("TransactionText_ReferenceAccountNumberCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.reference_account_number_caption'")]
        [Persistent("reference_account_number_caption")]
        public UserTextItem ReferenceAccountNumberCaption
        {
            get => freference_account_number_caption;
            set => SetPropertyValue(nameof(ReferenceAccountNumberCaption), ref freference_account_number_caption, value);
        }

        [Association("TransactionText_ReferenceAccountNameCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.reference_account_name_caption'")]
        [Persistent("reference_account_name_caption")]
        public UserTextItem ReferenceAccountNameCaption
        {
            get => freference_account_name_caption;
            set => SetPropertyValue(nameof(ReferenceAccountNameCaption), ref freference_account_name_caption, value);
        }

        [Association("TransactionText_NarrationCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.narration_caption'")]
        [Persistent("narration_caption")]
        public UserTextItem NarrationCaption
        {
            get => fnarration_caption;
            set => SetPropertyValue(nameof(NarrationCaption), ref fnarration_caption, value);
        }

        [Association("TransactionText_AliasAccountNumberCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.alias_account_number_caption'")]
        [Persistent("alias_account_number_caption")]
        public UserTextItem AliasAccountNumberCaption
        {
            get => falias_account_number_caption;
            set => SetPropertyValue(nameof(AliasAccountNumberCaption), ref falias_account_number_caption, value);
        }

        [Association("TransactionText_AliasAccountNameCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.alias_account_name_caption'")]
        [Persistent("alias_account_name_caption")]
        public UserTextItem AliasAccountNameCaption
        {
            get => falias_account_name_caption;
            set => SetPropertyValue(nameof(AliasAccountNameCaption), ref falias_account_name_caption, value);
        }

        [Association("TransactionText_DepositorNameCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.depositor_name_caption'")]
        [Persistent("depositor_name_caption")]
        public UserTextItem DepositorNameCaption
        {
            get => fdepositor_name_caption;
            set => SetPropertyValue(nameof(DepositorNameCaption), ref fdepositor_name_caption, value);
        }

        [Association("TransactionText_PhoneNumberCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.phone_number_caption'")]
        [Persistent("phone_number_caption")]
        public UserTextItem PhoneNumberCaption
        {
            get => fphone_number_caption;
            set => SetPropertyValue(nameof(PhoneNumberCaption), ref fphone_number_caption, value);
        }

        [Association("TransactionText_IDNumberCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.id_number_caption'")]
        [Persistent("id_number_caption")]
        public UserTextItem IDNumberCaption
        {
            get => fid_number_caption;
            set => SetPropertyValue(nameof(IDNumberCaption), ref fid_number_caption, value);
        }

        [Association("TransactionText_ReceiptTemplateReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.receipt_template'")]
        [Persistent("receipt_template")]
        public UserTextItem ReceiptTemplate
        {
            get => freceipt_template;
            set => SetPropertyValue(nameof(ReceiptTemplate), ref freceipt_template, value);
        }

        [Association("TransactionTextReferencesxlns_TextItemFundsSource_caption")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_TransactionText.FundsSource_caption'")]
        [Persistent("FundsSource_caption")]
        public UserTextItem FundsSource_caption
        {
            get => fFundsSource_caption;
            set => SetPropertyValue(nameof(FundsSource_caption), ref fFundsSource_caption, value);
        }

        public TransactionText(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
