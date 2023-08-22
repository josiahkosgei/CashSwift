
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("CIT")]
    public partial class CIT
    {
        public CIT()
        {
            CITDenominations = new HashSet<CITDenomination>();
            CITPrintouts = new HashSet<CITPrintout>();
            CITTransactions = new HashSet<CITTransaction>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        public DateTime cit_date { get; set; }
        public DateTime? cit_complete_date { get; set; }
        public Guid start_user { get; set; }
        public Guid? auth_user { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime toDate { get; set; }
        [StringLength(50)]
        public string old_bag_number { get; set; }
        [StringLength(50)]
        public string new_bag_number { get; set; }
        [StringLength(50)]
        public string seal_number { get; set; }
        public bool complete { get; set; }
        public int cit_error { get; set; }
        [StringLength(255)]
        public string cit_error_message { get; set; }

        [ForeignKey("auth_user")]
        public virtual ApplicationUser AuthUser { get; set; }
        [ForeignKey("device_id")]
        public virtual Device Device { get; set; }
        [ForeignKey("start_user")]
        public virtual ApplicationUser StartUser { get; set; }
        public virtual ICollection<CITDenomination> CITDenominations { get; set; }
        public virtual ICollection<CITPrintout> CITPrintouts { get; set; }
        public virtual ICollection<CITTransaction> CITTransactions { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}