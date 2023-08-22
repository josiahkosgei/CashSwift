
//BusinessObjects.Authentication..PasswordHistory


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [Browsable(false)]
    public class PasswordHistory : XPLiteObject
    {
        private Guid fid;
        private DateTime flogDate;
        private ApplicationUser fuser_id;
        private string fpassword;

        public PasswordHistory(Session session)
          : base(session)
        {
        }

        public PasswordHistory Initialise(
          ApplicationUser applicationUser,
          DateTime logDate,
          string password)
        {
            User = applicationUser != null ? applicationUser : throw new NullReferenceException("No user specified.");
            LogDate = logDate;
            Password = password;
            return this;
        }

        [Key(true)]
        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime LogDate
        {
            get => flogDate;
            set => SetPropertyValue<DateTime>("datetime", ref flogDate, value);
        }

        [Association("PasswordHistoryReferencesApplicationUser")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser User
        {
            get => fuser_id;
            set => SetPropertyValue("user_id", ref fuser_id, value);
        }

        [Size(71)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        [PasswordPropertyText(true)]
        public string Password
        {
            get => fpassword;
            set
            {
                SetPropertyValue(nameof(Password), ref fpassword, value);
                User.ApplicationUserLoginDetail.LastPasswordDate = LogDate;
            }
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
