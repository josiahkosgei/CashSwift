
//BusinessObjects.Authentication.CashSwift.WebUserPasswordHistory


using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [DomainComponent]
    [Browsable(false)]
    [FriendlyKeyProperty("user")]
    [DefaultProperty("datetime:g")]
    public class WebUserPasswordHistory : BaseObject
    {
        private string fuser;
        private DateTime fdatetime;
        private string fpassword;

        public WebUserPasswordHistory(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [ModelDefault("AllowEdit", "False")]
        public string user
        {
            get => fuser;
            set => SetPropertyValue(nameof(user), ref fuser, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime datetime
        {
            get => fdatetime;
            set => SetPropertyValue(nameof(datetime), ref fdatetime, value);
        }

        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        [PasswordPropertyText(true)]
        public string password
        {
            get => fpassword;
            set => SetPropertyValue(nameof(password), ref fpassword, value);
        }
    }
}
