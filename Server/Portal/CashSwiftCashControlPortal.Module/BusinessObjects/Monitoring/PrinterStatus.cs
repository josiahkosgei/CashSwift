using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Monitoring
{
    [FriendlyKeyProperty("printer_id.device_id")]
    [DefaultProperty("printer_id.name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class PrinterStatus : XPLiteObject
    {
        private Guid fid;
        private DevicePrinter fprinter_id;
        private string fmachine_name;
        private bool fis_error;
        private bool fhas_paper;
        private bool fcover_open;
        private int ferror_code;
        private string ferror_name;
        private string ferror_message;
        private DateTime fmodified;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [NoForeignKey]
        [Association("PrinterStatusReferencesDevicePrinter")]
        [ModelDefault("AllowEdit", "False")]
        public DevicePrinter printer_id
        {
            get => fprinter_id;
            set => SetPropertyValue(nameof(printer_id), ref fprinter_id, value);
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
        public bool is_error
        {
            get => fis_error;
            set => SetPropertyValue(nameof(is_error), ref fis_error, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool has_paper
        {
            get => fhas_paper;
            set => SetPropertyValue(nameof(has_paper), ref fhas_paper, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool cover_open
        {
            get => fcover_open;
            set => SetPropertyValue(nameof(cover_open), ref fcover_open, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int error_code
        {
            get => ferror_code;
            set => SetPropertyValue<int>(nameof(error_code), ref ferror_code, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(50)]
        public string error_name
        {
            get => ferror_name;
            set => SetPropertyValue(nameof(error_name), ref ferror_name, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(50)]
        public string error_message
        {
            get => ferror_message;
            set => SetPropertyValue(nameof(error_message), ref ferror_message, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime modified
        {
            get => fmodified;
            set => SetPropertyValue<DateTime>(nameof(modified), ref fmodified, value);
        }

        public PrinterStatus(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
