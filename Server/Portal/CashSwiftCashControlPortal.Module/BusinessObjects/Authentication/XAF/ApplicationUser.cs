
using CashSwiftCashControlPortal.Module.BusinessObjects.CITs;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftCashControlPortal.Module.BusinessObjects.Exceptions;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using CashSwiftCashControlPortal.Module.Controllers;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Linq;
using System.Text;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;
using CashSwift.Library.Standard.Security;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF
{
    [NavigationItem("User Management")]
    [FriendlyKeyProperty("FullName")]
    [DefaultProperty("username")]
    public class ApplicationUser :
      XPLiteObject,
      ISecurityUser,
      IAuthenticationStandardUser,
      ISecurityUserWithRoles,
      IPermissionPolicyUser
    {
        private Guid fid;
        private string ffname;
        private string flname;
        private DeviceRole fDeviceRole;
        private string femail;
        private bool femail_enabled;
        private string fphone;
        private bool fphone_enabled;
        private bool fdepositor_enabled;
        private int flogin_attempts;
        private UserGroup fuser_group;
        private bool fUserDeleted;
        private bool isActive;
        private string username = string.Empty;
        private bool changePasswordOnFirstLogon;
        private string storedPassword;
        private string _defaultPassword;
        private ApplicationUserLoginDetail fApplicationUserLoginDetail;
        private bool fIsActiveDirectoryUser;
        private XPCollection<AuditDataItemPersistent> changeHistory;

        public ApplicationUser(Session session)
          : base(session)
        {
            ApplicationUserLoginDetail = new ApplicationUserLoginDetail(session)
            {
                User = this
            };
        }

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("First Name")]
        [Persistent("fname")]
        public string fname
        {
            get => ffname;
            set => SetPropertyValue(nameof(fname), ref ffname, value);
        }

        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Last Name")]
        [Persistent("lname")]
        public string lname
        {
            get => flname;
            set => SetPropertyValue(nameof(lname), ref flname, value);
        }

        [Association("ApplicationUserReferencesRole")]
        [RuleRequiredField(DefaultContexts.Save)]
        [Persistent("role_id")]
        public DeviceRole DeviceRole
        {
            get => fDeviceRole;
            set => SetPropertyValue(nameof(DeviceRole), ref fDeviceRole, value);
        }

        [RuleRegularExpression("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$", CustomMessageTemplate = "Invalid email address.", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Email")]
        public string email
        {
            get => femail;
            set => SetPropertyValue(nameof(email), ref femail, value);
        }

        [DisplayName("Email Enabled")]
        public bool email_enabled
        {
            get => femail_enabled;
            set => SetPropertyValue(nameof(email_enabled), ref femail_enabled, value);
        }

        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid email address.")]
        [Size(50)]
        [DisplayName("Phone")]
        public string phone
        {
            get => fphone;
            set => SetPropertyValue(nameof(phone), ref fphone, value);
        }

        [DisplayName("Phone Enabled")]
        public bool phone_enabled
        {
            get => fphone_enabled;
            set => SetPropertyValue(nameof(phone_enabled), ref fphone_enabled, value);
        }

        [Persistent("depositor_enabled")]
        [DisplayName("Depositor Active")]
        [ModelDefault("AllowEdit", "False")]
        public bool DepositorEnabled
        {
            get => fdepositor_enabled;
            set => SetPropertyValue(nameof(DepositorEnabled), ref fdepositor_enabled, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Failed Login Attempts")]
        public int login_attempts
        {
            get => flogin_attempts;
            set => SetPropertyValue<int>(nameof(login_attempts), ref flogin_attempts, value);
        }

        [Association("ApplicationUserReferencesUserGroup")]
        [RuleRequiredField]
        [DisplayName("User Group")]
        public UserGroup user_group
        {
            get => fuser_group;
            set => SetPropertyValue(nameof(user_group), ref fuser_group, value);
        }

        public string FullName => string.Format("{0} {1}", fname ?? string.Empty, lname ?? string.Empty);

        [Browsable(false)]
        public bool UserDeleted
        {
            get => fUserDeleted;
            set => SetPropertyValue(nameof(UserDeleted), ref fUserDeleted, value);
        }

        [Browsable(false)]
        [NonPersistent]
        public string EncodedPassword => !string.IsNullOrWhiteSpace(storedPassword) ? CashSwiftHashing.EncodeTo64(storedPassword) : null;

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Portal Active")]
        public bool IsActive
        {
            get => isActive;
            set => SetPropertyValue(nameof(IsActive), ref isActive, value);
        }

        [RuleRequiredField("ApplicationUserNameRequired", DefaultContexts.Save)]
        [RuleUniqueValue("ApplicationUserNameIsUnique", DefaultContexts.Save, "The login with the entered username was already registered within the system.")]
        [Size(75)]
        [DisplayName("Username")]
        [Persistent("username")]
        public string UserName
        {
            get => username;
            set => SetPropertyValue(nameof(UserName), ref username, value);
        }

        [Persistent("password_reset_required")]
        public bool ChangePasswordOnFirstLogon
        {
            get => !IsActiveDirectoryUser && changePasswordOnFirstLogon;
            set => SetPropertyValue(nameof(ChangePasswordOnFirstLogon), ref changePasswordOnFirstLogon, value);
        }

        [Browsable(false)]
        [Size(71)]
        [Persistent("password")]
        [SecurityBrowsable]
        [PasswordPropertyText(true)]
        protected string StoredPassword
        {
            get => storedPassword;
            set => storedPassword = value;
        }

        public bool ComparePassword(string password) => PasswordStorage.VerifyPassword(password, storedPassword);

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AuthenticationException("Please enter a password");
            if (storedPassword == PasswordStorage.CreateHash(password))
                throw new AuthenticationException("New password must be different from old password");
            Session.Reload(this);
            PasswordPolicy passwordPolicy = GetPasswordPolicy();
            if (passwordPolicy == null)
                throw new ArgumentNullException("No PasswordPolicy defined. Contact Administrator");
            PasswordPolicyItems PasswordPolicy = new PasswordPolicyItems()
            {
                HistorySize = passwordPolicy.history_size,
                Lower_Case_length = passwordPolicy.min_lowercase,
                Minimum_Length = passwordPolicy.min_length,
                Numeric_length = passwordPolicy.min_digits,
                Special_length = passwordPolicy.min_special,
                Upper_Case_length = passwordPolicy.min_uppercase,
                Use_History = passwordPolicy.use_history
            };
            StringBuilder stringBuilder = new StringBuilder(100);
            IList<PasswordPolicyResult> passwordPolicyResultList = ValidatePassword(password, PasswordPolicy);
            if (passwordPolicyResultList == null)
            {
                StoredPassword = PasswordStorage.CreateHash(password);
                PasswordHistories.Add(new PasswordHistory(Session).Initialise(this, DateTime.Now, storedPassword));
                foreach (object theObject in PasswordHistories.OrderByDescending(c => c.LogDate).Skip(passwordPolicy.history_size))
                    Session.Delete(theObject);
                OnChanged("StoredPassword");
            }
            else
            {
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.MINIMUM_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must be at least {0:#} characters long", passwordPolicy.min_length));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.UPPER_CASE_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must have at least {0:#} UPPERCASE characters", passwordPolicy.min_uppercase));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.LOWER_CASE_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must have at least {0:#} lowercase characters", passwordPolicy.min_lowercase));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.SPECIAL_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must have at least {0:#} special characters", passwordPolicy.min_special));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.NUMERIC_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must have at least {0:#} numeric characters 1234567890", passwordPolicy.min_digits));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.HISTORY))
                    stringBuilder.AppendLine(string.Format("Cannot use one of the last {0:#} passwords", passwordPolicy.history_size));
                throw new Exception("Invalid Password " + Environment.NewLine + " " + stringBuilder.ToString());
            }
        }

        public IList<PasswordPolicyResult> ValidatePassword(
          string password,
          PasswordPolicyItems PasswordPolicy)
        {
            IList<PasswordPolicyResult> passwordPolicyResultList = PasswordPolicyManager.Validate(password, PasswordPolicy);
            if (passwordPolicyResultList == null && PasswordPolicy.Use_History)
            {
                foreach (PasswordHistory passwordHistory in PasswordHistories.OrderByDescending(x => x.LogDate).Take(PasswordPolicy.HistorySize))
                {
                    if (PasswordStorage.VerifyPassword(password, passwordHistory.Password))
                        passwordPolicyResultList =  new List<PasswordPolicyResult>()
            {
              PasswordPolicyResult.HISTORY
            };
                }
            }
            return passwordPolicyResultList;
        }

        public bool IsPasswordExpired() => ApplicationUserLoginDetail.LastPasswordDate < DateTime.Now.AddDays(-((double)GetPasswordPolicy()?.expiry_days));

        private PasswordPolicy GetPasswordPolicy()
        {
            try
            {
                return Session.FindObject(typeof(PasswordPolicy), null) as PasswordPolicy;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        IList<ISecurityRole> ISecurityUserWithRoles.Roles
        {
            get
            {
                IList<ISecurityRole> roles = new List<ISecurityRole>();
                foreach (WebPortalRole role in Roles)
                    roles.Add(role);
                return roles;
            }
        }

        [Association("ApplicationUser-WebPortalRole")]
        [RuleRequiredField("WebPortalRoleIsRequired", DefaultContexts.Save, CustomMessageTemplate = "An active user on the portal must have at least one web portal role assigned", TargetCriteria = "IsActive")]
        [DisplayName("Web Roles")]
        public XPCollection<WebPortalRole> Roles => GetCollection<WebPortalRole>(nameof(Roles));

        IEnumerable<IPermissionPolicyRole> IPermissionPolicyUser.Roles => Roles.OfType<IPermissionPolicyRole>();

        [NonPersistent]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public string DefaultPassword
        {
            get => _defaultPassword;
            set
            {
                Session session = Session;
                if ((session != null ? (session.IsNewObject(this) ? 1 : 0) : 0) != 0)
                    _defaultPassword = value;
                else if (value != null)
                    throw new InvalidOperationException("Illegal password access detected");
            }
        }

        [Browsable(false)]
        [Association("PasswordHistoryReferencesApplicationUser")]
        public XPCollection<PasswordHistory> PasswordHistories => GetCollection<PasswordHistory>(nameof(PasswordHistories));

        [Association("InitiatingUser-UserLockEntries")]
        public XPCollection<UserLockLogEntry> InitiatedUserLocks => GetCollection<UserLockLogEntry>(nameof(InitiatedUserLocks));

        [ModelDefault("AllowEdit", "False")]
        [RuleRequiredField]
        public ApplicationUserLoginDetail ApplicationUserLoginDetail
        {
            get => fApplicationUserLoginDetail;
            set
            {
                if (fApplicationUserLoginDetail == value)
                    return;
                ApplicationUserLoginDetail applicationUserLoginDetail = fApplicationUserLoginDetail;
                fApplicationUserLoginDetail = value;
                if (IsLoading)
                    return;
                if (applicationUserLoginDetail != null && applicationUserLoginDetail.User == this)
                    applicationUserLoginDetail.User =  null;
                if (fApplicationUserLoginDetail != null)
                    fApplicationUserLoginDetail.User = this;
                OnChanged(nameof(ApplicationUserLoginDetail));
            }
        }

        [Persistent("is_ad_user")]
        public bool IsActiveDirectoryUser
        {
            get => fIsActiveDirectoryUser;
            set => SetPropertyValue(nameof(IsActiveDirectoryUser), ref fIsActiveDirectoryUser, value);
        }

        [Association("ApplicationUser_AuthorisedCITs")]
        public XPCollection<CIT> AuthorisedCITs => GetCollection<CIT>(nameof(AuthorisedCITs));

        [Association("ApplicationUser_InitiatedCITs")]
        public XPCollection<CIT> InitiatedCITs => GetCollection<CIT>(nameof(InitiatedCITs));

        [Association("DeviceLockReferencesApplicationUser")]
        public XPCollection<DeviceLock> DeviceLocks => GetCollection<DeviceLock>(nameof(DeviceLocks));

        [Association("EscrowJamReferencesApplicationUser_authorising_user")]
        public XPCollection<EscrowJam> AuthorisedEscrowJams => GetCollection<EscrowJam>(nameof(AuthorisedEscrowJams));

        [Association("EscrowJamReferencesApplicationUser_initialising_user")]
        public XPCollection<EscrowJam> InitiatedEscrowJams => GetCollection<EscrowJam>(nameof(InitiatedEscrowJams));

        [Association("TransactionPostingReferencesApplicationUser_Auth")]
        public XPCollection<TransactionPosting> AuthorisedTransactionPosting => GetCollection<TransactionPosting>(nameof(AuthorisedTransactionPosting));

        [Association("TransactionPostingReferencesApplicationUser_Init")]
        public XPCollection<TransactionPosting> InitiatedTransactionPosting => GetCollection<TransactionPosting>(nameof(InitiatedTransactionPosting));

        [Association("DeviceLoginReferencesApplicationUser")]
        public XPCollection<DeviceLogin> DeviceLogins => GetCollection<DeviceLogin>(nameof(DeviceLogins));

        [Association("CITPostingReferencesApplicationUser_Auth")]
        public XPCollection<CITPosting> AuthorisedCITPostings => GetCollection<CITPosting>(nameof(AuthorisedCITPostings));

        [Association("CITPostingReferencesApplicationUser_Init")]
        public XPCollection<CITPosting> InitiatedCITPostings => GetCollection<CITPosting>(nameof(InitiatedCITPostings));

        [ModelDefault("AllowNew", "False")]
        [ModelDefault("AllowDelete", "False")]
        [ModelDefault("AllowLink", "False")]
        [ModelDefault("AllowUnlink", "False")]
        [ModelDefault("AllowEdit", "False")]
        [Association("TransactionReferencesApplicationUser_Auth")]
        public XPCollection<Transaction> AuthorisedDeposits => GetCollection<Transaction>(nameof(AuthorisedDeposits));

        [ModelDefault("AllowNew", "False")]
        [ModelDefault("AllowDelete", "False")]
        [ModelDefault("AllowLink", "False")]
        [ModelDefault("AllowUnlink", "False")]
        [ModelDefault("AllowEdit", "False")]
        [Association("TransactionReferencesApplicationUser_Init")]
        public XPCollection<Transaction> InitiatedDeposits => GetCollection<Transaction>(nameof(InitiatedDeposits));

        public XPCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                    changeHistory = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                return changeHistory;
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DefaultPassword = "password";
        }

        private void SetDefaultPassword()
        {
            PasswordPolicy passwordPolicy = GetPasswordPolicy();
            DefaultPassword = PasswordGenerator.Generate((uint)passwordPolicy.min_length, (uint)passwordPolicy.min_lowercase, (uint)passwordPolicy.min_uppercase, (uint)passwordPolicy.min_digits, (uint)passwordPolicy.min_special);
            ChangePasswordOnFirstLogon = true;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrWhiteSpace(DefaultPassword))
                return;
            if (Roles.Count() > 0)
                IsActive = true;
            DeviceRole deviceRole = DeviceRole;
            if ((deviceRole != null ? (deviceRole.Permissions.Count() > 0 ? 1 : 0) : 0) != 0)
                DepositorEnabled = true;
            SetDefaultPassword();
            SetPassword(DefaultPassword);
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            if (string.IsNullOrWhiteSpace(DefaultPassword))
                return;
            new EmailManager(Session).SendNewUserEmail(this, DefaultPassword ?? "Password error detected. Contact administrator");
        }

        public bool LockUser(bool webPortalInitiated, ApplicationUser initiatingUser = null)
        {
            try
            {
                if (initiatingUser?.UserName == UserName)
                    throw new ArgumentException("Cannot perform operation on current user");
                IsActive = false;
                DepositorEnabled = false;
                ApplicationUserLoginDetail.UserLockEntries.Add(new UserLockLogEntry(Session).Initialise(DateTime.Now, ApplicationUserLoginDetail, CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift.UserLockType.Lock, webPortalInitiated, initiatingUser));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UnlockUser(bool webPortalInitiated, ApplicationUser initiatingUser = null)
        {
            try
            {
                if (initiatingUser.UserName == UserName)
                    throw new ArgumentException("Cannot perform operation on current user");
                ResetLoginCount();
                IsActive = true;
                ApplicationUserLoginDetail.UserLockEntries.Add(new UserLockLogEntry(Session).Initialise(DateTime.Now, ApplicationUserLoginDetail, CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift.UserLockType.Unlock, webPortalInitiated, initiatingUser));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UnlockUserOnDepositor(bool webPortalInitiated, ApplicationUser initiatingUser = null)
        {
            try
            {
                if (initiatingUser.UserName == UserName)
                    throw new ArgumentException("Cannot perform operation on current user");
                ResetLoginCount();
                DepositorEnabled = true;
                ApplicationUserLoginDetail.UserLockEntries.Add(new UserLockLogEntry(Session).Initialise(DateTime.Now, ApplicationUserLoginDetail, CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift.UserLockType.UnlockOnDepositor, webPortalInitiated, initiatingUser));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteUser(ApplicationUser initiatingUser)
        {
            try
            {
                if (initiatingUser.UserName == UserName)
                    throw new ArgumentException("Cannot perform operation on current user");
                if (initiatingUser == null)
                    throw new NullReferenceException(nameof(initiatingUser));
                ResetLoginCount();
                UserDeleted = true;
                IsActive = false;
                DepositorEnabled = false;
                phone_enabled = false;
                email_enabled = false;
                ApplicationUserLoginDetail.UserLockEntries.Add(new UserLockLogEntry(Session).Initialise(DateTime.Now, ApplicationUserLoginDetail, CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift.UserLockType.Disable, false, initiatingUser));
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UnDeleteUser(ApplicationUser initiatingUser)
        {
            try
            {
                if (initiatingUser == null)
                    throw new NullReferenceException(nameof(initiatingUser));
                if (initiatingUser.UserName == UserName)
                    throw new ArgumentException("Cannot perform operation on current user");
                ResetLoginCount();
                UserDeleted = false;
                IsActive = false;
                DepositorEnabled = false;
                ApplicationUserLoginDetail.UserLockEntries.Add(new UserLockLogEntry(Session).Initialise(DateTime.Now, ApplicationUserLoginDetail, CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift.UserLockType.Enable, false, initiatingUser));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ResetLoginCount()
        {
            ApplicationUserLoginDetail.FailedLoginCount = 0;
            login_attempts = 0;
        }

        public void Login()
        {
            try
            {
                ApplicationUserLoginDetail.Login();
            }
            catch (Exception ex)
            {
            }
        }

        public void LogOff()
        {
            try
            {
                ApplicationUserLoginDetail.Logout();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
