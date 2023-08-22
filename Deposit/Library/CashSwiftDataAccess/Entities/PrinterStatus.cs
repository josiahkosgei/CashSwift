
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashSwiftDataAccess.Entities
{
    public partial class PrinterStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid printer_id { get; set; }
        public bool is_error { get; set; }
        public bool has_paper { get; set; }
        public bool cover_open { get; set; }
        public int error_code { get; set; }
        [Required]
        [StringLength(50)]
        public string error_name { get; set; }
        [StringLength(50)]
        public string error_message { get; set; }
        public DateTime modified { get; set; }
        [Required]
        [StringLength(50)]
        public string machine_name { get; set; }
    }
}