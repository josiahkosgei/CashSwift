﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("AlertEmailAttachment")]
    public partial class AlertEmailAttachment
    {
        [Key]
        public Guid id { get; set; }
        public Guid alert_email_id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string path { get; set; }
        [Required]
        [StringLength(6)]
        public string type { get; set; }
        [Required]
        public byte[] data { get; set; }
        [Required]
        [MaxLength(64)]
        public byte[] hash { get; set; }
    }
}