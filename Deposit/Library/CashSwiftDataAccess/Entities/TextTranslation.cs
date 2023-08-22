
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("TextTranslation", Schema = "xlns")]
    public partial class TextTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid TextItemID { get; set; }
        [Required]
        [StringLength(5)]
        // [Unicode(false)]
        public string LanguageCode { get; set; }
        [Required]
        public string TranslationText { get; set; }

        [ForeignKey("LanguageCode")]
        // [InverseProperty("TextTranslations")]
        public virtual Language LanguageCodeNavigation { get; set; }
        [ForeignKey("TextItemID")]
        // [InverseProperty("TextTranslations")]
        public virtual TextItem TextItem { get; set; }
    }
}