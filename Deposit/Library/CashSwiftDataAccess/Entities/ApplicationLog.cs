
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    /// <summary>
    /// Stores the general application log that the GUI and other local systems write to
    /// </summary>
    [Table("ApplicationLog")]
    public partial class ApplicationLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        /// <summary>
        /// The session this log entry belongs to
        /// </summary>
        public Guid? session_id { get; set; }
        public Guid device_id { get; set; }
        /// <summary>
        /// Datetime the system deems for the log entry.
        /// </summary>
        public DateTime log_date { get; set; }
        /// <summary>
        /// The name of the log event
        /// </summary>
        [Required]
        [StringLength(50)]
        public string event_name { get; set; }
        /// <summary>
        /// the details of the log message
        /// </summary>
        [Required]
        // [StringLength(250)]
        public string event_detail { get; set; }
        /// <summary>
        /// the type of the log event used for grouping and sorting
        /// </summary>
        [Required]
        [StringLength(50)]
        public string event_type { get; set; }
        /// <summary>
        /// Which internal component produced the log entry e.g. GUI, APIs, DeviceController etc
        /// </summary>
        [Required]
        [StringLength(100)]
        public string component { get; set; }
        /// <summary>
        /// the LogLevel
        /// </summary>
        public int log_level { get; set; }
        [Required]
        [StringLength(50)]
        public string machine_name { get; set; }
    }
}