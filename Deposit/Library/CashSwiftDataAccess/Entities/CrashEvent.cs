
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    /// <summary>
    /// contains a crash report
    /// </summary>
    [Table("CrashEvent", Schema = "exp")]
    // [Index("device_id", Name = "idevice_id_exp_CrashEvent_7BCB8F5E")]
    public partial class CrashEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        public DateTime datetime { get; set; }
        public DateTime date_detected { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        [StringLength(50)]
        public string machine_name { get; set; }
    }
}