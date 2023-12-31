﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("WebUserPasswordHistory")]
    [Index("GCRecord", Name = "iGCRecord_WebUserPasswordHistory")]
    public partial class WebUserPasswordHistory
    {
        [Key]
        public Guid Oid { get; set; }
        [StringLength(100)]
        public string user { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? datetime { get; set; }
        [StringLength(100)]
        public string password { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? GCRecord { get; set; }
    }
}