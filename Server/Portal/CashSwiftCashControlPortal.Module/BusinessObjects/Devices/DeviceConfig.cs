
//BusinessObjects.Devices.DeviceConfig


using CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Devices
{
    [NavigationItem("Device Management")]
    [FriendlyKeyProperty("config_id")]
    [DefaultProperty("group_id.name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class DeviceConfig : XPLiteObject
    {
        private Guid fid;
        private ConfigGroup fgroup_id;
        private Config fconfig_id;
        private string fvalue1;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("DeviceConfigReferencesConfigGroup")]
        [RuleRequiredField(DefaultContexts.Save)]
        public ConfigGroup group_id
        {
            get => fgroup_id;
            set => SetPropertyValue(nameof(group_id), ref fgroup_id, value);
        }

        [Indexed("group_id", Name = "UX_DeviceConfig", Unique = true)]
        [Size(50)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Association("DeviceConfigReferencesConfig")]
        public Config config_id
        {
            get => fconfig_id;
            set => SetPropertyValue(nameof(config_id), ref fconfig_id, value);
        }

        [Size(-1)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Persistent("config_value")]
        public string ConfigValue
        {
            get => fvalue1;
            set => SetPropertyValue(nameof(ConfigValue), ref fvalue1, value);
        }

        public DeviceConfig(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
