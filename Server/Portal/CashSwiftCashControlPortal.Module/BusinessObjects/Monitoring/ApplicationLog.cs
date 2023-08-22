
//BusinessObjects.Monitoring.ApplicationLog


using CashSwift.Library.Standard.Statuses;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Monitoring
{
    [FriendlyKeyProperty("device_id")]
    [DefaultProperty("event_name")]
    [Persistent("ApplicationLog")]
    [Appearance("log_level3", AppearanceItemType = "ViewItem", BackColor = "LightSalmon", Context = "ListView", Criteria = "log_level=3", FontColor = "Black", Priority = 2, TargetItems = "*")]
    [Appearance("log_level2", AppearanceItemType = "ViewItem", BackColor = "Orange", Context = "ListView", Criteria = "log_level=2", FontColor = "Black", Priority = 1, TargetItems = "*")]
    [Appearance("log_level0", AppearanceItemType = "ViewItem", BackColor = "Azure", Context = "ListView", Criteria = "log_level=0", FontColor = "Black", Priority = 0, TargetItems = "*")]
    public class ApplicationLog : XPLiteObject
    {
        private Guid fid;
        private DepositorSession fsession_id;
        private Device fdevice_id;
        private DateTime flog_date;
        private string fevent_name;
        private string fevent_detail;
        private string fevent_type;
        private string fcomponent;
        private ErrorLevel flog_level;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [NoForeignKey]
        [Association("ApplicationLogReferencesDepositorSession")]
        [ModelDefault("AllowEdit", "False")]
        public DepositorSession session_id
        {
            get => fsession_id;
            set => SetPropertyValue(nameof(session_id), ref fsession_id, value);
        }

        [NoForeignKey]
        [Association("ApplicationLogReferencesDevice")]
        [ModelDefault("AllowEdit", "False")]
        public Device device_id
        {
            get => fdevice_id;
            set => SetPropertyValue(nameof(device_id), ref fdevice_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime log_date
        {
            get => flog_date;
            set => SetPropertyValue<DateTime>(nameof(log_date), ref flog_date, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string event_name
        {
            get => fevent_name;
            set => SetPropertyValue(nameof(event_name), ref fevent_name, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(-1)]
        public string event_detail
        {
            get => fevent_detail;
            set => SetPropertyValue(nameof(event_detail), ref fevent_detail, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string event_type
        {
            get => fevent_type;
            set => SetPropertyValue(nameof(event_type), ref fevent_type, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string component
        {
            get => fcomponent;
            set => SetPropertyValue(nameof(component), ref fcomponent, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public ErrorLevel log_level
        {
            get => flog_level;
            set => SetPropertyValue(nameof(log_level), ref flog_level, value);
        }

        public ApplicationLog(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
