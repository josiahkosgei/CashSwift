﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("sysTextTranslation", Schema = "xlns")]
    [Index("LanguageCode", "SysTextItemID", Name = "UX_Translation_Language_Pair", IsUnique = true)]
    [Index("LanguageCode", Name = "iLanguageCode_xlns_sysTextTranslation_03BB080F")]
    [Index("SysTextItemID", Name = "iSysTextItemID_xlns_sysTextTranslation_7FDC4652")]
    public partial class sysTextTranslation
    {
        [Key]
        public Guid id { get; set; }
        public Guid SysTextItemID { get; set; }
        [Required]
        [StringLength(5)]
        [Unicode(false)]
        public string LanguageCode { get; set; }
        [Required]
        public string TranslationSysText { get; set; }

        [ForeignKey("LanguageCode")]
        [InverseProperty("sysTextTranslations")]
        public virtual Language LanguageCodeNavigation { get; set; }
        [ForeignKey("SysTextItemID")]
        [InverseProperty("sysTextTranslations")]
        public virtual sysTextItem SysTextItem { get; set; }
    }
}