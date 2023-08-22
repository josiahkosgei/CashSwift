
//BusinessObjects.Transactions.DepositorSession


using CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftCashControlPortal.Module.BusinessObjects.Monitoring;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [FriendlyKeyProperty("KeyProperty")]
    [DefaultProperty("KeyProperty")]
    [VisibleInReports]
    [VisibleInDashboards]
    public class DepositorSession : XPLiteObject
    {
        private Guid fid;
        private Device fdevice_id;
        private DateTime fsession_start;
        private DateTime fsession_end;
        private Language flanguage_code;
        private bool fcomplete;
        private bool fcomplete_success;
        private int ferror_code;
        private string ferror_message;
        private bool fterms_accepted;
        private bool faccount_verified;
        private bool freference_account_verified;

        public string KeyProperty => string.Format("{0}:{1:yyyy-MM-dd HH:mm}", device_id.name, session_start);

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("DepositorSessionReferencesDevice")]
        [ModelDefault("AllowEdit", "False")]
        public Device device_id
        {
            get => fdevice_id;
            set => SetPropertyValue(nameof(device_id), ref fdevice_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime session_start
        {
            get => fsession_start;
            set => SetPropertyValue<DateTime>(nameof(session_start), ref fsession_start, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime session_end
        {
            get => fsession_end;
            set => SetPropertyValue<DateTime>(nameof(session_end), ref fsession_end, value);
        }

        [Size(5)]
        [Association("DepositorSessionReferencesLanguage")]
        [ModelDefault("AllowEdit", "False")]
        public Language language_code
        {
            get => flanguage_code;
            set => SetPropertyValue(nameof(language_code), ref flanguage_code, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool complete
        {
            get => fcomplete;
            set => SetPropertyValue(nameof(complete), ref fcomplete, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool complete_success
        {
            get => fcomplete_success;
            set => SetPropertyValue(nameof(complete_success), ref fcomplete_success, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int error_code
        {
            get => ferror_code;
            set => SetPropertyValue<int>(nameof(error_code), ref ferror_code, value);
        }

        [Size(255)]
        [ModelDefault("AllowEdit", "False")]
        public string error_message
        {
            get => ferror_message;
            set => SetPropertyValue(nameof(error_message), ref ferror_message, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool terms_accepted
        {
            get => fterms_accepted;
            set => SetPropertyValue(nameof(terms_accepted), ref fterms_accepted, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool account_verified
        {
            get => faccount_verified;
            set => SetPropertyValue(nameof(account_verified), ref faccount_verified, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool reference_account_verified
        {
            get => freference_account_verified;
            set => SetPropertyValue(nameof(reference_account_verified), ref freference_account_verified, value);
        }

        [Association("TransactionReferencesDepositorSession")]
        public XPCollection<Transaction> Transactions => GetCollection<Transaction>(nameof(Transactions));

        [Association("ApplicationLogReferencesDepositorSession")]
        [NoForeignKey]
        public XPCollection<ApplicationLog> ApplicationLogs => GetCollection<ApplicationLog>(nameof(ApplicationLogs));

        public DepositorSession(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
