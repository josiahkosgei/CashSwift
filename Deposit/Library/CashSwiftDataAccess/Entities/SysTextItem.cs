
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("sysTextItem", Schema = "xlns")]
    public partial class sysTextItem
    {
        public sysTextItem()
        {
            sysTextTranslations = new HashSet<sysTextTranslation>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        // [InverseProperty("sysTextItems")]
        public virtual sysTextItemCategory CategoryNavigation { get; set; }
        [ForeignKey("TextItemTypeID")]
        // [InverseProperty("sysTextItems")]
        public virtual sysTextItemType TextItemType { get; set; }
        // [InverseProperty("SysTextItem")]
        public virtual ICollection<sysTextTranslation> sysTextTranslations { get; set; }
    }
}