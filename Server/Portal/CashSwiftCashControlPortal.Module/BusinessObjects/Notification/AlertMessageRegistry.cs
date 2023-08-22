
//BusinessObjects.Notification.AlertMessageRegistry



using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Notification
{
    [NavigationItem("Notification")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class AlertMessageRegistry : XPLiteObject
    {
        private Guid fid;
        private AlertMessageType falert_type_id;
        private DeviceRole frole_id;
        private bool femail_enabled = true;
        private bool fphone_enabled;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed("Role", Name = "UX_AlertMessageRegistry", Unique = true)]
        [Association("AlertMessageRegistryReferencesAlertMessageType")]
        [Persistent("alert_type_id")]
        [RuleRequiredField(DefaultContexts.Save)]
        public AlertMessageType AlertType
        {
            get => falert_type_id;
            set => SetPropertyValue(nameof(AlertType), ref falert_type_id, value);
        }

        [Association("AlertMessageRegistryReferencesRole")]
        [Persistent("role_id")]
        [RuleRequiredField(DefaultContexts.Save)]
        public DeviceRole Role
        {
            get => frole_id;
            set => SetPropertyValue(nameof(Role), ref frole_id, value);
        }

        [Persistent("email_enabled")]
        public bool EmailEnabled
        {
            get => femail_enabled;
            set => SetPropertyValue(nameof(EmailEnabled), ref femail_enabled, value);
        }

        [Persistent("phone_enabled")]
        public bool PhoneEnabled
        {
            get => fphone_enabled;
            set => SetPropertyValue(nameof(PhoneEnabled), ref fphone_enabled, value);
        }

        public AlertMessageRegistry(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
