﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("sysTextItem", Schema = "xlns")]
    [Index("Token", Name = "UX_SysTextItem_name", IsUnique = true)]
    [Index("Category", Name = "iCategory_xlns_sysTextItem_A264365A")]
    [Index("TextItemTypeID", Name = "iTextItemTypeID_xlns_sysTextItem_BD18CE82")]
    public partial class sysTextItem
    {
        public sysTextItem()
        {
            sysTextTranslations = new HashSet<sysTextTranslation>();
        }

        [Key]
        public Guid id { get; set; }
        [Required]
        [StringLength(100)]
        public string Token { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public string DefaultTranslation { get; set; }
        public Guid Category { get; set; }
        public Guid? TextItemTypeID { get; set; }

        [ForeignKey("Category")]
        [InverseProperty("sysTextItems")]
        public virtual sysTextItemCategory CategoryNavigation { get; set; }
        [ForeignKey("TextItemTypeID")]
        [InverseProperty("sysTextItems")]
        public virtual sysTextItemType TextItemType { get; set; }
        [InverseProperty("SysTextItem")]
        public virtual ICollection<sysTextTranslation> sysTextTranslations { get; set; }
    }
}