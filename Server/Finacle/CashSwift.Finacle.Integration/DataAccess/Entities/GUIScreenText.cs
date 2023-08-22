﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    /// <summary>
    /// Stores the text for a screen for a language
    /// </summary>
    [Table("GUIScreenText")]
    [Index("guiscreen_id", Name = "UX_GUIScreenText_gui_screen_id", IsUnique = true)]
    [Index("btn_accept_caption", Name = "ibtn_accept_caption_GUIScreenText")]
    [Index("btn_back_caption", Name = "ibtn_back_caption_GUIScreenText")]
    [Index("btn_cancel_caption", Name = "ibtn_cancel_caption_GUIScreenText")]
    [Index("full_instructions", Name = "ifull_instructions_GUIScreenText")]
    [Index("guiscreen_id", Name = "iguiscreen_id_GUIScreenText")]
    [Index("screen_title", Name = "iscreen_title_GUIScreenText")]
    [Index("screen_title_instruction", Name = "iscreen_title_instruction_GUIScreenText")]
    public partial class GUIScreenText
    {
        public GUIScreenText()
        {
            GUIScreens = new HashSet<GUIScreen>();
        }

        [Key]
        public Guid id { get; set; }
        /// <summary>
        /// The GUIScreen this entry corresponds to
        /// </summary>
        public int guiscreen_id { get; set; }
        public Guid screen_title { get; set; }
        public Guid? screen_title_instruction { get; set; }
        public Guid? full_instructions { get; set; }
        public Guid? btn_accept_caption { get; set; }
        public Guid? btn_back_caption { get; set; }
        public Guid? btn_cancel_caption { get; set; }

        [ForeignKey("btn_accept_caption")]
        [InverseProperty("GUIScreenTextbtn_accept_captionNavigations")]
        public virtual TextItem btn_accept_captionNavigation { get; set; }
        [ForeignKey("btn_back_caption")]
        [InverseProperty("GUIScreenTextbtn_back_captionNavigations")]
        public virtual TextItem btn_back_captionNavigation { get; set; }
        [ForeignKey("btn_cancel_caption")]
        [InverseProperty("GUIScreenTextbtn_cancel_captionNavigations")]
        public virtual TextItem btn_cancel_captionNavigation { get; set; }
        [ForeignKey("full_instructions")]
        [InverseProperty("GUIScreenTextfull_instructionsNavigations")]
        public virtual TextItem full_instructionsNavigation { get; set; }
        [ForeignKey("guiscreen_id")]
        [InverseProperty("GUIScreenText")]
        public virtual GUIScreen guiscreen { get; set; }
        [ForeignKey("screen_title")]
        [InverseProperty("GUIScreenTextscreen_titleNavigations")]
        public virtual TextItem screen_titleNavigation { get; set; }
        [ForeignKey("screen_title_instruction")]
        [InverseProperty("GUIScreenTextscreen_title_instructionNavigations")]
        public virtual TextItem screen_title_instructionNavigation { get; set; }
        [InverseProperty("gui_textNavigation")]
        public virtual ICollection<GUIScreen> GUIScreens { get; set; }
    }
}