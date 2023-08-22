
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("GuiScreenList_Screen")]
    public partial class GuiScreenListScreen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int screen { get; set; }
        public int gui_screen_list { get; set; }
        public int screen_order { get; set; }
        public bool required { get; set; }
        public Guid? validation_list_id { get; set; }
        public Guid? guiprepoplist_id { get; set; }
        public bool enabled { get; set; }

        [ForeignKey("gui_screen_list")]
        public virtual GUIScreenList GuiScreenList { get; set; }
        [ForeignKey("guiprepoplist_id")]
        public virtual GUIPrepopList GUIPrepopList { get; set; }
        [ForeignKey("screen")]
        public virtual GUIScreen GUIScreen { get; set; }
        [ForeignKey("validation_list_id")]
        public virtual ValidationList ValidationList { get; set; }
    }
}