
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("AlertEmail")]
    public partial class AlertEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime created { get; set; }
        [StringLength(100)]
        public string from { get; set; }
        public string to { get; set; }
        [Required]
        [StringLength(100)]
        public string subject { get; set; }
        public string html_message { get; set; }
        public string raw_text_message { get; set; }
        public bool sent { get; set; }
        public DateTime? send_date { get; set; }
        public Guid alert_event_id { get; set; }
        public bool send_error { get; set; }
        [StringLength(255)]
        public string send_error_message { get; set; }

        [ForeignKey("alert_event_id")]
        //// [InverseProperty("AlertEmails")]
        public virtual AlertEvent alert_event { get; set; }
    }
}