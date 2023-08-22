
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("DeviceConfig")]
    public partial class DeviceConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int group_id { get; set; }
        [Required]
        [StringLength(50)]
        public string config_id { get; set; }
        [Required]
        public string config_value { get; set; }

        [ForeignKey("config_id")]
        public virtual Config config { get; set; }
        [ForeignKey("group_id")]
        public virtual ConfigGroup group { get; set; }
    }
}