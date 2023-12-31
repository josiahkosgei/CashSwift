﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("PostResponse", Schema = "cb")]
    [Index("RequestID", Name = "UX_PostResponse", IsUnique = true)]
    public partial class PostResponse
    {
        [Key]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Session { get; set; }
        public DateTime Created { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string RequestID { get; set; }
        public string Raw { get; set; }
    }
}