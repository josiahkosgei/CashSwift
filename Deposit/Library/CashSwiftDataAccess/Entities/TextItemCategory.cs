﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TextItemCategory", Schema = "xlns")]
    // [Index("name", Name = "UX_UI_TextItemCategory_name", IsUnique = true)]
    // [Index("Parent", Name = "iParent_xlns_TextItemCategory_051D04A6")]
    public partial class TextItemCategory
    {
        public TextItemCategory()
        {
            InverseParentNavigation = new HashSet<TextItemCategory>();
            TextItems = new HashSet<TextItem>();
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
        public virtual TextItemCategory ParentNavigation { get; set; }
        // [InverseProperty("ParentNavigation")]
        public virtual ICollection<TextItemCategory> InverseParentNavigation { get; set; }
        // [InverseProperty("CategoryNavigation")]
        public virtual ICollection<TextItem> TextItems { get; set; }
    }
}