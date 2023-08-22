
//BusinessObjects.Authentication.CashSwift.WebUserLoginCount


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [Browsable(false)]
    [FriendlyKeyProperty("User")]
    [DefaultProperty("modified:g")]
    public class WebUserLoginCount : BaseObject
    {
        private DateTime _modified;
        private int _loginCount;
        private string user;

        public WebUserLoginCount(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime modified
        {
            get => _modified;
            set => SetPropertyValue(nameof(modified), ref _modified, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int loginCount
        {
            get => _loginCount;
            set => SetPropertyValue(nameof(loginCount), ref _loginCount, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public string User
        {
            get => user;
            set
            {
                if (this.user == value)
                    return;
                string user = this.user;
                this.user = value;
                if (IsLoading)
                    return;
                OnChanged(nameof(User));
            }
        }

        public void Initialise(string username, int newLoginCount)
        {
            user = username;
            loginCount = newLoginCount;
            modified = DateTime.Now;
        }

        public void IncementLoginCount()
        {
            ++loginCount;
            modified = DateTime.Now;
        }

        public void ResetLoginCount()
        {
            loginCount = 0;
            modified = DateTime.Now;
        }
    }
}
