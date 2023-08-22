﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("XPObjectType")]
    [Index("TypeName", Name = "iTypeName_XPObjectType", IsUnique = true)]
    public partial class XPObjectType
    {
        public XPObjectType()
        {
            PermissionPolicyRoles = new HashSet<PermissionPolicyRole>();
            PermissionPolicyTypePermissionsObjects = new HashSet<PermissionPolicyTypePermissionsObject>();
            XPWeakReferenceObjectTypeNavigations = new HashSet<XPWeakReference>();
            XPWeakReferenceTargetTypeNavigations = new HashSet<XPWeakReference>();
        }

        [Key]
        public int OID { get; set; }
        [StringLength(254)]
        public string TypeName { get; set; }
        [StringLength(254)]
        public string AssemblyName { get; set; }

        [InverseProperty("ObjectTypeNavigation")]
        public virtual ICollection<PermissionPolicyRole> PermissionPolicyRoles { get; set; }
        [InverseProperty("ObjectTypeNavigation")]
        public virtual ICollection<PermissionPolicyTypePermissionsObject> PermissionPolicyTypePermissionsObjects { get; set; }
        [InverseProperty("ObjectTypeNavigation")]
        public virtual ICollection<XPWeakReference> XPWeakReferenceObjectTypeNavigations { get; set; }
        [InverseProperty("TargetTypeNavigation")]
        public virtual ICollection<XPWeakReference> XPWeakReferenceTargetTypeNavigations { get; set; }
    }
}