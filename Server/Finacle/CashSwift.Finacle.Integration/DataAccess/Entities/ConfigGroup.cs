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
    /// Group together configurations so devices can share configs
    /// </summary>
    [Table("ConfigGroup")]
    [Index("parent_group", Name = "iparent_group_ConfigGroup")]
    public partial class ConfigGroup
    {
        public ConfigGroup()
        {
            DeviceConfigs = new HashSet<DeviceConfig>();
            Devices = new HashSet<Device>();
            Inverseparent_groupNavigation = new HashSet<ConfigGroup>();
        }

        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(512)]
        public string description { get; set; }
        public int? parent_group { get; set; }

        [ForeignKey("parent_group")]
        [InverseProperty("Inverseparent_groupNavigation")]
        public virtual ConfigGroup parent_groupNavigation { get; set; }
        [InverseProperty("group")]
        public virtual ICollection<DeviceConfig> DeviceConfigs { get; set; }
        [InverseProperty("config_groupNavigation")]
        public virtual ICollection<Device> Devices { get; set; }
        [InverseProperty("parent_groupNavigation")]
        public virtual ICollection<ConfigGroup> Inverseparent_groupNavigation { get; set; }
    }
}