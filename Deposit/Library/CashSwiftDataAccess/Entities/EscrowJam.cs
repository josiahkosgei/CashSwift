
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("EscrowJam", Schema = "exp")]
    public partial class EscrowJam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid transaction_id { get; set; }
        public DateTime date_detected { get; set; }
        public long dropped_amount { get; set; }
        public long escrow_amount { get; set; }
        public long posted_amount { get; set; }
        public long retreived_amount { get; set; }
        public DateTime? recovery_date { get; set; }
        public Guid? initialising_user { get; set; }
        public Guid? authorising_user { get; set; }
        [StringLength(100)]
        public string additional_info { get; set; }

        [ForeignKey("authorising_user")]
        public virtual ApplicationUser AuthorisingUser { get; set; }
        [ForeignKey("initialising_user")]
        public virtual ApplicationUser InitialisingUser { get; set; }
        [ForeignKey("transaction_id")]
        public virtual Transaction Transaction { get; set; }

        public string ToRawTextString() => string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t", id, transaction_id, date_detected, dropped_amount, escrow_amount, posted_amount, retreived_amount, recovery_date, InitialisingUser, AuthorisingUser, additional_info);

        public string ToEmailString() => string.Format("<tr><td>{0:yyyy-MM-dd HH:mm:ss.fff}</td><td>{1:#,#0.##}</td><td>{2:#,#0.##}</td><td>{3:#,#0.##}</td><td>{4:#,#0.##}</td><td>{5:yyyy-MM-dd HH:mm:ss.fff}</td><td>{6}</td><td>{7}</td></tr>", date_detected, dropped_amount / 100M, escrow_amount / 100M, posted_amount / 100M, retreived_amount, recovery_date, AuthorisingUser?.username, InitialisingUser?.username);

    }
}