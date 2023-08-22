
using System;
using System.ComponentModel.DataAnnotations;


namespace CashSwiftDataAccess.Entities
{
    public partial class ThisDevice
    {
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string device_number { get; set; }
        [Required]
        [StringLength(50)]
        public string device_location { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(128)]
        public string machine_name { get; set; }
        public Guid branch_id { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public int type_id { get; set; }
        public bool enabled { get; set; }
        public int config_group { get; set; }
        public int? user_group { get; set; }
        public int GUIScreen_list { get; set; }
        public int? language_list { get; set; }
        public int currency_list { get; set; }
        public int transaction_type_list { get; set; }
        public int login_cycles { get; set; }
        public int login_attempts { get; set; }
    }
}