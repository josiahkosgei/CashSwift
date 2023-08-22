﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("PermissionPolicyObjectPermissionsObject")]
    [Index("GCRecord", Name = "iGCRecord_PermissionPolicyObjectPermissionsObject")]
    [Index("TypePermissionObject", Name = "iTypePermissionObject_PermissionPolicyObjectPermissionsObject")]
    public partial class PermissionPolicyObjectPermissionsObject
    {
        [Key]
        public Guid Oid { get; set; }
        public string Criteria { get; set; }
        public int? ReadState { get; set; }
        public int? WriteState { get; set; }
        public int? DeleteState { get; set; }
        public int? NavigateState { get; set; }
        public Guid? TypePermissionObject { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? GCRecord { get; set; }

        [ForeignKey("TypePermissionObject")]
        [InverseProperty("PermissionPolicyObjectPermissionsObjects")]
        public virtual PermissionPolicyTypePermissionsObject TypePermissionObjectNavigation { get; set; }
    }
}