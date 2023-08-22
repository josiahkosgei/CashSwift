﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("UserLock")]
    [Index("ApplicationUserLoginDetail", Name = "iApplicationUserLoginDetail_UserLock")]
    [Index("InitiatingUser", Name = "iInitiatingUser_UserLock")]
    public partial class UserLock
    {
        [Key]
        public Guid id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDate { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }
        public int? LockType { get; set; }
        public bool? WebPortalInitiated { get; set; }
        public Guid? InitiatingUser { get; set; }

        [ForeignKey("ApplicationUserLoginDetail")]
        [InverseProperty("UserLocks")]
        public virtual ApplicationUserLoginDetail ApplicationUserLoginDetailNavigation { get; set; }
        [ForeignKey("InitiatingUser")]
        [InverseProperty("UserLocks")]
        public virtual ApplicationUser InitiatingUserNavigation { get; set; }
    }
}