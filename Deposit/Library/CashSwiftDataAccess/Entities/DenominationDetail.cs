
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("DenominationDetail")]
    // [Index("tx_id", Name = "itx_id_DenominationDetail")]
    public partial class DenominationDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid tx_id { get; set; }
        public int denom { get; set; }
        public long count { get; set; }
        public long subtotal { get; set; }
        public DateTime? datetime { get; set; }

        [ForeignKey("tx_id")]
        //// [InverseProperty("DenominationDetails")]
        public virtual Transaction tx { get; set; }
    }
}