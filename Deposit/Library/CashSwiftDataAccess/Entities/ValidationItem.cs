
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("ValidationItem")]
    public partial class ValidationItem
    {
        public ValidationItem()
        {
            ValidationItemValues = new HashSet<ValidationItemValue>();
            ValidationListValidationItems = new HashSet<ValidationListValidationItem>();
            ValidationTexts = new HashSet<ValidationText>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(10)]
        public string category { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public Guid type_id { get; set; }
        public bool enabled { get; set; }
        public int? error_code { get; set; }
        public Guid? validation_text_id { get; set; }

        [ForeignKey("type_id")]
        public virtual ValidationType ValidationType { get; set; }
        [ForeignKey("validation_text_id")]
        public virtual ValidationText ValidationText { get; set; }
        public virtual ICollection<ValidationItemValue> ValidationItemValues { get; set; }
        public virtual ICollection<ValidationListValidationItem> ValidationListValidationItems { get; set; }
        public virtual ICollection<ValidationText> ValidationTexts { get; set; }
    }
}