using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("AlertAttachmentType")]
    public partial class AlertAttachmentType
    {
        [Key]
        [StringLength(6)]
        public string code { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public int? alert_type_id { get; set; }
        public bool enabled { get; set; }
        [StringLength(100)]
        public string mime_type { get; set; }
        [StringLength(100)]
        public string mime_subtype { get; set; }
    }
}