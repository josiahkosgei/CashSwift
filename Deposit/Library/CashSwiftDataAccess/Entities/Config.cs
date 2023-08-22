
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Config")]
    // [Index("category_id", Name = "icategory_id_Config")]
    public partial class Config
    {
        public Config()
        {
            DeviceConfigs = new HashSet<DeviceConfig>();
        }

        [Key]
        [StringLength(50)]
        public string name { get; set; }
        public string default_value { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public Guid category_id { get; set; }

        [ForeignKey("category_id")]
        // [InverseProperty("Configs")]
        public virtual ConfigCategory category { get; set; }
        // [InverseProperty("config")]
        public virtual ICollection<DeviceConfig> DeviceConfigs { get; set; }
    }
}