﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    /// <summary>
    /// Types of messages for alerts sent via email or phone
    /// </summary>
    [Table("AlertMessageType")]
    public partial class AlertMessageType
    {
        public AlertMessageType()
        {
            AlertMessageRegistries = new HashSet<AlertMessageRegistry>();
        }

        [Key]
        public int id { get; set; }
        /// <summary>
        /// Name of the AlertMessage
        /// </summary>
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        /// <summary>
        /// Title displayed in th eheader sction of messages
        /// </summary>
        [StringLength(255)]
        public string title { get; set; }
        /// <summary>
        /// The HTML template that will be merged into later
        /// </summary>
        public string email_content_template { get; set; }
        /// <summary>
        /// The raw text template that will be merged into later
        /// </summary>
        public string raw_email_content_template { get; set; }
        /// <summary>
        /// The SMS template that will be merged into later
        /// </summary>
        [StringLength(255)]
        public string phone_content_template { get; set; }
        /// <summary>
        /// whether or not the alert message type in enabled and can be instantiated
        /// </summary>
        [Required]
        public bool? enabled { get; set; }

        [InverseProperty("alert_type")]
        public virtual ICollection<AlertMessageRegistry> AlertMessageRegistries { get; set; }
    }
}