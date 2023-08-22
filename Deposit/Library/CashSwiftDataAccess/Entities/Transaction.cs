
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Transaction")]
    public partial class Transaction
    {
        public Transaction()
        {
            DenominationDetails = new HashSet<DenominationDetail>();
            EscrowJams = new HashSet<EscrowJam>();
            Printouts = new HashSet<Printout>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int? tx_type { get; set; }
        public Guid session_id { get; set; }
        public int? tx_random_number { get; set; }
        public Guid device_id { get; set; }
        public DateTime tx_start_date { get; set; }
        public DateTime? tx_end_date { get; set; }
        public bool tx_completed { get; set; }
        [StringLength(3)]
        // [Unicode(false)]
        public string tx_currency { get; set; }
        public long? tx_amount { get; set; }
        [StringLength(50)]
        public string tx_account_number { get; set; }
        [StringLength(100)]
        public string cb_account_name { get; set; }
        [StringLength(50)]
        public string tx_ref_account { get; set; }
        [StringLength(100)]
        public string cb_ref_account_name { get; set; }
        [StringLength(50)]
        public string tx_narration { get; set; }
        [StringLength(50)]
        public string tx_depositor_name { get; set; }
        [StringLength(50)]
        public string tx_id_number { get; set; }
        [StringLength(50)]
        public string tx_phone { get; set; }
        [StringLength(255)]
        public string funds_source { get; set; }
        public int tx_result { get; set; }
        public int tx_error_code { get; set; }
        [StringLength(255)]
        public string tx_error_message { get; set; }
        [StringLength(50)]
        public string cb_tx_number { get; set; }
        public DateTime? cb_date { get; set; }
        [StringLength(50)]
        public string cb_tx_status { get; set; }
        public string cb_status_detail { get; set; }
        public bool notes_rejected { get; set; }
        public bool jam_detected { get; set; }
        public Guid? cit_id { get; set; }
        public bool escrow_jam { get; set; }
        [StringLength(50)]
        public string tx_suspense_account { get; set; }
        public Guid? init_user { get; set; }
        public Guid? auth_user { get; set; }

        [ForeignKey("auth_user")]
        public virtual ApplicationUser AuthUserNavigation { get; set; }
        [ForeignKey("cit_id")]
        public virtual CIT CIT { get; set; }
        [ForeignKey("device_id")]
        public virtual Device Device { get; set; }
        [ForeignKey("init_user")]
        public virtual ApplicationUser InitUserNavigation { get; set; }
        [ForeignKey("session_id")]
        public virtual DepositorSession Session { get; set; }
        [ForeignKey("tx_currency")]
        public virtual Currency Currency { get; set; }
        [ForeignKey("tx_type")]
        public virtual TransactionTypeListItem TransactionTypeListItem { get; set; }
        public virtual ICollection<DenominationDetail> DenominationDetails { get; set; }
        public virtual ICollection<EscrowJam> EscrowJams { get; set; }
        public virtual ICollection<Printout> Printouts { get; set; }
    }
}