
//BusinessObjects.CITs.CITTransaction


using CashSwift.Library.Standard.Utilities;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.CITs
{
    [FriendlyKeyProperty("KeyProperty")]
    [DefaultProperty("KeyProperty")]
    [NavigationItem("CIT Management")]
    public class CITTransaction : XPLiteObject
    {
        private Guid fid;
        private DateTime fdatetime;
        private CIT fcit_id;
        private string fcurrency;
        private long famount;
        private string fsuspense_account;
        private string faccount_number;
        private string fnarration;
        private string fcb_tx_number;
        private DateTime fcb_date;
        private string fcb_tx_status;
        private string fcb_status_detail;
        private int ferror_code;
        private string ferror_message;

        public string KeyProperty => string.Format("{0}:{1:yyyy-MM-dd HH:mm}", cit_id.device_id.name, Date);

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        [Persistent("datetime")]
        public DateTime Date
        {
            get => fdatetime;
            set => SetPropertyValue<DateTime>(nameof(Date), ref fdatetime, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Association("CITTransactionReferencesCIT")]
        public CIT cit_id
        {
            get => fcit_id;
            set => SetPropertyValue(nameof(cit_id), ref fcit_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(3)]
        public string currency
        {
            get => fcurrency;
            set => SetPropertyValue(nameof(currency), ref fcurrency, value);
        }

        [ModelDefault("DisplayFormat", "#,##0.##")]
        [PersistentAlias("[amount]/100.0")]
        public Decimal TotalAmount => amount / 100.0M;

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        public long amount
        {
            get => famount;
            set => SetPropertyValue(nameof(amount), ref famount, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string suspense_account
        {
            get => fsuspense_account;
            set => SetPropertyValue(nameof(suspense_account), ref fsuspense_account, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string account_number
        {
            get => faccount_number;
            set => SetPropertyValue(nameof(account_number), ref faccount_number, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string narration
        {
            get => fnarration;
            set => SetPropertyValue(nameof(narration), ref fnarration, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Transaction Reference")]
        public string cb_tx_number
        {
            get => fcb_tx_number;
            set => SetPropertyValue(nameof(cb_tx_number), ref fcb_tx_number, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime cb_date
        {
            get => fcb_date;
            set => SetPropertyValue<DateTime>(nameof(cb_date), ref fcb_date, value);
        }

        [Size(16)]
        [ModelDefault("AllowEdit", "False")]
        public string cb_tx_status
        {
            get => fcb_tx_status;
            set => SetPropertyValue(nameof(cb_tx_status), ref fcb_tx_status, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(-1)]
        public string cb_status_detail
        {
            get => fcb_status_detail;
            set => SetPropertyValue(nameof(cb_status_detail), ref fcb_status_detail, value);
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
            set => SetPropertyValue(nameof(error_message), ref ferror_message, value.Left(byte.MaxValue));
        }

        [Association("CITPostingReferencesCITTransaction")]
        public XPCollection<CITPosting> CITPostings => GetCollection<CITPosting>(nameof(CITPostings));

        public CITTransaction(Session session)
          : base(session)
        {
        }
    }
}
