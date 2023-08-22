using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Language")]
    public partial class Language
    {
        public Language()
        {
            DepositorSessions = new HashSet<DepositorSession>();
            LanguageListLanguages = new HashSet<LanguageListLanguage>();
            LanguageLists = new HashSet<LanguageList>();
            TextTranslations = new HashSet<TextTranslation>();
            sysTextTranslations = new HashSet<sysTextTranslation>();
        }

        [Key]
        [StringLength(5)]
        // [Unicode(false)]
        public string code { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string flag { get; set; }
        public bool enabled { get; set; }

        // [InverseProperty("language_codeNavigation")]
        public virtual ICollection<DepositorSession> DepositorSessions { get; set; }
        // [InverseProperty("language_itemNavigation")]
        public virtual ICollection<LanguageListLanguage> LanguageListLanguages { get; set; }
        // [InverseProperty("default_languageNavigation")]
        public virtual ICollection<LanguageList> LanguageLists { get; set; }
        // [InverseProperty("LanguageCodeNavigation")]
        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
        // [InverseProperty("LanguageCodeNavigation")]
        public virtual ICollection<sysTextTranslation> sysTextTranslations { get; set; }
    }
}