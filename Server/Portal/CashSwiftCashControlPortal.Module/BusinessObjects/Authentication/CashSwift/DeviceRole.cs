
//BusinessObjects.Authentication..DeviceRole


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Notification;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [NavigationItem("User Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    [Persistent("Role")]
    public class DeviceRole : XPLiteObject
    {
        private Guid fid;
        private string fname;
        private string fdescription;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
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

        [Association("AlertMessageRegistryReferencesRole")]
        public XPCollection<AlertMessageRegistry> AlertMessageRegistries => GetCollection<AlertMessageRegistry>(nameof(AlertMessageRegistries));

        [ManyToManyAlias("AlertMessageRegistries", "AlertType")]
        public IList<AlertMessageType> AlertMessages => GetList<AlertMessageType>(nameof(AlertMessages));

        [Association("PermissionReferencesRole")]
        public XPCollection<Permission> Permissions => GetCollection<Permission>(nameof(Permissions));

        [ManyToManyAlias("Permissions", "Activity")]
        public IList<Activity> Activities => GetList<Activity>(nameof(Activities));

        [Association("ApplicationUserReferencesRole")]
        public XPCollection<ApplicationUser> ApplicationUsers => GetCollection<ApplicationUser>(nameof(ApplicationUsers));

        public DeviceRole(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
