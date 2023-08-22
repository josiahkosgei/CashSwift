
//BusinessObjects.Devices.DeviceType


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Devices
{
    [NavigationItem("Device Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [VisibleInReports]
    [VisibleInDashboards]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class DeviceType : XPLiteObject
    {
        private int fid;
        private string fname;
        private string fdescription;
        private bool fnote_in;
        private bool fnote_out;
        private bool fnote_escrow;
        private bool fcoin_in;
        private bool fcoin_out;
        private bool fcoin_escrow;

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

        public bool note_in
        {
            get => fnote_in;
            set => SetPropertyValue(nameof(note_in), ref fnote_in, value);
        }

        public bool note_out
        {
            get => fnote_out;
            set => SetPropertyValue(nameof(note_out), ref fnote_out, value);
        }

        public bool note_escrow
        {
            get => fnote_escrow;
            set => SetPropertyValue(nameof(note_escrow), ref fnote_escrow, value);
        }

        public bool coin_in
        {
            get => fcoin_in;
            set => SetPropertyValue(nameof(coin_in), ref fcoin_in, value);
        }

        public bool coin_out
        {
            get => fcoin_out;
            set => SetPropertyValue(nameof(coin_out), ref fcoin_out, value);
        }

        public bool coin_escrow
        {
            get => fcoin_escrow;
            set => SetPropertyValue(nameof(coin_escrow), ref fcoin_escrow, value);
        }

        [Association("DeviceReferencesDeviceType")]
        public XPCollection<Device> Devices => GetCollection<Device>(nameof(Devices));

        public DeviceType(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
