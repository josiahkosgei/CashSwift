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
    /// Link a ValidationItem to a ValidationList
    /// </summary>
    [Table("ValidationList_ValidationItem")]
    [Index("validation_item_id", "validation_list_id", Name = "UX_ValidationList_ValidationItem_UniqueItem", IsUnique = true)]
    [Index("validation_list_id", "order", Name = "UX_ValidationList_ValidationItem_UniqueOrder", IsUnique = true)]
    [Index("validation_item_id", Name = "ivalidation_item_id_ValidationList_ValidationItem")]
    [Index("validation_list_id", Name = "ivalidation_list_id_ValidationList_ValidationItem")]
    public partial class ValidationList_ValidationItem
    {
        [Key]
        public Guid id { get; set; }
        public Guid validation_list_id { get; set; }
        public Guid validation_item_id { get; set; }
        public int order { get; set; }
        [Required]
        public bool? enabled { get; set; }

        [ForeignKey("validation_item_id")]
        [InverseProperty("ValidationList_ValidationItems")]
        public virtual ValidationItem validation_item { get; set; }
        [ForeignKey("validation_list_id")]
        [InverseProperty("ValidationList_ValidationItems")]
        public virtual ValidationList validation_list { get; set; }
    }
}