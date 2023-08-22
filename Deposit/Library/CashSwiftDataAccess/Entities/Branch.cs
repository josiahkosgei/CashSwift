
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Branch")]
    public partial class Branch
    {
        public Branch()
        {
            Devices = new HashSet<Device>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string branch_code { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public Guid bank_id { get; set; }

        [ForeignKey("bank_id")]
        public virtual Bank Bank { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}