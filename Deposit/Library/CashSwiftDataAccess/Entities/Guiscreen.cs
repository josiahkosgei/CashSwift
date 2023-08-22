
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("GUIScreen")]
    public partial class GUIScreen
    {
        public GUIScreen()
        {
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public int type { get; set; }
        public bool enabled { get; set; }
        public int? keyboard { get; set; }
        public bool? is_masked { get; set; }
        [StringLength(50)]
        public string prefill_text { get; set; }
        [StringLength(50)]
        public string input_mask { get; set; }
        public Guid? gui_text { get; set; }

        [ForeignKey("gui_text")]
        public virtual GUIScreenText GUIScreenText { get; set; }
        [ForeignKey("type")]
        public virtual GUIScreenType GUIScreenType { get; set; }
        public virtual ICollection<GUIScreenText> GUIScreenTexts { get; set; }
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
    }
}