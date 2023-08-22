
//BusinessObjects.Transactions.Transaction


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.CITs;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftCashControlPortal.Module.BusinessObjects.Exceptions;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [NavigationItem("Transactions")]
    [FriendlyKeyProperty("KeyProperty")]
    [DefaultProperty("KeyProperty")]
    [VisibleInReports]
    [VisibleInDashboards]
    public class Transaction : XPLiteObject
    {
        private Guid fid;
        private DateTime ftx_start_date;
        private DateTime ftx_end_date;
        private string fcb_tx_number;
        private Device fdevice_id;
        private TransactionTypeListItem ftx_type;
        private ApplicationConfiguration.Currency ftx_currency;
        private string ftx_account_number;
        private string fcb_account_name;
        private string ftx_ref_account;
        private string fcb_ref_account_name;
        private string ftx_narration;
        private string ftx_depositor_name;
        private string ftx_id_number;
        private string ftx_phone;
        private DepositorSession fsession_id;
        private int ftx_random_number;
        private bool ftx_completed;
        private long ftx_amount;
        private string ffunds_source;
        private int ftx_result;
        private int ftx_error_code;
        private string ftx_error_message;
        private DateTime fcb_date;
        private string fcb_tx_status;
        private string fcb_status_detail;
        private bool fnotes_rejected;
        private bool fjam_detected;
        private bool fescrow_jam_detected;
        private CIT fcit_id;
        private string ftx_suspense_account;
        private ApplicationUser finit_user;
        private ApplicationUser fauth_user;

        public string KeyProperty => string.Format("{0}:{1:yyyy-MM-dd HH:mm}", device_id.name, tx_start_date);

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
        public DateTime tx_start_date
        {
            get => ftx_start_date;
            set => SetPropertyValue<DateTime>(nameof(tx_start_date), ref ftx_start_date, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime tx_end_date
        {
            get => ftx_end_date;
            set => SetPropertyValue<DateTime>(nameof(tx_end_date), ref ftx_end_date, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Transaction Reference")]
        public string cb_tx_number
        {
            get => fcb_tx_number;
            set => SetPropertyValue(nameof(cb_tx_number), ref fcb_tx_number, value);
        }

        [Association("TransactionReferencesDevice")]
        [ModelDefault("AllowEdit", "False")]
        public Device device_id
        {
            get => fdevice_id;
            set => SetPropertyValue(nameof(device_id), ref fdevice_id, value);
        }

        [Association("TransactionReferencesTransactionTypeListItem")]
        [ModelDefault("AllowEdit", "False")]
        public TransactionTypeListItem tx_type
        {
            get => ftx_type;
            set => SetPropertyValue(nameof(tx_type), ref ftx_type, value);
        }

        [Size(3)]
        [Association("TransactionReferencesCurrency")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationConfiguration.Currency tx_currency
        {
            get => ftx_currency;
            set => SetPropertyValue(nameof(tx_currency), ref ftx_currency, value);
        }

        [ModelDefault("DisplayFormat", "#,##0.##")]
        [PersistentAlias("[tx_amount]/100.0")]
        public Decimal TotalAmount => tx_amount / 100.0M;

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string tx_account_number
        {
            get => ftx_account_number;
            set => SetPropertyValue(nameof(tx_account_number), ref ftx_account_number, value);
        }

        [Size(100)]
        [ModelDefault("AllowEdit", "False")]
        public string cb_account_name
        {
            get => fcb_account_name;
            set => SetPropertyValue(nameof(cb_account_name), ref fcb_account_name, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string tx_ref_account
        {
            get => ftx_ref_account;
            set => SetPropertyValue(nameof(tx_ref_account), ref ftx_ref_account, value);
        }

        [Size(100)]
        [ModelDefault("AllowEdit", "False")]
        public string cb_ref_account_name
        {
            get => fcb_ref_account_name;
            set => SetPropertyValue(nameof(cb_ref_account_name), ref fcb_ref_account_name, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string tx_narration
        {
            get => ftx_narration;
            set => SetPropertyValue(nameof(tx_narration), ref ftx_narration, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string tx_depositor_name
        {
            get => ftx_depositor_name;
            set => SetPropertyValue(nameof(tx_depositor_name), ref ftx_depositor_name, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(50)]
        public string tx_id_number
        {
            get => ftx_id_number;
            set => SetPropertyValue(nameof(tx_id_number), ref ftx_id_number, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        public string tx_phone
        {
            get => ftx_phone;
            set => SetPropertyValue(nameof(tx_phone), ref ftx_phone, value);
        }

        [Association("TransactionReferencesDepositorSession")]
        [ModelDefault("AllowEdit", "False")]
        public DepositorSession session_id
        {
            get => fsession_id;
            set => SetPropertyValue(nameof(session_id), ref fsession_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "0")]
        [DisplayName("Transaction Number")]
        public int tx_random_number
        {
            get => ftx_random_number;
            set => SetPropertyValue<int>(nameof(tx_random_number), ref ftx_random_number, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool tx_completed
        {
            get => ftx_completed;
            set => SetPropertyValue(nameof(tx_completed), ref ftx_completed, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        public long tx_amount
        {
            get => ftx_amount;
            set => SetPropertyValue(nameof(tx_amount), ref ftx_amount, value);
        }

        [Size(255)]
        [ModelDefault("AllowEdit", "False")]
        public string funds_source
        {
            get => ffunds_source;
            set => SetPropertyValue(nameof(funds_source), ref ffunds_source, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int tx_result
        {
            get => ftx_result;
            set => SetPropertyValue<int>(nameof(tx_result), ref ftx_result, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int tx_error_code
        {
            get => ftx_error_code;
            set => SetPropertyValue<int>(nameof(tx_error_code), ref ftx_error_code, value);
        }

        [Size(255)]
        [ModelDefault("AllowEdit", "False")]
        public string tx_error_message
        {
            get => ftx_error_message;
            set => SetPropertyValue(nameof(tx_error_message), ref ftx_error_message, value);
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
        public bool notes_rejected
        {
            get => fnotes_rejected;
            set => SetPropertyValue(nameof(notes_rejected), ref fnotes_rejected, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool jam_detected
        {
            get => fjam_detected;
            set => SetPropertyValue(nameof(jam_detected), ref fjam_detected, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Persistent("escrow_jam")]
        public bool escrow_jam_detected
        {
            get => fescrow_jam_detected;
            set => SetPropertyValue(nameof(escrow_jam_detected), ref fescrow_jam_detected, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Association("TransactionReferencesCIT")]
        public CIT cit_id
        {
            get => fcit_id;
            set => SetPropertyValue(nameof(cit_id), ref fcit_id, value);
        }

        [Size(50)]
        [ModelDefault("AllowEdit", "False")]
        [DisplayName("Suspense Account")]
        public string tx_suspense_account
        {
            get => ftx_suspense_account;
            set => SetPropertyValue(nameof(tx_suspense_account), ref ftx_suspense_account, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Persistent("init_user")]
        [DisplayName("Deposit User")]
        [Association("TransactionReferencesApplicationUser_Init")]
        public ApplicationUser init_user
        {
            get => finit_user;
            set => SetPropertyValue(nameof(init_user), ref finit_user, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Persistent("auth_user")]
        [DisplayName("Authorising User")]
        [Association("TransactionReferencesApplicationUser_Auth")]
        public ApplicationUser auth_user
        {
            get => fauth_user;
            set => SetPropertyValue(nameof(auth_user), ref fauth_user, value);
        }

        [Association("DenominationDetailReferencesTransaction")]
        public XPCollection<DenominationDetail> DenominationDetails => GetCollection<DenominationDetail>(nameof(DenominationDetails));

        [Association("PrintoutReferencesTransaction")]
        public XPCollection<Printout> Printouts => GetCollection<Printout>(nameof(Printouts));

        [Association("EscrowJamReferencesTransaction")]
        public XPCollection<EscrowJam> EscrowJams => GetCollection<EscrowJam>(nameof(EscrowJams));

        [Association("TransactionPostingReferencesTransaction")]
        public XPCollection<TransactionPosting> TransactionPostings => GetCollection<TransactionPosting>(nameof(TransactionPostings));

        public Transaction(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
