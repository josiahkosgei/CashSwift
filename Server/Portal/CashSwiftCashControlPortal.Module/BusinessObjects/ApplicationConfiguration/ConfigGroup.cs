
//BusinessObjects.ApplicationConfiguration.ConfigGroup


using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration
{
    [NavigationItem("Application Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class ConfigGroup : XPLiteObject, ITreeNode, ITreeNodeImageProvider
    {
        private int fid;
        private string fname;
        private string fdescription;
        private ConfigGroup fparent_group;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public int id
        {
            get => fid;
            set => SetPropertyValue<int>(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The name was already registered within the system.")]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Name")]
        [Persistent("name")]
        public string name
        {
            get => fname;
            set => SetPropertyValue(nameof(name), ref fname, value);
        }

        [Size(255)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [DisplayName("Description")]
        public string description
        {
            get => fdescription;
            set => SetPropertyValue(nameof(description), ref fdescription, value);
        }

        [Association("ConfigGroupReferencesConfigGroup")]
        public ConfigGroup parent_group
        {
            get => fparent_group;
            set => SetPropertyValue(nameof(parent_group), ref fparent_group, value);
        }

        [Association("ConfigGroupReferencesConfigGroup")]
        public XPCollection<ConfigGroup> ConfigGroupCollection => GetCollection<ConfigGroup>(nameof(ConfigGroupCollection));

        [Association("DeviceConfigReferencesConfigGroup")]
        public XPCollection<DeviceConfig> DeviceConfigs => GetCollection<DeviceConfig>(nameof(DeviceConfigs));

        [Association("DeviceReferencesConfigGroup")]
        public XPCollection<Device> Devices => GetCollection<Device>(nameof(Devices));

        public ConfigGroup(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        protected override void OnSaving()
        {
            base.OnSaving();
            IEnumerable<ConfigGroup> configGroups = new XPQuery<ConfigGroup>(Session).Select(c => c);
            Dictionary<int, ConfigGroup> lookup = configGroups.ToDictionary(type => type.id);
            if (HierarchyMethods.ContainsCycles(configGroups, type => lookup[type.parent_group.id]))
                throw new CashSwiftException("Looped reference detected in ConfigGroup hierarchy");
        }

        public IBindingList Children => ConfigGroupCollection;

        public string Name => name;

        public ITreeNode Parent => parent_group;

        public Image GetImage(out string imageName)
        {
            imageName = Children == null || Children.Count <= 0 ? "ModelEditor_Settings" : "BO_Category";
            return ImageLoader.Instance.GetImageInfo(imageName).Image;
        }
    }
}
