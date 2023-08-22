
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("AlertMessageRegistry")]
    public partial class AlertMessageRegistry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int alert_type_id { get; set; }
        public Guid role_id { get; set; }
        [Required]
        public bool? email_enabled { get; set; }
        public bool phone_enabled { get; set; }

        [ForeignKey("alert_type_id")]
        //// [InverseProperty("AlertMessageRegistries")]
        public virtual AlertMessageType alert_type { get; set; }
        [ForeignKey("role_id")]
        //// [InverseProperty("AlertMessageRegistries")]
        public virtual Role role { get; set; }
    }
}