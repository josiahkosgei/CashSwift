
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("ValidationType")]
    public partial class ValidationType
    {
        public ValidationType()
        {
            ValidationItems = new HashSet<ValidationItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public bool enabled { get; set; }

        // [InverseProperty("type")]
        public virtual ICollection<ValidationItem> ValidationItems { get; set; }
    }
}