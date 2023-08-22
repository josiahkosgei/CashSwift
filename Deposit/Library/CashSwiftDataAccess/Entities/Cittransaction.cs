
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("CITTransaction")]
    public partial class CITTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime datetime { get; set; }
        public Guid cit_id { get; set; }
        [Required]
        [StringLength(3)]
        public string currency { get; set; }
        public long amount { get; set; }
        [Required]
        [StringLength(50)]
        public string suspense_account { get; set; }
        [Required]
        [StringLength(50)]
        public string account_number { get; set; }
        [StringLength(50)]
        public string narration { get; set; }
        [StringLength(50)]
        public string cb_tx_number { get; set; }
        public DateTime? cb_date { get; set; }
        [StringLength(50)]
        public string cb_tx_status { get; set; }
        public string cb_status_detail { get; set; }
        public int error_code { get; set; }
        [StringLength(255)]
        public string error_message { get; set; }

        [ForeignKey("cit_id")]
        public virtual CIT CIT { get; set; }
    }
}