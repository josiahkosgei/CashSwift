﻿
//BusinessObjects.Devices.DeviceSuspenseAccount


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Devices
{
    [NavigationItem("Device Management")]
    [FriendlyKeyProperty("device_id.name")]
    [DefaultProperty("tx_type_listItem.name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class DeviceSuspenseAccount : XPLiteObject
    {
        private Guid fid;
        private Device fdevice_id;
        private ApplicationConfiguration.Currency fcurrency_code;
        private string faccount_number;
        private string faccount_name;
        private bool fenabled;
        private XPCollection<AuditDataItemPersistent> changeHistory;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed("currency_code", Name = "UX_Device_SuspenseAccount", Unique = true)]
        [Association("DeviceSuspenseAccountReferencesDevice")]
        [RuleRequiredField(DefaultContexts.Save)]
        public Device device_id
        {
            get => fdevice_id;
            set => SetPropertyValue(nameof(device_id), ref fdevice_id, value);
        }

        [Size(3)]
        [Association("DeviceSuspenseAccountReferencesCurrency")]
        [RuleRequiredField(DefaultContexts.Save)]
        public ApplicationConfiguration.Currency currency_code
        {
            get => fcurrency_code;
            set => SetPropertyValue(nameof(currency_code), ref fcurrency_code, value);
        }

        [Size(50)]
        [RuleRequiredField(DefaultContexts.Save)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string account_number
        {
            get => faccount_number;
            set => SetPropertyValue(nameof(account_number), ref faccount_number, value);
        }

        [Size(50)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string account_name
        {
            get => faccount_name;
            set => SetPropertyValue(nameof(account_name), ref faccount_name, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        public XPCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                    changeHistory = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                return changeHistory;
            }
        }

        public DeviceSuspenseAccount(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        protected override void OnSaved() => base.OnSaved();
    }
}
