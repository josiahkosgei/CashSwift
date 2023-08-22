using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("DeviceType")]
    public partial class DeviceType
    {
        public DeviceType()
        {
            Devices = new HashSet<Device>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string description { get; set; }
        public bool note_in { get; set; }
        public bool note_out { get; set; }
        public bool note_escrow { get; set; }
        public bool coin_in { get; set; }
        public bool coin_out { get; set; }
        public bool coin_escrow { get; set; }

        // [InverseProperty("type")]
        public virtual ICollection<Device> Devices { get; set; }
    }
}