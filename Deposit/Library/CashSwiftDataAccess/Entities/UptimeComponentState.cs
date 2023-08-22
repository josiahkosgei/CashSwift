
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("UptimeComponentState")]
    public partial class UptimeComponentState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid device { get; set; }
        public DateTime created { get; set; }
        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }
        public int component_state { get; set; }
    }
}