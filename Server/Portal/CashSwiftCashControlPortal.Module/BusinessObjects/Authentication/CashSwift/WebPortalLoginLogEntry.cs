
//BusinessObjects.Authentication.CashSwift.WebPortalLoginLogEntry


using CashSwift.Library.Standard.Security;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Linq;
using System.Web;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [NavigationItem("User Management")]
    [FriendlyKeyProperty("ApplicationUserLoginDetail")]
    [DefaultProperty("LogDate")]
    [Persistent("WebPortalLogin")]
    public class WebPortalLoginLogEntry : XPLiteObject
    {
        private Guid fid;
        private DateTime fLogDate = DateTime.Now;
        private ApplicationUserLoginDetail fApplicationUserLoginDetail;
        private LoginAction fWebPortalLoginAction;
        private bool fSuccess;
        private bool fIsActive;
        private bool fChangePassword;
        private string fMessage;
        private string fSessionID;
        private string fSFBegone;
        private string fhash;

        public WebPortalLoginLogEntry(Session session)
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

        [Association("ApplicationUserLoginDetail-WebPortalLogins")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUserLoginDetail ApplicationUserLoginDetail
        {
            get => fApplicationUserLoginDetail;
            set => SetPropertyValue("User", ref fApplicationUserLoginDetail, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public LoginAction WebPortalLoginAction
        {
            get => fWebPortalLoginAction;
            set => SetPropertyValue(nameof(WebPortalLoginAction), ref fWebPortalLoginAction, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool Success
        {
            get => fSuccess;
            set => SetPropertyValue(nameof(Success), ref fSuccess, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool IsActive
        {
            get => fIsActive;
            set => SetPropertyValue(nameof(IsActive), ref fIsActive, value);
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

        [Browsable(false)]
        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        [Indexed("WebPortalLoginAction", Name = "iSessionID_WebPortalLogin", Unique = true)]
        public string SessionID
        {
            get => fSessionID;
            set => SetPropertyValue(nameof(SessionID), ref fSessionID, value);
        }

        [Browsable(false)]
        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string SFBegone
        {
            get => fSFBegone;
            set => SetPropertyValue(nameof(SFBegone), ref fSFBegone, value);
        }

        [Browsable(false)]
        [Size(128)]
        [ModelDefault("AllowEdit", "False")]
        public string Hash
        {
            get => fhash;
            set => SetPropertyValue(nameof(Hash), ref fhash, value);
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public WebPortalLoginLogEntry Initialise(
          ApplicationUserLoginDetail applicationUserLoginDetail,
          DateTime logDate,
          LoginAction webPortalLoginAction,
          bool success,
          bool isActive,
          bool changePassword,
          string message = null)
        {
            LogDate = logDate;
            WebPortalLoginAction = webPortalLoginAction;
            ApplicationUserLoginDetail = applicationUserLoginDetail;
            Success = success;
            IsActive = isActive;
            ChangePassword = changePassword;
            Message = message;
            SessionID = HttpContext.Current.Session.SessionID;
            SFBegone = Guid.NewGuid().ToString();
            Hash = CalculateLoginHash(HttpContext.Current.Session.SessionID, SFBegone);
            return this;
        }

        public string CalculateLoginHash(string sessionID, string sfbegone) => CashSwiftHashing.SHA256WithEncode(sessionID + sfbegone);
    }
}
