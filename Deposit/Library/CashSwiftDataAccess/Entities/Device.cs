
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CashSwiftDataAccess.Entities
{
    [Table("Device")]
    public partial class Device
    {
        public DeviceCITSuspenseAccount GetCITSuspenseAccount(string currency) => DeviceCITSuspenseAccounts.FirstOrDefault(x => x.Currency.code.Equals(currency, StringComparison.OrdinalIgnoreCase));

        public Device()
        {
            CITs = new HashSet<CIT>();
            DepositorSessions = new HashSet<DepositorSession>();
            DeviceCITSuspenseAccounts = new HashSet<DeviceCITSuspenseAccount>();
            DeviceLocks = new HashSet<DeviceLock>();
            DeviceLogins = new HashSet<DeviceLogin>();
            DevicePrinters = new HashSet<DevicePrinter>();
            DeviceStatuses = new HashSet<DeviceStatus>();
            DeviceSuspenseAccounts = new HashSet<DeviceSuspenseAccount>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [StringLength(17)]
        // // [Unicode(false)]
        public string mac_address { get; set; }
        public Guid app_id { get; set; }
        [Required]
        [MaxLength(32)]
        public byte[] app_key { get; set; }

        [ForeignKey("GUIScreen_list")]
        // [InverseProperty("Devices")]
        public virtual GUIScreenList GUIScreenList { get; set; }
        [ForeignKey("branch_id")]
        // [InverseProperty("Devices")]
        public virtual Branch Branch { get; set; }
        [ForeignKey("config_group")]
        // [InverseProperty("Devices")]
        public virtual ConfigGroup ConfigGroup { get; set; }
        [ForeignKey("currency_list")]
        // [InverseProperty("Devices")]
        public virtual CurrencyList CurrencyList { get; set; }
        [ForeignKey("language_list")]
        // [InverseProperty("Devices")]
        public virtual LanguageList LanguageList { get; set; }
        [ForeignKey("transaction_type_list")]
        // [InverseProperty("Devices")]
        public virtual TransactionTypeList TransactionTypeList { get; set; }
        [ForeignKey("type_id")]
        // [InverseProperty("Devices")]
        public virtual DeviceType DeviceType { get; set; }
        [ForeignKey("user_group")]
        // [InverseProperty("Devices")]
        public virtual UserGroup UserGroup { get; set; }
        // [InverseProperty("device")]
        public virtual ICollection<CIT> CITs { get; set; }
        // [InverseProperty("device")]
        public virtual ICollection<DepositorSession> DepositorSessions { get; set; }
        // [InverseProperty("device")]
        public virtual ICollection<DeviceCITSuspenseAccount> DeviceCITSuspenseAccounts { get; set; }
        // [InverseProperty("device")]
        public virtual ICollection<DeviceLock> DeviceLocks { get; set; }
        // [InverseProperty("device")]
        public virtual ICollection<DeviceLogin> DeviceLogins { get; set; }
        // [InverseProperty("device")]
        public virtual ICollection<DevicePrinter> DevicePrinters { get; set; }
        public virtual ICollection<DeviceStatus> DeviceStatuses { get; set; }
        // [InverseProperty("device")]
        public virtual ICollection<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }
        // [InverseProperty("device")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}