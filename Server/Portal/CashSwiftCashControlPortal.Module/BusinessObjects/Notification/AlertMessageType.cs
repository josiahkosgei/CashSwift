
//BusinessObjects.Notification.AlertMessageType



using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Notification
{
    [NavigationItem("Notification")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class AlertMessageType : XPLiteObject
    {
        private int fid;
        private string fname;
        private string fdescription;
        private string ftitle;
        private string femail_content_template;
        private string fraw_email_content_template;
        private string fphone_content_template;
        private bool fenabled;

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

        [Size(255)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string title
        {
            get => ftitle;
            set => SetPropertyValue(nameof(title), ref ftitle, value);
        }

        [Size(-1)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string email_content_template
        {
            get => femail_content_template;
            set => SetPropertyValue(nameof(email_content_template), ref femail_content_template, value);
        }

        [Size(-1)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string raw_email_content_template
        {
            get => fraw_email_content_template;
            set => SetPropertyValue(nameof(raw_email_content_template), ref fraw_email_content_template, value);
        }

        [Size(255)]
        public string phone_content_template
        {
            get => fphone_content_template;
            set => SetPropertyValue(nameof(phone_content_template), ref fphone_content_template, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [Association("AlertMessageRegistryReferencesAlertMessageType")]
        public XPCollection<AlertMessageRegistry> AlertMessageRegistries => GetCollection<AlertMessageRegistry>(nameof(AlertMessageRegistries));

        [ManyToManyAlias("AlertMessageRegistries", "Role")]
        public IList<DeviceRole> Roles => GetList<DeviceRole>(nameof(Roles));

        public AlertMessageType(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
