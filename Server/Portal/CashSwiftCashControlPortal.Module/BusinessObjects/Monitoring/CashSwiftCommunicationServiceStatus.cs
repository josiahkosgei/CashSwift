using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Monitoring
{
    [VisibleInReports]
    [VisibleInDashboards]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class CommunicationServiceStatus : XPLiteObject
    {
        private int fid;
        private int femail_status;
        private int femail_error;
        private string femail_error_message;
        private int fsms_status;
        private int fsms_error;
        private string fsms_error_message;
        private DateTime fmodified;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public int id
        {
            get => fid;
            set => SetPropertyValue<int>(nameof(id), ref fid, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int email_status
        {
            get => femail_status;
            set => SetPropertyValue<int>(nameof(email_status), ref femail_status, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int email_error
        {
            get => femail_error;
            set => SetPropertyValue<int>(nameof(email_error), ref femail_error, value);
        }

        [Size(-1)]
        [ModelDefault("AllowEdit", "False")]
        public string email_error_message
        {
            get => femail_error_message;
            set => SetPropertyValue(nameof(email_error_message), ref femail_error_message, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int sms_status
        {
            get => fsms_status;
            set => SetPropertyValue<int>(nameof(sms_status), ref fsms_status, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int sms_error
        {
            get => fsms_error;
            set => SetPropertyValue<int>(nameof(sms_error), ref fsms_error, value);
        }

        [Size(-1)]
        [ModelDefault("AllowEdit", "False")]
        public string sms_error_message
        {
            get => fsms_error_message;
            set => SetPropertyValue(nameof(sms_error_message), ref fsms_error_message, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime modified
        {
            get => fmodified;
            set => SetPropertyValue<DateTime>(nameof(modified), ref fmodified, value);
        }

        public CommunicationServiceStatus(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
