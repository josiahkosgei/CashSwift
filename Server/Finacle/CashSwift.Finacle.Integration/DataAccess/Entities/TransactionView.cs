﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Keyless]
    public partial class TransactionView
    {
        [Column("Random-Number")]
        public int? Random_Number { get; set; }
        [Column("Start-Date")]
        public DateTime Start_Date { get; set; }
        [Column("End-Date")]
        public DateTime? End_Date { get; set; }
        [Required]
        [Column("Branch-Name")]
        [StringLength(50)]
        public string Branch_Name { get; set; }
        [Required]
        [Column("Device-Name")]
        [StringLength(50)]
        public string Device_Name { get; set; }
        [Required]
        [Column("Device-Location")]
        [StringLength(50)]
        public string Device_Location { get; set; }
        [Required]
        [Column("Transaction-Type-Name")]
        [StringLength(50)]
        public string Transaction_Type_Name { get; set; }
        [Column("CB-Transaction-Number")]
        [StringLength(50)]
        public string CB_Transaction_Number { get; set; }
        [StringLength(3)]
        [Unicode(false)]
        public string Currency { get; set; }
        [Column("Sub-Total")]
        public long? Sub_Total { get; set; }
        [Column("Denomination:-50")]
        public long Denomination__50 { get; set; }
        [Column("Denomination:-100")]
        public long Denomination__100 { get; set; }
        [Column("Denomination:-200")]
        public long Denomination__200 { get; set; }
        [Column("Denomination:-500")]
        public long Denomination__500 { get; set; }
        [Column("Denomination:-1000")]
        public long Denomination__1000 { get; set; }
        [Column("Account-Number")]
        [StringLength(50)]
        public string Account_Number { get; set; }
        [Column("Account-Name")]
        [StringLength(100)]
        public string Account_Name { get; set; }
        [Column("Reference-Account-Number")]
        [StringLength(50)]
        public string Reference_Account_Number { get; set; }
        [Column("Reference-Account-Name")]
        [StringLength(100)]
        public string Reference_Account_Name { get; set; }
        [StringLength(50)]
        public string Narration { get; set; }
        [Column("Depositor-Name")]
        [StringLength(50)]
        public string Depositor_Name { get; set; }
        [Column("ID-Number")]
        [StringLength(50)]
        public string ID_Number { get; set; }
        [Column("Phone-Number")]
        [StringLength(50)]
        public string Phone_Number { get; set; }
        public int Result { get; set; }
        [Column("Error-Code")]
        public int Error_Code { get; set; }
        [Column("Error-Message")]
        [StringLength(255)]
        public string Error_Message { get; set; }
        [Column("CB-Status")]
        [StringLength(50)]
        public string CB_Status { get; set; }
        [Column("Jam-Detected")]
        public bool Jam_Detected { get; set; }
        [Column("Transaction-ID")]
        public Guid Transaction_ID { get; set; }
        [Column("CIT-ID")]
        public Guid? CIT_ID { get; set; }
        [Column("Device-ID")]
        public Guid Device_ID { get; set; }
    }
}