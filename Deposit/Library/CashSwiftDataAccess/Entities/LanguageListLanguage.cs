
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("LanguageList_Language")]
    public partial class LanguageListLanguage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int language_list { get; set; }
        [Required]
        [StringLength(5)]
        // [Unicode(false)]
        public string language_item { get; set; }
        public int language_order { get; set; }

        [ForeignKey("language_item")]
        public virtual Language Language { get; set; }
        [ForeignKey("language_list")]
        public virtual LanguageList LanguageList { get; set; }
    }
}