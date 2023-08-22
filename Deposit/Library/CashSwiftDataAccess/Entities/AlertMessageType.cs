using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("AlertMessageType")]
    public partial class AlertMessageType
    {
        public AlertMessageType()
        {
            AlertMessageRegistries = new HashSet<AlertMessageRegistry>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        [StringLength(255)]
        public string title { get; set; }
        public string email_content_template { get; set; }
        public string raw_email_content_template { get; set; }
        [StringLength(255)]
        public string phone_content_template { get; set; }
        [Required]
        public bool? enabled { get; set; }

        //// [InverseProperty("alert_type")]
        public virtual ICollection<AlertMessageRegistry> AlertMessageRegistries { get; set; }
    }
}