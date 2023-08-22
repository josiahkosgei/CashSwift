
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("GUIPrepopList_Item")]
    public partial class GUIPrepopListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid List { get; set; }
        public Guid Item { get; set; }
        public int List_Order { get; set; }

        [ForeignKey("Item")]
        public virtual GUIPrepopItem GUIPrepopItemNavigation { get; set; }
        [ForeignKey("List")]
        public virtual GUIPrepopList GUIPrepopListNavigation { get; set; }
    }
}