﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("sysTextItemCategory", Schema = "xlns")]
    // [Index("name", Name = "UX_TextItemCategory_name", IsUnique = true)]
    // [Index("Parent", Name = "iParent_xlns_sysTextItemCategory_51488F7B")]
    public partial class sysTextItemCategory
    {
        public sysTextItemCategory()
        {
            InverseParentNavigation = new HashSet<sysTextItemCategory>();
            sysTextItems = new HashSet<sysTextItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public Guid? Parent { get; set; }

        [ForeignKey("Parent")]
        // [InverseProperty("InverseParentNavigation")]
        public virtual sysTextItemCategory ParentNavigation { get; set; }
        // [InverseProperty("ParentNavigation")]
        public virtual ICollection<sysTextItemCategory> InverseParentNavigation { get; set; }
        // [InverseProperty("CategoryNavigation")]
        public virtual ICollection<sysTextItem> sysTextItems { get; set; }
    }
}