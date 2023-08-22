
//BusinessObjects.Authentication.CashSwift.ApplicationUserLoginDetail


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [FriendlyKeyProperty("User")]
    [DefaultProperty("User")]
    public class ApplicationUserLoginDetail : BaseObject
    {
        private ApplicationUser fUser;
        private DateTime fLastPasswordDate;
        private DateTime fLastLoginDate;
        private WebPortalLoginLogEntry fLastLoginLogEntry;
        private int fFailedLoginCount;
        private string fOTP;
        private DateTime fOTPExpire;
        private string fOTPEnabled;
        private string fResetEmailCode;
        private DateTime fResetEmailExpire = DateTime.MinValue;
        private bool fResetEmailEnabled;

        public ApplicationUserLoginDetail(Session session)
          : base(session)
        {
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser User
        {
            get => fUser;
            set
            {
                if (this.fUser == value)
                    return;
                ApplicationUser fUser = this.fUser;
                this.fUser = value;
                if (IsLoading)
                    return;
                if (fUser != null && fUser.ApplicationUserLoginDetail == this)
                    fUser.ApplicationUserLoginDetail =  null;
                if (this.fUser != null)
                    this.fUser.ApplicationUserLoginDetail = this;
                OnChanged(nameof(User));
            }
        }

        [DbType("datetime2")]
        [Size(7)]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime LastPasswordDate
        {
            get => fLastPasswordDate;
            set => SetPropertyValue(nameof(LastPasswordDate), ref fLastPasswordDate, value);
        }

        [DbType("datetime2")]
        [Size(7)]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime LastLoginDate
        {
            get => fLastLoginDate;
            set => SetPropertyValue(nameof(LastLoginDate), ref fLastLoginDate, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public WebPortalLoginLogEntry LastLoginLogEntry
        {
            get => fLastLoginLogEntry;
            set => SetPropertyValue(nameof(LastLoginLogEntry), ref fLastLoginLogEntry, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int FailedLoginCount
        {
            get => fFailedLoginCount;
            set => SetPropertyValue(nameof(FailedLoginCount), ref fFailedLoginCount, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        [PasswordPropertyText(true)]
        public string OTP
        {
            get => fOTP;
            set => SetPropertyValue(nameof(OTP), ref fOTP, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime OTPExpire
        {
            get => fOTPExpire;
            set => SetPropertyValue(nameof(OTPExpire), ref fOTPExpire, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DefaultValue(false)]
        public string OTPEnabled
        {
            get => fOTPEnabled;
            set => SetPropertyValue(nameof(OTPEnabled), ref fOTPEnabled, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        [Size(128)]
        [PasswordPropertyText(true)]
        public string ResetEmailCode
        {
            get => fResetEmailCode;
            set => SetPropertyValue(nameof(ResetEmailCode), ref fResetEmailCode, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime ResetEmailExpire
        {
            get => fResetEmailExpire;
            set => SetPropertyValue(nameof(ResetEmailExpire), ref fResetEmailExpire, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DefaultValue(true)]
        public bool ResetEmailEnabled
        {
            get => fResetEmailEnabled;
            set => SetPropertyValue(nameof(ResetEmailEnabled), ref fResetEmailEnabled, value);
        }

        [Association("ApplicationUserLoginDetail-WebPortalLogins")]
        public XPCollection<WebPortalLoginLogEntry> WebPortalLogEntries => GetCollection<WebPortalLoginLogEntry>(nameof(WebPortalLogEntries));

        [Association("ApplicationUserLoginDetail-UserLockEntries")]
        public XPCollection<UserLockLogEntry> UserLockEntries => GetCollection<UserLockLogEntry>(nameof(UserLockEntries));

        public override void AfterConstruction() => base.AfterConstruction();

        public void Login()
        {
            LastLoginDate = DateTime.Now;
            WebPortalLoginLogEntry newObject = new WebPortalLoginLogEntry(Session).Initialise(this, LastLoginDate, LoginAction.Login, true, User.IsActive, User.ChangePasswordOnFirstLogon);
            WebPortalLogEntries.Add(newObject);
            LastLoginLogEntry = newObject;
        }

        public bool IsValidLogin(string sessionID, string sfbegone) => !string.IsNullOrWhiteSpace(sessionID) && !string.IsNullOrWhiteSpace(sfbegone) && LastLoginLogEntry != null && string.Equals(sessionID, LastLoginLogEntry?.SessionID) && string.Equals(sfbegone, LastLoginLogEntry?.SFBegone) && LastLoginLogEntry?.Hash == LastLoginLogEntry?.CalculateLoginHash(sessionID, sfbegone);

        public void Logout()
        {
            WebPortalLogEntries.Add(new WebPortalLoginLogEntry(Session).Initialise(this, DateTime.Now, LoginAction.Logoff, true, User.IsActive, User.ChangePasswordOnFirstLogon));
            LastLoginLogEntry =  null;
        }
    }
}
