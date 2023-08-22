
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("DeviceSuspenseAccount")]
    public partial class DeviceSuspenseAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        [Required]
        [StringLength(3)]
        // [Unicode(false)]
        public string currency_code { get; set; }
        [Required]
        [StringLength(50)]
        public string account_number { get; set; }
        [StringLength(100)]
        public string account_name { get; set; }
        public bool enabled { get; set; }
        public Guid? account { get; set; }

        [ForeignKey("currency_code")]
        public virtual Currency currency_codeNavigation { get; set; }
        [ForeignKey("device_id")]
        public virtual Device device { get; set; }
    }
}