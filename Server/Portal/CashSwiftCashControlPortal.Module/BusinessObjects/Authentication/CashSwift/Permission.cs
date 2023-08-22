
//BusinessObjects.Authentication..Permission


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [NavigationItem("User Management")]
    [FriendlyKeyProperty("Role")]
    [DefaultProperty("Activity")]
    public class Permission : XPLiteObject
    {
        private Guid fid;
        private DeviceRole frole_id;
        private Activity factivity_id;
        private bool fstandalone_allowed = true;
        private bool fstandalone_authentication_required;
        private bool fstandalone_can_authenticate;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed("Activity", Name = "UX_Permission", Unique = true)]
        [Association("PermissionReferencesRole")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Depsitor Role")]
        [Persistent("role_id")]
        public DeviceRole Role
        {
            get => frole_id;
            set => SetPropertyValue(nameof(Role), ref frole_id, value);
        }

        [Association("PermissionReferencesActivity")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Activity")]
        [Persistent("activity_id")]
        public Activity Activity
        {
            get => factivity_id;
            set => SetPropertyValue(nameof(Activity), ref factivity_id, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Allowed")]
        [Persistent("standalone_allowed")]
        public bool Allowed
        {
            get => fstandalone_allowed;
            set => SetPropertyValue(nameof(Allowed), ref fstandalone_allowed, value);
        }

        [Persistent("standalone_authentication_required")]
        public bool AuthenticationRequired
        {
            get => fstandalone_authentication_required;
            set => SetPropertyValue(nameof(AuthenticationRequired), ref fstandalone_authentication_required, value);
        }

        [Persistent("standalone_can_authenticate")]
        public bool CanAuthenticate
        {
            get => fstandalone_can_authenticate;
            set => SetPropertyValue(nameof(CanAuthenticate), ref fstandalone_can_authenticate, value);
        }

        public Permission(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
