﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    /// <summary>
    /// Link a Device to its configuration
    /// </summary>
    [Table("DeviceConfig")]
    [Index("config_id", "group_id", Name = "UX_DeviceConfig", IsUnique = true)]
    [Index("config_id", Name = "iconfig_id_DeviceConfig")]
    [Index("group_id", Name = "igroup_id_DeviceConfig")]
    public partial class DeviceConfig
    {
        [Key]
        public Guid id { get; set; }
        public int group_id { get; set; }
        [Required]
        [StringLength(50)]
        public string config_id { get; set; }
        [Required]
        public string config_value { get; set; }

        [ForeignKey("config_id")]
        [InverseProperty("DeviceConfigs")]
        public virtual Config config { get; set; }
        [ForeignKey("group_id")]
        [InverseProperty("DeviceConfigs")]
        public virtual ConfigGroup group { get; set; }
    }
}