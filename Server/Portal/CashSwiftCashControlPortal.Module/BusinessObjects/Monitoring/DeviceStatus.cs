using CashSwift.Library.Standard.Statuses;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Monitoring
{
    [NavigationItem("System Status")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [VisibleInReports]
    [VisibleInDashboards]
    [Appearance("ErrorLevel2", AppearanceItemType = "ViewItem", BackColor = "LightSalmon", Context = "ListView", Criteria = "ErrorLevel=2", FontColor = "Black", Priority = 1, TargetItems = "*")]
    [Appearance("ErrorLevel1", AppearanceItemType = "ViewItem", BackColor = "Orange", Context = "ListView", Criteria = "ErrorLevel=1", FontColor = "Black", Priority = 2, TargetItems = "*")]
    [Appearance("ErrorLevel0", AppearanceItemType = "ViewItem", BackColor = "LightGreen", Context = "ListView", Criteria = "ErrorLevel=0", FontColor = "Black", Priority = 0, TargetItems = "*")]
    public class DeviceStatus : XPLiteObject
    {
        private Guid fid;
        private Device fdevice_id;
        private string fmachine_name;
        private DateTime fmodified;
        private int fbag_percent_full;
        private string fbag_note_capacity;
        private string fcontroller_state;
        private string fba_type;
        private string fba_status;
        private string fba_currency;
        private string fbag_number;
        private string fbag_status;
        private int fbag_note_level;
        private long fbag_value_level;
        private long fbag_value_capacity;
        private string fsensors_type;
        private string fsensors_status;
        private int fsensors_value;
        private string fsensors_door;
        private string fsensors_bag;
        private string fescrow_type;
        private string fescrow_status;
        private string fescrow_position;
        private string ftransaction_status;
        private string ftransaction_type;
        private DateTime fmachine_datetime;
        private int fcurrent_status;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [NoForeignKey]
        [Association("DeviceStatusReferencesDevice")]
        [ModelDefault("AllowEdit", "False")]
        [Indexed(Name = "UX_device_id_DeviceStatus", Unique = true)]
        public Device device_id
        {
            get => fdevice_id;
            set => SetPropertyValue(nameof(device_id), ref fdevice_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(50)]
        [Persistent("machine_name")]
        public string MachineName
        {
            get => fmachine_name;
            set => SetPropertyValue(nameof(MachineName), ref fmachine_name, value?.ToUpperInvariant());
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime modified
        {
            get => fmodified;
            set => SetPropertyValue<DateTime>(nameof(modified), ref fmodified, value);
        }

        [NonPersistent]
        public string CurrentStatusString => ((DeviceState)current_status).ToString();

        [ModelDefault("AllowEdit", "False")]
        public int bag_percent_full
        {
            get => fbag_percent_full;
            set => SetPropertyValue<int>(nameof(bag_percent_full), ref fbag_percent_full, value);
        }

        [Size(10)]
        [ModelDefault("AllowEdit", "False")]
        public string bag_note_capacity
        {
            get => fbag_note_capacity;
            set => SetPropertyValue(nameof(bag_note_capacity), ref fbag_note_capacity, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string controller_state
        {
            get => fcontroller_state;
            set => SetPropertyValue(nameof(controller_state), ref fcontroller_state, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string ba_type
        {
            get => fba_type;
            set => SetPropertyValue(nameof(ba_type), ref fba_type, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string ba_status
        {
            get => fba_status;
            set => SetPropertyValue(nameof(ba_status), ref fba_status, value);
        }

        [Size(3)]
        [ModelDefault("AllowEdit", "False")]
        public string ba_currency
        {
            get => fba_currency;
            set => SetPropertyValue(nameof(ba_currency), ref fba_currency, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string bag_number
        {
            get => fbag_number;
            set => SetPropertyValue(nameof(bag_number), ref fbag_number, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string bag_status
        {
            get => fbag_status;
            set => SetPropertyValue(nameof(bag_status), ref fbag_status, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int bag_note_level
        {
            get => fbag_note_level;
            set => SetPropertyValue<int>(nameof(bag_note_level), ref fbag_note_level, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public long bag_value_level
        {
            get => fbag_value_level;
            set => SetPropertyValue(nameof(bag_value_level), ref fbag_value_level, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public long bag_value_capacity
        {
            get => fbag_value_capacity;
            set => SetPropertyValue(nameof(bag_value_capacity), ref fbag_value_capacity, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string sensors_type
        {
            get => fsensors_type;
            set => SetPropertyValue(nameof(sensors_type), ref fsensors_type, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string sensors_status
        {
            get => fsensors_status;
            set => SetPropertyValue(nameof(sensors_status), ref fsensors_status, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int sensors_value
        {
            get => fsensors_value;
            set => SetPropertyValue<int>(nameof(sensors_value), ref fsensors_value, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(20)]
        public string sensors_door
        {
            get => fsensors_door;
            set => SetPropertyValue(nameof(sensors_door), ref fsensors_door, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string sensors_bag
        {
            get => fsensors_bag;
            set => SetPropertyValue(nameof(sensors_bag), ref fsensors_bag, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(20)]
        public string escrow_type
        {
            get => fescrow_type;
            set => SetPropertyValue(nameof(escrow_type), ref fescrow_type, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string escrow_status
        {
            get => fescrow_status;
            set => SetPropertyValue(nameof(escrow_status), ref fescrow_status, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string escrow_position
        {
            get => fescrow_position;
            set => SetPropertyValue(nameof(escrow_position), ref fescrow_position, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string transaction_status
        {
            get => ftransaction_status;
            set => SetPropertyValue(nameof(transaction_status), ref ftransaction_status, value);
        }

        [Size(20)]
        [ModelDefault("AllowEdit", "False")]
        public string transaction_type
        {
            get => ftransaction_type;
            set => SetPropertyValue(nameof(transaction_type), ref ftransaction_type, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime machine_datetime
        {
            get => fmachine_datetime;
            set => SetPropertyValue<DateTime>(nameof(machine_datetime), ref fmachine_datetime, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int current_status
        {
            get => fcurrent_status;
            set => SetPropertyValue<int>(nameof(current_status), ref fcurrent_status, value);
        }

        public DeviceStatusLevel ErrorLevel
        {
            get
            {
                if (current_status != 0)
                    return DeviceStatusLevel.ERROR;
                return !(DateTime.Now.AddMinutes(-10.0) > modified) ? DeviceStatusLevel.OK : DeviceStatusLevel.WARNING;
            }
        }

        [NonPersistent]
        public double BagValueAmount => bag_value_level / 100.0;

        [NonPersistent]
        public double BagValueCapacityAmount => bag_value_capacity / 100.0;

        public DeviceStatus(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
