﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("SessionException", Schema = "exp")]
    [Index("session_id", Name = "isession_id_exp_SessionException_E2E5599E")]
    public partial class SessionException
    {
        [Key]
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