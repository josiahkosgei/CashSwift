
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("GUIPrepopItem")]
    public partial class GUIPrepopItem
    {
        public GUIPrepopItem()
        {
            GUIPrepopListItems = new HashSet<GUIPrepopListItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(100)]
        public string description { get; set; }
        public Guid value { get; set; }
        [Required]
        public bool? enabled { get; set; }

        [ForeignKey("value")]
        public virtual TextItem TextItemNavigation { get; set; }
        public virtual ICollection<GUIPrepopListItem> GUIPrepopListItems { get; set; }
    }
}