using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("LanguageList")]
    public partial class LanguageList
    {
        public LanguageList()
        {
            Devices = new HashSet<Device>();
            LanguageListLanguages = new HashSet<LanguageListLanguage>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(100)]
        public string description { get; set; }
        [Required]
        public bool? enabled { get; set; }
        [Required]
        [StringLength(5)]
        public string default_language { get; set; }

        [ForeignKey("default_language")]
        public virtual Language LanguageNavigation { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<LanguageListLanguage> LanguageListLanguages { get; set; }
    }
}