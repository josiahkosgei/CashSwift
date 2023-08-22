
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("DeviceLock")]
    // [Index("device_id", Name = "idevice_DeviceLock")]
    // [Index("locking_user", Name = "ilocking_user_DeviceLock")]
    public partial class DeviceLock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        public DateTime lock_date { get; set; }
        public bool locked { get; set; }
        public Guid? locking_user { get; set; }
        [StringLength(50)]
        public string web_locking_user { get; set; }
        public bool locked_by_device { get; set; }

        [ForeignKey("device_id")]
        // [InverseProperty("DeviceLocks")]
        public virtual Device device { get; set; }
        [ForeignKey("locking_user")]
        // [InverseProperty("DeviceLocks")]
        public virtual ApplicationUser lockingUserNavigation { get; set; }
    }
}