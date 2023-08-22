
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashSwiftDataAccess.Entities
{
    public partial class DeviceStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        [Required]
        [StringLength(20)]
        public string controller_state { get; set; }
        [Required]
        [StringLength(20)]
        public string ba_type { get; set; }
        [Required]
        [StringLength(20)]
        public string ba_status { get; set; }
        [Required]
        [StringLength(3)]
        public string ba_currency { get; set; }
        [Required]
        [StringLength(50)]
        public string bag_number { get; set; }
        [Required]
        [StringLength(20)]
        public string bag_status { get; set; }
        public int bag_note_level { get; set; }
        [Required]
        [StringLength(10)]
        public string bag_note_capacity { get; set; }
        public long? bag_value_level { get; set; }
        public long? bag_value_capacity { get; set; }
        public int bag_percent_full { get; set; }
        [Required]
        [StringLength(20)]
        public string sensors_type { get; set; }
        [Required]
        [StringLength(20)]
        public string sensors_status { get; set; }
        public int sensors_value { get; set; }
        [Required]
        [StringLength(20)]
        public string sensors_door { get; set; }
        [Required]
        [StringLength(20)]
        public string sensors_bag { get; set; }
        [Required]
        [StringLength(20)]
        public string escrow_type { get; set; }
        [Required]
        [StringLength(20)]
        public string escrow_status { get; set; }
        [Required]
        [StringLength(20)]
        public string escrow_position { get; set; }
        [StringLength(20)]
        public string transaction_status { get; set; }
        [StringLength(20)]
        public string transaction_type { get; set; }
        public DateTime? machine_datetime { get; set; }
        public int current_status { get; set; }
        public DateTime? modified { get; set; }
        [Required]
        [StringLength(50)]
        public string machine_name { get; set; }

        [ForeignKey("device_id")]
        public virtual Device Device { get; set; }
    }
}