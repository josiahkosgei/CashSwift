
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TransactionText")]
    public partial class TransactionText
    {
        public TransactionText()
        {
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int tx_item { get; set; }
        public Guid? disclaimer { get; set; }
        public Guid? terms { get; set; }
        public Guid? full_instructions { get; set; }
        public Guid? listItem_caption { get; set; }
        public Guid? account_number_caption { get; set; }
        public Guid? account_name_caption { get; set; }
        public Guid? reference_account_number_caption { get; set; }
        public Guid? reference_account_name_caption { get; set; }
        public Guid? narration_caption { get; set; }
        public Guid? alias_account_number_caption { get; set; }
        public Guid? alias_account_name_caption { get; set; }
        public Guid? depositor_name_caption { get; set; }
        public Guid? phone_number_caption { get; set; }
        public Guid? id_number_caption { get; set; }
        public Guid? receipt_template { get; set; }
        public Guid? FundsSource_caption { get; set; }
        [ForeignKey("FundsSource_caption")]
        // [InverseProperty("TransactionTextFundsSource_captionNavigation")]
        public virtual TextItem FundsSource_captionNavigation { get; set; }
        [ForeignKey("account_name_caption")]
        // [InverseProperty("TransactionTextaccount_name_captionNavigation")]
        public virtual TextItem account_name_captionNavigation { get; set; }
        [ForeignKey("account_number_caption")]
        // [InverseProperty("TransactionTextaccount_number_captionNavigation")]
        public virtual TextItem account_number_captionNavigation { get; set; }
        [ForeignKey("alias_account_name_caption")]
        // [InverseProperty("TransactionTextalias_account_name_captionNavigation")]
        public virtual TextItem alias_account_name_captionNavigation { get; set; }
        [ForeignKey("alias_account_number_caption")]
        // [InverseProperty("TransactionTextalias_account_number_captionNavigation")]
        public virtual TextItem alias_account_number_captionNavigation { get; set; }
        [ForeignKey("depositor_name_caption")]
        // [InverseProperty("TransactionTextdepositor_name_captionNavigation")]
        public virtual TextItem depositor_name_captionNavigation { get; set; }
        [ForeignKey("disclaimer")]
        // [InverseProperty("TransactionTextdisclaimerNavigations")]
        public virtual TextItem disclaimerNavigation { get; set; }
        [ForeignKey("full_instructions")]
        // [InverseProperty("TransactionTextfull_instructionsNavigation")]
        public virtual TextItem full_instructionsNavigation { get; set; }
        [ForeignKey("id_number_caption")]
        // [InverseProperty("TransactionTextid_number_captionNavigation")]
        public virtual TextItem id_number_captionNavigation { get; set; }
        [ForeignKey("listItem_caption")]
        // [InverseProperty("TransactionTextlistItem_captionNavigation")]
        public virtual TextItem listItem_captionNavigation { get; set; }
        [ForeignKey("narration_caption")]
        // [InverseProperty("TransactionTextnarration_captionNavigation")]
        public virtual TextItem narration_captionNavigation { get; set; }
        [ForeignKey("phone_number_caption")]
        // [InverseProperty("TransactionTextphone_number_captionNavigation")]
        public virtual TextItem phone_number_captionNavigation { get; set; }
        [ForeignKey("receipt_template")]
        // [InverseProperty("TransactionTextreceipt_templateNavigation")]
        public virtual TextItem receipt_templateNavigation { get; set; }
        [ForeignKey("reference_account_name_caption")]
        // [InverseProperty("TransactionTextreference_account_name_captionNavigation")]
        public virtual TextItem reference_account_name_captionNavigation { get; set; }
        [ForeignKey("reference_account_number_caption")]
        // [InverseProperty("TransactionTextreference_account_number_captionNavigation")]
        public virtual TextItem reference_account_number_captionNavigation { get; set; }
        [ForeignKey("terms")]
        // [InverseProperty("TransactionTexttermsNavigations")]
        public virtual TextItem termsNavigation { get; set; }
        [ForeignKey("tx_item")]
        // [InverseProperty("TransactionText")]
        public virtual TransactionTypeListItem TransactionTypeListItemNavigation { get; set; }
        //[InverseProperty("TransactionTextNavigation")]
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}