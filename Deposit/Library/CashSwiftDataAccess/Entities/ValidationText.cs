
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("ValidationText")]
    public partial class ValidationText
    {
        public ValidationText()
        {
            ValidationItems = new HashSet<ValidationItem>();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid validation_item_id { get; set; }
        public Guid? error_message { get; set; }
        public Guid? success_message { get; set; }

        [ForeignKey("error_message")]
        // [InverseProperty("ValidationTexterror_messageNavigation")]
        public virtual TextItem error_messageNavigation { get; set; }
        [ForeignKey("success_message")]
        // [InverseProperty("ValidationTextsuccess_messageNavigation")]
        public virtual TextItem success_messageNavigation { get; set; }
        [ForeignKey("validation_item_id")]
        // [InverseProperty("ValidationTexts")]
        public virtual ValidationItem validation_item { get; set; }
        // [InverseProperty("validation_text")]
        public virtual ICollection<ValidationItem> ValidationItems { get; set; }
    }
}