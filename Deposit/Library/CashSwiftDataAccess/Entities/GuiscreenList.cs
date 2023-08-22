using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("GUIScreenList")]
    public partial class GUIScreenList
    {
        public GUIScreenList()
        {
            Devices = new HashSet<Device>();
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public bool enabled { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}