﻿
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("SessionException", Schema = "exp")]
    public partial class SessionException
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime datetime { get; set; }
        public Guid session_id { get; set; }
        public int code { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        public int level { get; set; }
        [StringLength(255)]
        public string message { get; set; }
        [StringLength(255)]
        public string additional_info { get; set; }
        public string stack { get; set; }
        [Required]
        [StringLength(50)]
        public string machine_name { get; set; }
    }
}