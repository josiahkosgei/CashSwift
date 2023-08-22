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
    /// The system password policy
    /// </summary>
    [Table("PasswordPolicy")]
    public partial class PasswordPolicy
    {
        public PasswordPolicy()
        {
            ApplicationUserChangePasswords = new HashSet<ApplicationUserChangePassword>();
        }

        [Key]
        public Guid id { get; set; }
        public int min_length { get; set; }
        public int min_lowercase { get; set; }
        public int min_digits { get; set; }
        public int min_uppercase { get; set; }
        public int min_special { get; set; }
        [Required]
        [StringLength(100)]
        public string allowed_special { get; set; }
        public int expiry_days { get; set; }
        public int history_size { get; set; }
        [Required]
        public bool? use_history { get; set; }

        [InverseProperty("PasswordPolicyNavigation")]
        public virtual ICollection<ApplicationUserChangePassword> ApplicationUserChangePasswords { get; set; }
    }
}