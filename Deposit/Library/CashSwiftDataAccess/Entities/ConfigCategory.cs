
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("ConfigCategory")]
    public partial class ConfigCategory
    {
        public ConfigCategory()
        {
            Configs = new HashSet<Config>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }

        // [InverseProperty("category")]
        public virtual ICollection<Config> Configs { get; set; }
    }
}