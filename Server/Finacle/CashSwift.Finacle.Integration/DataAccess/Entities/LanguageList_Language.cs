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
    /// [m2m] LanguageList and Language
    /// </summary>
    [Table("LanguageList_Language")]
    [Index("language_item", "language_list", Name = "UX_LanguageList_Language_LanguageItem", IsUnique = true)]
    [Index("language_list", "language_order", Name = "UX_LanguageList_Language_Order", IsUnique = true)]
    [Index("language_item", Name = "ilanguage_item_LanguageList_Language")]
    [Index("language_list", Name = "ilanguage_list_LanguageList_Language")]
    public partial class LanguageList_Language
    {
        [Key]
        public Guid id { get; set; }
        public int language_list { get; set; }
        [Required]
        [StringLength(5)]
        [Unicode(false)]
        public string language_item { get; set; }
        public int language_order { get; set; }

        [ForeignKey("language_item")]
        [InverseProperty("LanguageList_Languages")]
        public virtual Language language_itemNavigation { get; set; }
        [ForeignKey("language_list")]
        [InverseProperty("LanguageList_Languages")]
        public virtual LanguageList language_listNavigation { get; set; }
    }
}