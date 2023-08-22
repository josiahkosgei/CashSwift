
//BusinessObjects.Transactions.TransactionPosting


using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.MakerChecker;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [NavigationItem("Transactions")]
    [FriendlyKeyProperty("initialising_user")]
    [DefaultProperty("dr_currency")]
    public class TransactionPosting : XPLiteObject, IMakerChecker
    {
        private Guid fid;
        private string fcb_tx_number;
        private Transaction ftx_id;
        private DateTime fpost_date;
        private string fdr_account;
        private string fdr_currency;
        private long fdr_amount;
        private string fcr_account;
        private string fcr_currency;
        private long fcr_amount;
        private string fnarration;
        private string ferror_code;
        private string ferror_message;
        private DateTime fcb_date;
        private string fcb_tx_status;
        private string fcb_status_detail;
        private bool fdevice_initiated;
        private TransactionPostingStatus fpost_status;
        private bool fis_complete;
        private ApplicationUser finitialising_user;
        private DateTime finit_date;
        private ApplicationUser fauthorising_user;
        private DateTime fauth_date;
        private MakerCheckerDecision? fauth_response;
        private string freason;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        [DisplayName("CB Reference")]
        public string cb_tx_number
        {
            get => fcb_tx_number;
            set => SetPropertyValue(nameof(cb_tx_number), ref fcb_tx_number, value);
        }

        [Association("TransactionPostingReferencesTransaction")]
        [DisplayName("Transaction")]
        [ModelDefault("AllowEdit", "False")]
        public Transaction tx_id
        {
            get => ftx_id;
            set => SetPropertyValue(nameof(tx_id), ref ftx_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Post Date")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime post_date
        {
            get => fpost_date;
            set => SetPropertyValue<DateTime>(nameof(post_date), ref fpost_date, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Debit Account")]
        [Size(50)]
        public string dr_account
        {
            get => fdr_account;
            set => SetPropertyValue(nameof(dr_account), ref fdr_account, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Debit Currency")]
        [Size(3)]
        public string dr_currency
        {
            get => fdr_currency;
            set => SetPropertyValue(nameof(dr_currency), ref fdr_currency, value);
        }

        [ModelDefault("DisplayFormat", "#,##0.##")]
        [DisplayName("Debit Amount")]
        [PersistentAlias("[tx_amount]/100.0")]
        public Decimal DRAmount => dr_amount / 100.0M;

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        [ModelDefault("DisplayFormat", "#,##0.##")]
        public long dr_amount
        {
            get => fdr_amount;
            set => SetPropertyValue(nameof(dr_amount), ref fdr_amount, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Credit Account")]
        [Size(50)]
        public string cr_account
        {
            get => fcr_account;
            set => SetPropertyValue(nameof(cr_account), ref fcr_account, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Credit Currency")]
        [Size(3)]
        public string cr_currency
        {
            get => fcr_currency;
            set => SetPropertyValue(nameof(cr_currency), ref fcr_currency, value);
        }

        [DisplayName("Credit Amount")]
        [ModelDefault("DisplayFormat", "#,##0.##")]
        [PersistentAlias("[tx_amount]/100.0")]
        public Decimal CRAmount => cr_amount / 100.0M;

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        [ModelDefault("DisplayFormat", "#,##0.##")]
        public long cr_amount
        {
            get => fcr_amount;
            set => SetPropertyValue(nameof(cr_amount), ref fcr_amount, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Narration")]
        [Size(100)]
        public string narration
        {
            get => fnarration;
            set => SetPropertyValue(nameof(narration), ref fnarration, value.Left(100));
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Error Code")]
        [Size(50)]
        public string error_code
        {
            get => ferror_code;
            set => SetPropertyValue(nameof(error_code), ref ferror_code, value != null ? value.Left(50) : null);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Error Message")]
        [Size(255)]
        public string error_message
        {
            get => ferror_message;
            set => SetPropertyValue(nameof(error_message), ref ferror_message, value.Left(254));
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("CB Date")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime cb_date
        {
            get => fcb_date;
            set => SetPropertyValue<DateTime>(nameof(cb_date), ref fcb_date, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("CB Transaction Status")]
        [Size(16)]
        public string cb_tx_status
        {
            get => fcb_tx_status;
            set => SetPropertyValue(nameof(cb_tx_status), ref fcb_tx_status, value.Left(16));
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("CB Status Detail")]
        [Size(100)]
        public string cb_status_detail
        {
            get => fcb_status_detail;
            set => SetPropertyValue(nameof(cb_status_detail), ref fcb_status_detail, value.Left(100));
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Device Initiated")]
        public bool device_initiated
        {
            get => fdevice_initiated;
            set => SetPropertyValue(nameof(device_initiated), ref fdevice_initiated, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Status")]
        public TransactionPostingStatus post_status
        {
            get => fpost_status;
            set => SetPropertyValue(nameof(post_status), ref fpost_status, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Complete")]
        public bool is_complete
        {
            get => fis_complete;
            set => SetPropertyValue(nameof(is_complete), ref fis_complete, value);
        }

        [Indexed(Name = "iinitialising_user_TransactionPosting")]
        [Association("TransactionPostingReferencesApplicationUser_Init")]
        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Initialising User")]
        public ApplicationUser initialising_user
        {
            get => finitialising_user;
            set => SetPropertyValue(nameof(initialising_user), ref finitialising_user, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Init Date")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime init_date
        {
            get => finit_date;
            set => SetPropertyValue<DateTime>(nameof(init_date), ref finit_date, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Association("TransactionPostingReferencesApplicationUser_Auth")]
        [Indexed(Name = "iauthorising_user_TransactionPosting")]
        [DisplayName("Authorising User")]
        public ApplicationUser authorising_user
        {
            get => fauthorising_user;
            set => SetPropertyValue(nameof(authorising_user), ref fauthorising_user, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Auth Date")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime auth_date
        {
            get => fauth_date;
            set => SetPropertyValue<DateTime>(nameof(auth_date), ref fauth_date, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Auth Response")]
        public MakerCheckerDecision? auth_response
        {
            get => fauth_response;
            set => SetPropertyValue(nameof(auth_response), ref fauth_response, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Reason")]
        [Size(100)]
        public string reason
        {
            get => freason;
            set => SetPropertyValue(nameof(reason), ref freason, value?.Trim());
        }

        public TransactionPosting(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
