
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("DeviceLogin")]
    // [Index("User", Name = "iUser_DeviceLogin")]
    public partial class DeviceLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public Guid User { get; set; }
        public bool? Success { get; set; }
        public bool? DepositorEnabled { get; set; }
        public bool? ChangePassword { get; set; }
        [StringLength(200)]
        public string Message { get; set; }
        public bool? ForcedLogout { get; set; }
        public Guid device_id { get; set; }

        [ForeignKey("User")]
        // [InverseProperty("DeviceLogins")]
        public virtual ApplicationUser UserNavigation { get; set; }
        [ForeignKey("device_id")]
        // [InverseProperty("DeviceLogins")]
        public virtual Device device { get; set; }
    }
}