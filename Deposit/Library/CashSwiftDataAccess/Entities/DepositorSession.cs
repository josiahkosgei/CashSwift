
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("DepositorSession")]
    // [Index("device_id", Name = "idevice_id_DepositorSession")]
    // [Index("language_code", Name = "ilanguage_code_DepositorSession")]
    public partial class DepositorSession
    {
        public DepositorSession()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        public DateTime session_start { get; set; }
        public DateTime? session_end { get; set; }
        [StringLength(5)]
        // [Unicode(false)]
        public string language_code { get; set; }
        public bool complete { get; set; }
        public bool complete_success { get; set; }
        public int? error_code { get; set; }
        [StringLength(255)]
        public string error_message { get; set; }
        public bool terms_accepted { get; set; }
        public bool account_verified { get; set; }
        public bool reference_account_verified { get; set; }
        [StringLength(64)]
        // [Unicode(false)]
        public string salt { get; set; }

        [ForeignKey("device_id")]
        // [InverseProperty("DepositorSessions")]
        public virtual Device device { get; set; }
        [ForeignKey("language_code")]
        // [InverseProperty("DepositorSessions")]
        public virtual Language language_codeNavigation { get; set; }
        // [InverseProperty("session")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}