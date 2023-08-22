
//BusinessObjects.CITs.CIT


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.CITs
{
    [VisibleInReports]
    [VisibleInDashboards]
    [NavigationItem("CIT Management")]
    [FriendlyKeyProperty("KeyProperty")]
    [DefaultProperty("KeyProperty")]
    [Persistent("CIT")]
    public class CIT : XPLiteObject
    {
        private Guid fid;
        private Device fdevice_id;
        private DateTime fcit_date;
        private DateTime fcit_complete_date;
        private DateTime ffromDate;
        private DateTime ftoDate;
        private ApplicationUser fstart_user;
        private ApplicationUser fauth_user;
        private string fold_bag_number;
        private string fnew_bag_number;
        private string fseal_number;
        private bool fcomplete;
        private int fcit_error;
        private string fcit_error_message;

        public string KeyProperty => string.Format("{0}:{1:yyyy-MM-dd HH:mm}", device_id.name, cit_date);

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("CITReferencesDevice")]
        [ModelDefault("AllowEdit", "False")]
        public Device device_id
        {
            get => fdevice_id;
            set => SetPropertyValue(nameof(device_id), ref fdevice_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime cit_date
        {
            get => fcit_date;
            set => SetPropertyValue<DateTime>(nameof(cit_date), ref fcit_date, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime cit_complete_date
        {
            get => fcit_complete_date;
            set => SetPropertyValue<DateTime>(nameof(cit_complete_date), ref fcit_complete_date, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime fromDate
        {
            get => ffromDate;
            set => SetPropertyValue<DateTime>(nameof(fromDate), ref ffromDate, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime toDate
        {
            get => ftoDate;
            set => SetPropertyValue<DateTime>(nameof(toDate), ref ftoDate, value);
        }

        [Association("ApplicationUser_InitiatedCITs")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser start_user
        {
            get => fstart_user;
            set => SetPropertyValue(nameof(start_user), ref fstart_user, value);
        }

        [Association("ApplicationUser_AuthorisedCITs")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser auth_user
        {
            get => fauth_user;
            set => SetPropertyValue(nameof(auth_user), ref fauth_user, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string old_bag_number
        {
            get => fold_bag_number;
            set => SetPropertyValue(nameof(old_bag_number), ref fold_bag_number, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string new_bag_number
        {
            get => fnew_bag_number;
            set => SetPropertyValue(nameof(new_bag_number), ref fnew_bag_number, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string seal_number
        {
            get => fseal_number;
            set => SetPropertyValue(nameof(seal_number), ref fseal_number, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool complete
        {
            get => fcomplete;
            set => SetPropertyValue(nameof(complete), ref fcomplete, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int cit_error
        {
            get => fcit_error;
            set => SetPropertyValue<int>(nameof(cit_error), ref fcit_error, value);
        }

        [Size(255)]
        [ModelDefault("AllowEdit", "False")]
        public string cit_error_message
        {
            get => fcit_error_message;
            set => SetPropertyValue(nameof(cit_error_message), ref fcit_error_message, value);
        }

        [Association("CITDenominationsReferencesCIT")]
        public XPCollection<CITDenominations> CITDenominationsCollection => GetCollection<CITDenominations>(nameof(CITDenominationsCollection));

        [Association("TransactionReferencesCIT")]
        public XPCollection<Transaction> Transactions => GetCollection<Transaction>(nameof(Transactions));

        [Association("CITPrintoutReferencesCIT")]
        public XPCollection<CITPrintout> CITPrintouts => GetCollection<CITPrintout>(nameof(CITPrintouts));

        [Association("CITTransactionReferencesCIT")]
        public XPCollection<CITTransaction> CITTransactions => GetCollection<CITTransaction>(nameof(CITTransactions));

        public CIT(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
