using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("CurrencyList")]
    public partial class CurrencyList
    {
        public CurrencyList()
        {
            CurrencyListCurrencies = new HashSet<CurrencyListCurrency>();
            Devices = new HashSet<Device>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string description { get; set; }
        [Required]
        [StringLength(3)]
        // [Unicode(false)]
        public string default_currency { get; set; }

        [ForeignKey("default_currency")]
        public virtual Currency DefaultCurrencyNavigation { get; set; }
        public virtual ICollection<CurrencyListCurrency> CurrencyListCurrencies { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}