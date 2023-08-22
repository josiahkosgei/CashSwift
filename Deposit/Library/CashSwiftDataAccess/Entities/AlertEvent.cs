﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("AlertEvent")]
    public partial class AlertEvent
    {
        public AlertEvent()
        {
            AlertEmails = new HashSet<AlertEmail>();
            AlertSMSes = new HashSet<AlertSMS>();
            Inversealert_event = new HashSet<AlertEvent>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        public DateTime created { get; set; }
        public int alert_type_id { get; set; }
        public DateTime date_detected { get; set; }
        public DateTime? date_resolved { get; set; }
        public bool is_resolved { get; set; }
        public bool is_processed { get; set; }
        public Guid? alert_event_id { get; set; }
        public bool is_processing { get; set; }
        [Required]
        [StringLength(50)]
        public string machine_name { get; set; }

        [ForeignKey("alert_event_id")]
        // [InverseProperty("Inversealert_event")]
        public virtual AlertEvent alert_event { get; set; }
        // [InverseProperty("alert_event")]
        public virtual ICollection<AlertEmail> AlertEmails { get; set; }
        // [InverseProperty("alert_event")]
        public virtual ICollection<AlertSMS> AlertSMSes { get; set; }
        // [InverseProperty("alert_event")]
        public virtual ICollection<AlertEvent> Inversealert_event { get; set; }
    }
}