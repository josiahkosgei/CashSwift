
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Bank")]
    public partial class Bank
    {
        public Bank()
        {
            Branches = new HashSet<Branch>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string bank_code { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        [Required]
        [StringLength(2)]
        public string country_code { get; set; }

        [ForeignKey("country_code")]
        public virtual Country Country { get; set; }
        public virtual ICollection<Branch> Branches { get; set; }
    }
}