﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    [Table("ApplicationUser")]
    [Index("username", Name = "UX_ApplicationUser_Username", IsUnique = true)]
    [Index("ApplicationUserLoginDetail", Name = "iApplicationUserLoginDetail_ApplicationUser")]
    [Index("role_id", Name = "irole_id_ApplicationUser")]
    [Index("user_group", Name = "iuser_group_ApplicationUser")]
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            ApplicationUserChangePasswords = new HashSet<ApplicationUserChangePassword>();
            ApplicationUserLoginDetails = new HashSet<ApplicationUserLoginDetail>();
            CITPostingauthorising_userNavigations = new HashSet<CITPosting>();
            CITPostinginitialising_userNavigations = new HashSet<CITPosting>();
            CITauth_userNavigations = new HashSet<CIT>();
            CITstart_userNavigations = new HashSet<CIT>();
            DeviceLocks = new HashSet<DeviceLock>();
            DeviceLogins = new HashSet<DeviceLogin>();
            EscrowJamauthorising_userNavigations = new HashSet<EscrowJam>();
            EscrowJaminitialising_userNavigations = new HashSet<EscrowJam>();
            PasswordHistories = new HashSet<PasswordHistory>();
            TransactionPostingauthorising_userNavigations = new HashSet<TransactionPosting>();
            TransactionPostinginitialising_userNavigations = new HashSet<TransactionPosting>();
            Transactionauth_userNavigations = new HashSet<Transaction>();
            Transactioninit_userNavigations = new HashSet<Transaction>();
            UserLocks = new HashSet<UserLock>();
            WebPortalRoleRoles_ApplicationUserApplicationUsers = new HashSet<WebPortalRoleRoles_ApplicationUserApplicationUser>();
        }

        [Key]
        public Guid id { get; set; }
        /// <summary>
        /// username for logging into the system
        /// </summary>
        [Required]
        [StringLength(255)]
        public string username { get; set; }
        /// <summary>
        /// salted and hashed password utilising a password library
        /// </summary>
        [Required]
        [StringLength(71)]
        [Unicode(false)]
        public string password { get; set; }
        /// <summary>
        /// First names
        /// </summary>
        [Required]
        [StringLength(50)]
        public string fname { get; set; }
        /// <summary>
        /// Last name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string lname { get; set; }
        /// <summary>
        /// The role the user has e.g. Custodian, Branch Manager tc
        /// </summary>
        public Guid role_id { get; set; }
        /// <summary>
        /// user email address, used to receive emails from the system
        /// </summary>
        [StringLength(50)]
        public string email { get; set; }
        /// <summary>
        /// whether or not the user is allowed to receive emails
        /// </summary>
        [Required]
        public bool? email_enabled { get; set; }
        /// <summary>
        /// the phone number for the user to rceive SMSes from the system
        /// </summary>
        [StringLength(50)]
        public string phone { get; set; }
        /// <summary>
        /// can the user receive SMSes from the system
        /// </summary>
        public bool phone_enabled { get; set; }
        /// <summary>
        /// should the user rset their password at their next login
        /// </summary>
        public bool password_reset_required { get; set; }
        /// <summary>
        /// how many unsuccessful login attempts has the user mad in a row. used to lock the user automatically
        /// </summary>
        public int login_attempts { get; set; }
        public int user_group { get; set; }
        public bool? depositor_enabled { get; set; }
        public bool? UserDeleted { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }
        public bool is_ad_user { get; set; }

        [ForeignKey("ApplicationUserLoginDetail")]
        [InverseProperty("ApplicationUsers")]
        public virtual ApplicationUserLoginDetail ApplicationUserLoginDetailNavigation { get; set; }
        [ForeignKey("role_id")]
        [InverseProperty("ApplicationUsers")]
        public virtual Role role { get; set; }
        [ForeignKey("user_group")]
        [InverseProperty("ApplicationUsers")]
        public virtual UserGroup user_groupNavigation { get; set; }
        [InverseProperty("UserNavigation")]
        public virtual ICollection<ApplicationUserChangePassword> ApplicationUserChangePasswords { get; set; }
        [InverseProperty("UserNavigation")]
        public virtual ICollection<ApplicationUserLoginDetail> ApplicationUserLoginDetails { get; set; }
        [InverseProperty("authorising_userNavigation")]
        public virtual ICollection<CITPosting> CITPostingauthorising_userNavigations { get; set; }
        [InverseProperty("initialising_userNavigation")]
        public virtual ICollection<CITPosting> CITPostinginitialising_userNavigations { get; set; }
        [InverseProperty("auth_userNavigation")]
        public virtual ICollection<CIT> CITauth_userNavigations { get; set; }
        [InverseProperty("start_userNavigation")]
        public virtual ICollection<CIT> CITstart_userNavigations { get; set; }
        [InverseProperty("locking_userNavigation")]
        public virtual ICollection<DeviceLock> DeviceLocks { get; set; }
        [InverseProperty("UserNavigation")]
        public virtual ICollection<DeviceLogin> DeviceLogins { get; set; }
        [InverseProperty("authorising_userNavigation")]
        public virtual ICollection<EscrowJam> EscrowJamauthorising_userNavigations { get; set; }
        [InverseProperty("initialising_userNavigation")]
        public virtual ICollection<EscrowJam> EscrowJaminitialising_userNavigations { get; set; }
        [InverseProperty("UserNavigation")]
        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; }
        [InverseProperty("authorising_userNavigation")]
        public virtual ICollection<TransactionPosting> TransactionPostingauthorising_userNavigations { get; set; }
        [InverseProperty("initialising_userNavigation")]
        public virtual ICollection<TransactionPosting> TransactionPostinginitialising_userNavigations { get; set; }
        [InverseProperty("auth_userNavigation")]
        public virtual ICollection<Transaction> Transactionauth_userNavigations { get; set; }
        [InverseProperty("init_userNavigation")]
        public virtual ICollection<Transaction> Transactioninit_userNavigations { get; set; }
        [InverseProperty("InitiatingUserNavigation")]
        public virtual ICollection<UserLock> UserLocks { get; set; }
        [InverseProperty("ApplicationUsersNavigation")]
        public virtual ICollection<WebPortalRoleRoles_ApplicationUserApplicationUser> WebPortalRoleRoles_ApplicationUserApplicationUsers { get; set; }
    }
}