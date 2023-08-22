
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TextItemType", Schema = "xlns")]
    // [Index("token", Name = "UX_UI_TextItemType", IsUnique = true)]
    public partial class TextItemType
    {
        public TextItemType()
        {
            TextItems = new HashSet<TextItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(100)]
        public string token { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }

        // [InverseProperty("TextItemType")]
        public virtual ICollection<TextItem> TextItems { get; set; }
    }
}