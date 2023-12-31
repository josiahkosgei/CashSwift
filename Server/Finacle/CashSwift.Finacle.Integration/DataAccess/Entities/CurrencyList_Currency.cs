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
    /// [m2m] Currency and CurrencyList
    /// </summary>
    [Table("CurrencyList_Currency")]
    [Index("currency_list", "currency_item", Name = "UX_CurrencyList_Currency_CurrencyItem", IsUnique = true)]
    [Index("currency_list", "currency_order", Name = "UX_Currency_CurrencyList_Order", IsUnique = true)]
    [Index("currency_item", Name = "icurrency_item_CurrencyList_Currency")]
    [Index("currency_list", Name = "icurrency_list_CurrencyList_Currency")]
    public partial class CurrencyList_Currency
    {
        [Key]
        public Guid id { get; set; }
        /// <summary>
        /// The Currency list to which the currency is associated
        /// </summary>
        public int currency_list { get; set; }
        /// <summary>
        /// The currency in the list
        /// </summary>
        [Required]
        [StringLength(3)]
        [Unicode(false)]
        public string currency_item { get; set; }
        /// <summary>
        /// ASC Order of sorting for currencies in list.
        /// </summary>
        public int currency_order { get; set; }
        public long max_value { get; set; }
        public int max_count { get; set; }

        [ForeignKey("currency_item")]
        [InverseProperty("CurrencyList_Currencies")]
        public virtual Currency currency_itemNavigation { get; set; }
        [ForeignKey("currency_list")]
        [InverseProperty("CurrencyList_Currencies")]
        public virtual CurrencyList currency_listNavigation { get; set; }
    }
}