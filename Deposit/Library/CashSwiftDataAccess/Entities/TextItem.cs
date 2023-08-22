
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TextItem", Schema = "xlns")]
    public partial class TextItem
    {
        public TextItem()
        {
            GUIPrepopItems = new HashSet<GUIPrepopItem>();
            GUIScreenTextbtn_accept_captionNavigation = new HashSet<GUIScreenText>();
            GUIScreenTextbtn_back_captionNavigation = new HashSet<GUIScreenText>();
            GUIScreenTextbtn_cancel_captionNavigation = new HashSet<GUIScreenText>();
            GUIScreenTextfull_instructionsNavigation = new HashSet<GUIScreenText>();
            GUIScreenTextscreen_titleNavigation = new HashSet<GUIScreenText>();
            GUIScreenTextscreen_title_instructionNavigation = new HashSet<GUIScreenText>();
            TextTranslations = new HashSet<TextTranslation>();
            TransactionTextFundsSource_captionNavigation = new HashSet<TransactionText>();
            TransactionTextaccount_name_captionNavigation = new HashSet<TransactionText>();
            TransactionTextaccount_number_captionNavigation = new HashSet<TransactionText>();
            TransactionTextalias_account_name_captionNavigation = new HashSet<TransactionText>();
            TransactionTextalias_account_number_captionNavigation = new HashSet<TransactionText>();
            TransactionTextdepositor_name_captionNavigation = new HashSet<TransactionText>();
            TransactionTextdisclaimerNavigations = new HashSet<TransactionText>();
            TransactionTextfull_instructionsNavigation = new HashSet<TransactionText>();
            TransactionTextid_number_captionNavigation = new HashSet<TransactionText>();
            TransactionTextlistItem_captionNavigation = new HashSet<TransactionText>();
            TransactionTextnarration_captionNavigation = new HashSet<TransactionText>();
            TransactionTextphone_number_captionNavigation = new HashSet<TransactionText>();
            TransactionTextreceipt_templateNavigation = new HashSet<TransactionText>();
            TransactionTextreference_account_name_captionNavigation = new HashSet<TransactionText>();
            TransactionTextreference_account_number_captionNavigation = new HashSet<TransactionText>();
            TransactionTexttermsNavigations = new HashSet<TransactionText>();
            ValidationTexterror_messageNavigation = new HashSet<ValidationText>();
            ValidationTextsuccess_messageNavigation = new HashSet<ValidationText>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public string DefaultTranslation { get; set; }
        public Guid Category { get; set; }
        public Guid? TextItemTypeID { get; set; }

        [ForeignKey("Category")]
        // [InverseProperty("TextItems")]
        public virtual TextItemCategory CategoryNavigation { get; set; }
        [ForeignKey("TextItemTypeID")]
        // [InverseProperty("TextItems")]
        public virtual TextItemType TextItemType { get; set; }
        // [InverseProperty("valueNavigation")]
        public virtual ICollection<GUIPrepopItem> GUIPrepopItems { get; set; }
        // [InverseProperty("btn_accept_captionNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextbtn_accept_captionNavigation { get; set; }
        // [InverseProperty("btn_back_captionNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextbtn_back_captionNavigation { get; set; }
        // [InverseProperty("btn_cancel_captionNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextbtn_cancel_captionNavigation { get; set; }
        // [InverseProperty("full_instructionsNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextfull_instructionsNavigation { get; set; }
        // [InverseProperty("screen_titleNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextscreen_titleNavigation { get; set; }
        // [InverseProperty("screen_title_instructionNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextscreen_title_instructionNavigation { get; set; }
        // [InverseProperty("TextItem")]
        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
        // [InverseProperty("FundsSource_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextFundsSource_captionNavigation { get; set; }
        // [InverseProperty("account_name_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextaccount_name_captionNavigation { get; set; }
        // [InverseProperty("account_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextaccount_number_captionNavigation { get; set; }
        // [InverseProperty("alias_account_name_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextalias_account_name_captionNavigation { get; set; }
        // [InverseProperty("alias_account_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextalias_account_number_captionNavigation { get; set; }
        // [InverseProperty("depositor_name_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextdepositor_name_captionNavigation { get; set; }
        // [InverseProperty("disclaimerNavigation")]
        public virtual ICollection<TransactionText> TransactionTextdisclaimerNavigations { get; set; }
        // [InverseProperty("full_instructionsNavigation")]
        public virtual ICollection<TransactionText> TransactionTextfull_instructionsNavigation { get; set; }
        // [InverseProperty("id_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextid_number_captionNavigation { get; set; }
        // [InverseProperty("listItem_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextlistItem_captionNavigation { get; set; }
        // [InverseProperty("narration_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextnarration_captionNavigation { get; set; }
        // [InverseProperty("phone_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextphone_number_captionNavigation { get; set; }
        // [InverseProperty("receipt_templateNavigation")]
        public virtual ICollection<TransactionText> TransactionTextreceipt_templateNavigation { get; set; }
        // [InverseProperty("reference_account_name_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextreference_account_name_captionNavigation { get; set; }
        // [InverseProperty("reference_account_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextreference_account_number_captionNavigation { get; set; }
        // [InverseProperty("termsNavigation")]
        public virtual ICollection<TransactionText> TransactionTexttermsNavigations { get; set; }
        // [InverseProperty("error_messageNavigation")]
        public virtual ICollection<ValidationText> ValidationTexterror_messageNavigation { get; set; }
        // [InverseProperty("success_messageNavigation")]
        public virtual ICollection<ValidationText> ValidationTextsuccess_messageNavigation { get; set; }
    }
}