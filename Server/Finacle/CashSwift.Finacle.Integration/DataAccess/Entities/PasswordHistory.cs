﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("PasswordHistory")]
    [Index("User", Name = "iUser_PasswordHistory")]
    public partial class PasswordHistory
    {
        [Key]
        public Guid id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDate { get; set; }
        public Guid? User { get; set; }
        [StringLength(71)]
        public string Password { get; set; }

        [ForeignKey("User")]
        [InverseProperty("PasswordHistories")]
        public virtual ApplicationUser UserNavigation { get; set; }
    }
}