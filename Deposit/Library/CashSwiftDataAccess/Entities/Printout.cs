
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Printout")]
    // [Index("tx_id", Name = "itx_id_Printout")]
    public partial class Printout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime datetime { get; set; }
        public Guid tx_id { get; set; }
        public Guid print_guid { get; set; }
        public string print_content { get; set; }
        public bool is_copy { get; set; }

        [ForeignKey("tx_id")]
        // [InverseProperty("Printouts")]
        public virtual Transaction tx { get; set; }
    }
}