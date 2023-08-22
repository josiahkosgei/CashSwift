
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("CurrencyList_Currency")]
    public partial class CurrencyListCurrency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int currency_list { get; set; }
        [Required]
        [StringLength(3)]
        // [Unicode(false)]
        public string currency_item { get; set; }
        public int currency_order { get; set; }
        public long max_value { get; set; }
        public int max_count { get; set; }

        [ForeignKey("currency_item")]
        public virtual Currency Currency { get; set; }
        [ForeignKey("currency_list")]
        public virtual CurrencyList CurrencyList { get; set; }
    }
}