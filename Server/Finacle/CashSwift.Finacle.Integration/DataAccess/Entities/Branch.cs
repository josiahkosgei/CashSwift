﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("Branch")]
    [Index("bank_id", Name = "ibank_id_Branch")]
    public partial class Branch
    {
        public Branch()
        {
            Devices = new HashSet<Device>();
        }

        [Key]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string branch_code { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public Guid bank_id { get; set; }

        [ForeignKey("bank_id")]
        [InverseProperty("Branches")]
        public virtual Bank bank { get; set; }
        [InverseProperty("branch")]
        public virtual ICollection<Device> Devices { get; set; }
    }
}