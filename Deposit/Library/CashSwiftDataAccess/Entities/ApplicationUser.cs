
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("ApplicationUser")]
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            CITAuthUsers = new HashSet<CIT>();
            CITStartUsers = new HashSet<CIT>();
            DeviceLocks = new HashSet<DeviceLock>();
            DeviceLogins = new HashSet<DeviceLogin>();
            EscrowJamAuthorisingUsers = new HashSet<EscrowJam>();
            EscrowJamInitialisingUsers = new HashSet<EscrowJam>();
            PasswordHistories = new HashSet<PasswordHistory>();
            TransactionAuthUsers = new HashSet<Transaction>();
            TransactionInitUsers = new HashSet<Transaction>();
            UserLocks = new HashSet<UserLock>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(255)]
        public string username { get; set; }
        [Required]
        [StringLength(71)]
        public string password { get; set; }
        [Required]
        [StringLength(50)]
        public string fname { get; set; }
        [Required]
        [StringLength(50)]
        public string lname { get; set; }
        public Guid role_id { get; set; }
        [StringLength(50)]
        public string email { get; set; }
        [Required]
        public bool? email_enabled { get; set; }
        [StringLength(50)]
        public string phone { get; set; }
        public bool phone_enabled { get; set; }
        public bool password_reset_required { get; set; }
        public int login_attempts { get; set; }
        public int user_group { get; set; }
        public bool? depositor_enabled { get; set; }
        public bool? UserDeleted { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }
        public bool is_ad_user { get; set; }

        [ForeignKey("role_id")]
        public virtual Role role { get; set; }
        [ForeignKey("user_group")]
        public virtual UserGroup UserGroup { get; set; }
        public virtual ICollection<CIT> CITAuthUsers { get; set; }
        public virtual ICollection<CIT> CITStartUsers { get; set; }
        public virtual ICollection<DeviceLock> DeviceLocks { get; set; }
        public virtual ICollection<DeviceLogin> DeviceLogins { get; set; }
        public virtual ICollection<EscrowJam> EscrowJamAuthorisingUsers { get; set; }
        public virtual ICollection<EscrowJam> EscrowJamInitialisingUsers { get; set; }
        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; }
        public virtual ICollection<Transaction> TransactionAuthUsers { get; set; }
        public virtual ICollection<Transaction> TransactionInitUsers { get; set; }
        public virtual ICollection<UserLock> UserLocks { get; set; }

        public string FullName => fname + " " + lname;
    }
}