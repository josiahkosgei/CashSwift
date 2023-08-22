
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashSwiftDataAccess.Entities
{
    public partial class CITDenomination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid cit_id { get; set; }
        public DateTime? datetime { get; set; }
        [Required]
        [StringLength(3)]
        public string currency_id { get; set; }
        public int denom { get; set; }
        public long count { get; set; }
        public long subtotal { get; set; }

        [ForeignKey(nameof(cit_id))]
        public virtual CIT CIT { get; set; }
        [ForeignKey(nameof(currency_id))]
        public virtual Currency Currency { get; set; }
    }
}