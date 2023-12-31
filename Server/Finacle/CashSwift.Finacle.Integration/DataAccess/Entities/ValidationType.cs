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
    /// The type of validation e.g. regex, etc
    /// </summary>
    [Table("ValidationType")]
    public partial class ValidationType
    {
        public ValidationType()
        {
            ValidationItems = new HashSet<ValidationItem>();
        }

        [Key]
        public Guid id { get; set; }
        /// <summary>
        /// common name for the transaction e.g. Mpesa Deposit
        /// </summary>
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        /// <summary>
        /// common description for the transaction type
        /// </summary>
        [StringLength(255)]
        public string description { get; set; }
        public bool enabled { get; set; }

        [InverseProperty("type")]
        public virtual ICollection<ValidationItem> ValidationItems { get; set; }
    }
}