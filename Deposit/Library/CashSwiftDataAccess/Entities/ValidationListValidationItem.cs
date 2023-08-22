
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("ValidationList_ValidationItem")]
    public partial class ValidationListValidationItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid validation_list_id { get; set; }
        public Guid validation_item_id { get; set; }
        public int order { get; set; }
        [Required]
        public bool? enabled { get; set; }

        [ForeignKey("validation_item_id")]
        public virtual ValidationItem ValidationItem { get; set; }
        [ForeignKey("validation_list_id")]
        public virtual ValidationList ValidationList { get; set; }
    }
}