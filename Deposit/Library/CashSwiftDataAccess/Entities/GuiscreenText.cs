
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("GUIScreenText")]
    public partial class GUIScreenText
    {
        public GUIScreenText()
        {
            GUIScreens = new HashSet<GUIScreen>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int guiscreen_id { get; set; }
        public Guid screen_title { get; set; }
        public Guid? screen_title_instruction { get; set; }
        public Guid? full_instructions { get; set; }
        public Guid? btn_accept_caption { get; set; }
        public Guid? btn_back_caption { get; set; }
        public Guid? btn_cancel_caption { get; set; }

        [ForeignKey("btn_accept_caption")]
        // [InverseProperty("GUIScreenTextbtn_accept_captionNavigation")]
        public virtual TextItem btn_accept_captionNavigation { get; set; }
        [ForeignKey("btn_back_caption")]
        // [InverseProperty("GUIScreenTextbtn_back_captionNavigation")]
        public virtual TextItem btn_back_captionNavigation { get; set; }
        [ForeignKey("btn_cancel_caption")]
        // [InverseProperty("GUIScreenTextbtn_cancel_captionNavigation")]
        public virtual TextItem btn_cancel_captionNavigation { get; set; }
        [ForeignKey("full_instructions")]
        // [InverseProperty("GUIScreenTextfull_instructionsNavigation")]
        public virtual TextItem full_instructionsNavigation { get; set; }
        [ForeignKey("guiscreen_id")]
        // [InverseProperty("GUIScreenText")]
        public virtual GUIScreen GUIScreen { get; set; }
        [ForeignKey("screen_title")]
        // [InverseProperty("GUIScreenTextscreen_titleNavigation")]
        public virtual TextItem screen_titleNavigation { get; set; }
        [ForeignKey("screen_title_instruction")]
        // [InverseProperty("GUIScreenTextscreen_title_instructionNavigation")]
        public virtual TextItem screen_title_instructionNavigation { get; set; }
        // [InverseProperty("gui_textNavigation")]
        public virtual ICollection<GUIScreen> GUIScreens { get; set; }
    }
}