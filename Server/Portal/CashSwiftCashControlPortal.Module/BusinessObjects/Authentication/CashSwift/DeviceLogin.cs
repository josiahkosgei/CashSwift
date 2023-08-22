
//BusinessObjects.Authentication..DeviceLogin


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [NavigationItem("User Management")]
    [FriendlyKeyProperty("User")]
    [DefaultProperty("LogDate")]
    public class DeviceLogin : XPLiteObject
    {
        private Guid fid;
        private DateTime fLoginDate = DateTime.Now;
        private DateTime fLogoutDate = DateTime.Now;
        private ApplicationUser fUser;
        private bool fSuccess;
        private bool fDepositorEnabled;
        private bool fChangePassword;
        private string fMessage;
        private bool fForcedLogout;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [DbType("datetime2")]
        [Size(7)]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime LoginDate
        {
            get => fLoginDate;
            set => SetPropertyValue(nameof(LoginDate), ref fLoginDate, value);
        }

        [DbType("datetime2")]
        [Size(7)]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime LogoutDate
        {
            get => fLogoutDate;
            set => SetPropertyValue(nameof(LogoutDate), ref fLogoutDate, value);
        }

        [Association("DeviceLoginReferencesApplicationUser")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser User
        {
            get => fUser;
            set => SetPropertyValue(nameof(User), ref fUser, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool Success
        {
            get => fSuccess;
            set => SetPropertyValue(nameof(Success), ref fSuccess, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool DepositorEnabled
        {
            get => fDepositorEnabled;
            set => SetPropertyValue(nameof(DepositorEnabled), ref fDepositorEnabled, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool ChangePassword
        {
            get => fChangePassword;
            set => SetPropertyValue(nameof(ChangePassword), ref fChangePassword, value);
        }

        [Size(200)]
        [ModelDefault("AllowEdit", "False")]
        public string Message
        {
            get => fMessage;
            set => SetPropertyValue(nameof(Message), ref fMessage, new string(value != null ? value.Take(200).ToArray() : null));
        }

        [ModelDefault("AllowEdit", "False")]
        public bool ForcedLogout
        {
            get => fForcedLogout;
            set => SetPropertyValue(nameof(ForcedLogout), ref fForcedLogout, value);
        }

        public DeviceLogin(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
