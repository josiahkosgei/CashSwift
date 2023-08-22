
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("ValidationList")]
    public partial class ValidationList
    {
        public ValidationList()
        {
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
            ValidationListValidationItems = new HashSet<ValidationListValidationItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        [StringLength(25)]
        public string category { get; set; }
        public bool enabled { get; set; }

        // [InverseProperty("validation_list")]
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        // [InverseProperty("validation_list")]
        public virtual ICollection<ValidationListValidationItem> ValidationListValidationItems { get; set; }
    }
}