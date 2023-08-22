
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("ValidationItemValue")]
    // [Index("validation_item_id", "order", Name = "UX_ValidationItemValue", IsUnique = true)]
    // [Index("validation_item_id", Name = "ivalidation_item_id_ValidationItemValue")]
    public partial class ValidationItemValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid validation_item_id { get; set; }
        [Required]
        [StringLength(255)]
        public string value { get; set; }
        public int order { get; set; }

        [ForeignKey("validation_item_id")]
        // [InverseProperty("ValidationItemValues")]
        public virtual ValidationItem validation_item { get; set; }
    }
}