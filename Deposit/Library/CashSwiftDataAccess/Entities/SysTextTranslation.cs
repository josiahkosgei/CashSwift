
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("sysTextTranslation", Schema = "xlns")]
    public partial class sysTextTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid SysTextItemID { get; set; }
        [Required]
        [StringLength(5)]
        // [Unicode(false)]
        public string LanguageCode { get; set; }
        [Required]
        public string TranslationSysText { get; set; }

        [ForeignKey("LanguageCode")]
        // [InverseProperty("sysTextTranslations")]
        public virtual Language LanguageCodeNavigation { get; set; }
        [ForeignKey("SysTextItemID")]
        // [InverseProperty("sysTextTranslations")]
        public virtual sysTextItem SysTextItem { get; set; }
    }
}