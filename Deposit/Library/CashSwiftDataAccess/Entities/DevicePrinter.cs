﻿
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("DevicePrinter")]
    // [Index("device_id", Name = "idevice_id_DevicePrinter")]
    public partial class DevicePrinter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        [Required]
        public bool? is_infront { get; set; }
        [Required]
        [StringLength(5)]
        public string port { get; set; }
        [StringLength(50)]
        public string make { get; set; }
        [StringLength(50)]
        public string model { get; set; }
        [StringLength(50)]
        public string serial { get; set; }

        [ForeignKey("device_id")]
        // [InverseProperty("DevicePrinters")]
        public virtual Device device { get; set; }
    }
}