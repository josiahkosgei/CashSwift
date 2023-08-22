
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("PasswordPolicy")]
    public partial class PasswordPolicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int min_length { get; set; }
        public int min_lowercase { get; set; }
        public int min_digits { get; set; }
        public int min_uppercase { get; set; }
        public int min_special { get; set; }
        [Required]
        [StringLength(100)]
        public string allowed_special { get; set; }
        public int expiry_days { get; set; }
        public int history_size { get; set; }
        [Required]
        public bool? use_history { get; set; }
    }
}