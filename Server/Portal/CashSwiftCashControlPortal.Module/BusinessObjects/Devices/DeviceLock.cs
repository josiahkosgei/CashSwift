
//BusinessObjects.Devices.DeviceLock


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Devices
{
    [FriendlyKeyProperty("device_id.name")]
    [DefaultProperty("lock_date")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class DeviceLock : XPLiteObject
    {
        private Guid fid;
        private Device fdevice_id;
        private DateTime flock_date;
        private bool flocked;
        private ApplicationUser flocking_user;
        private string fweb_locking_user;
        private bool flocked_by_device;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("DeviceLockReferencesDevice")]
        [RuleRequiredField(DefaultContexts.Save)]
        public Device device_id
        {
            get => fdevice_id;
            set => SetPropertyValue(nameof(device_id), ref fdevice_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime lock_date
        {
            get => flock_date;
            set => SetPropertyValue<DateTime>(nameof(lock_date), ref flock_date, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool locked
        {
            get => flocked;
            set => SetPropertyValue(nameof(locked), ref flocked, value);
        }

        [Association("DeviceLockReferencesApplicationUser")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser locking_user
        {
            get => flocking_user;
            set => SetPropertyValue(nameof(locking_user), ref flocking_user, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string web_locking_user
        {
            get => fweb_locking_user;
            set => SetPropertyValue(nameof(web_locking_user), ref fweb_locking_user, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool locked_by_device
        {
            get => flocked_by_device;
            set => SetPropertyValue(nameof(locked_by_device), ref flocked_by_device, value);
        }

        public DeviceLock(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
