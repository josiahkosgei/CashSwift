
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    public partial class AlertSMS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime created { get; set; }
        [StringLength(100)]
        public string from { get; set; }
        public string to { get; set; }
        public string message { get; set; }
        public bool sent { get; set; }
        public DateTime? send_date { get; set; }
        public Guid alert_event_id { get; set; }
        public bool send_error { get; set; }
        [StringLength(255)]
        public string send_error_message { get; set; }

        [ForeignKey("alert_event_id")]
        public virtual AlertEvent alert_event { get; set; }
    }
}