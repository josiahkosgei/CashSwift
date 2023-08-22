
//BusinessObjects.Authentication..UserLockLogEntry


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [NavigationItem("User Management")]
    [FriendlyKeyProperty("FullName")]
    [DefaultProperty("username")]
    [Persistent("UserLock")]
    public class UserLockLogEntry : XPLiteObject
    {
        private Guid fid;
        private DateTime fLogDate = DateTime.Now;
        private ApplicationUserLoginDetail fApplicationUserLoginDetail;
        private UserLockType fLockType;
        private bool fWebPortalInitiated;
        private ApplicationUser fInitiatingUser;

        public UserLockLogEntry(Session session)
          : base(session)
        {
        }

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
        public DateTime LogDate
        {
            get => fLogDate;
            set => SetPropertyValue(nameof(LogDate), ref fLogDate, value);
        }

        [Association("ApplicationUserLoginDetail-UserLockEntries")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUserLoginDetail ApplicationUserLoginDetail
        {
            get => fApplicationUserLoginDetail;
            set => SetPropertyValue("User", ref fApplicationUserLoginDetail, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public UserLockType LockType
        {
            get => fLockType;
            set => SetPropertyValue(nameof(LockType), ref fLockType, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool WebPortalInitiated
        {
            get => fWebPortalInitiated;
            set => SetPropertyValue(nameof(WebPortalInitiated), ref fWebPortalInitiated, value);
        }

        [Association("InitiatingUser-UserLockEntries")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser InitiatingUser
        {
            get => fInitiatingUser;
            set => SetPropertyValue(nameof(InitiatingUser), ref fInitiatingUser, value);
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public UserLockLogEntry Initialise(
          DateTime logDate,
          ApplicationUserLoginDetail applicationUserLoginDetail,
          UserLockType lockType,
          bool webPortalInitiated,
          ApplicationUser initiatingUser)
        {
            LogDate = logDate;
            ApplicationUserLoginDetail = applicationUserLoginDetail;
            LockType = lockType;
            WebPortalInitiated = webPortalInitiated;
            InitiatingUser = initiatingUser;
            return this;
        }
    }
}
