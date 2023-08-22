﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("TextItem", Schema = "xlns")]
    [Index("Name", "Category", Name = "UX_UI_TextItem_name", IsUnique = true)]
    [Index("Category", Name = "iCategory_xlns_TextItem_CDE8C5ED")]
    [Index("TextItemTypeID", Name = "iTextItemTypeID_xlns_TextItem_2A2E0516")]
    public partial class TextItem
    {
        public TextItem()
        {
            GUIPrepopItems = new HashSet<GUIPrepopItem>();
            GUIScreenTextbtn_accept_captionNavigations = new HashSet<GUIScreenText>();
            GUIScreenTextbtn_back_captionNavigations = new HashSet<GUIScreenText>();
            GUIScreenTextbtn_cancel_captionNavigations = new HashSet<GUIScreenText>();
            GUIScreenTextfull_instructionsNavigations = new HashSet<GUIScreenText>();
            GUIScreenTextscreen_titleNavigations = new HashSet<GUIScreenText>();
            GUIScreenTextscreen_title_instructionNavigations = new HashSet<GUIScreenText>();
            TextTranslations = new HashSet<TextTranslation>();
            TransactionTextFundsSource_captionNavigations = new HashSet<TransactionText>();
            TransactionTextaccount_name_captionNavigations = new HashSet<TransactionText>();
            TransactionTextaccount_number_captionNavigations = new HashSet<TransactionText>();
            TransactionTextalias_account_name_captionNavigations = new HashSet<TransactionText>();
            TransactionTextalias_account_number_captionNavigations = new HashSet<TransactionText>();
            TransactionTextdepositor_name_captionNavigations = new HashSet<TransactionText>();
            TransactionTextdisclaimerNavigations = new HashSet<TransactionText>();
            TransactionTextfull_instructionsNavigations = new HashSet<TransactionText>();
            TransactionTextid_number_captionNavigations = new HashSet<TransactionText>();
            TransactionTextlistItem_captionNavigations = new HashSet<TransactionText>();
            TransactionTextnarration_captionNavigations = new HashSet<TransactionText>();
            TransactionTextphone_number_captionNavigations = new HashSet<TransactionText>();
            TransactionTextreceipt_templateNavigations = new HashSet<TransactionText>();
            TransactionTextreference_account_name_captionNavigations = new HashSet<TransactionText>();
            TransactionTextreference_account_number_captionNavigations = new HashSet<TransactionText>();
            TransactionTexttermsNavigations = new HashSet<TransactionText>();
            ValidationTexterror_messageNavigations = new HashSet<ValidationText>();
            ValidationTextsuccess_messageNavigations = new HashSet<ValidationText>();
        }

        [Key]
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
        [InverseProperty("TextItems")]
        public virtual TextItemCategory CategoryNavigation { get; set; }
        [ForeignKey("TextItemTypeID")]
        [InverseProperty("TextItems")]
        public virtual TextItemType TextItemType { get; set; }
        [InverseProperty("valueNavigation")]
        public virtual ICollection<GUIPrepopItem> GUIPrepopItems { get; set; }
        [InverseProperty("btn_accept_captionNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextbtn_accept_captionNavigations { get; set; }
        [InverseProperty("btn_back_captionNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextbtn_back_captionNavigations { get; set; }
        [InverseProperty("btn_cancel_captionNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextbtn_cancel_captionNavigations { get; set; }
        [InverseProperty("full_instructionsNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextfull_instructionsNavigations { get; set; }
        [InverseProperty("screen_titleNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextscreen_titleNavigations { get; set; }
        [InverseProperty("screen_title_instructionNavigation")]
        public virtual ICollection<GUIScreenText> GUIScreenTextscreen_title_instructionNavigations { get; set; }
        [InverseProperty("TextItem")]
        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
        [InverseProperty("FundsSource_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextFundsSource_captionNavigations { get; set; }
        [InverseProperty("account_name_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextaccount_name_captionNavigations { get; set; }
        [InverseProperty("account_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextaccount_number_captionNavigations { get; set; }
        [InverseProperty("alias_account_name_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextalias_account_name_captionNavigations { get; set; }
        [InverseProperty("alias_account_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextalias_account_number_captionNavigations { get; set; }
        [InverseProperty("depositor_name_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextdepositor_name_captionNavigations { get; set; }
        [InverseProperty("disclaimerNavigation")]
        public virtual ICollection<TransactionText> TransactionTextdisclaimerNavigations { get; set; }
        [InverseProperty("full_instructionsNavigation")]
        public virtual ICollection<TransactionText> TransactionTextfull_instructionsNavigations { get; set; }
        [InverseProperty("id_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextid_number_captionNavigations { get; set; }
        [InverseProperty("listItem_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextlistItem_captionNavigations { get; set; }
        [InverseProperty("narration_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextnarration_captionNavigations { get; set; }
        [InverseProperty("phone_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextphone_number_captionNavigations { get; set; }
        [InverseProperty("receipt_templateNavigation")]
        public virtual ICollection<TransactionText> TransactionTextreceipt_templateNavigations { get; set; }
        [InverseProperty("reference_account_name_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextreference_account_name_captionNavigations { get; set; }
        [InverseProperty("reference_account_number_captionNavigation")]
        public virtual ICollection<TransactionText> TransactionTextreference_account_number_captionNavigations { get; set; }
        [InverseProperty("termsNavigation")]
        public virtual ICollection<TransactionText> TransactionTexttermsNavigations { get; set; }
        [InverseProperty("error_messageNavigation")]
        public virtual ICollection<ValidationText> ValidationTexterror_messageNavigations { get; set; }
        [InverseProperty("success_messageNavigation")]
        public virtual ICollection<ValidationText> ValidationTextsuccess_messageNavigations { get; set; }
    }
}