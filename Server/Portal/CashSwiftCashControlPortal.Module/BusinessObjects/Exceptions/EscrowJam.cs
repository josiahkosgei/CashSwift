
//BusinessObjects.Exceptions.EscrowJam


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Exceptions
{
    [Persistent("exp.EscrowJam")]
    public class EscrowJam : XPLiteObject
    {
        private Guid fid;
        private Transaction ftransaction_id;
        private DateTime fdate_detected;
        private long fdropped_amount;
        private long fescrow_amount;
        private long fposted_amount;
        private long fretreived_amount;
        private DateTime frecovery_date;
        private ApplicationUser finitialising_user;
        private ApplicationUser fauthorising_user;
        private string fadditional_info;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("EscrowJamReferencesTransaction")]
        [ModelDefault("AllowEdit", "False")]
        public Transaction transaction_id
        {
            get => ftransaction_id;
            set => SetPropertyValue(nameof(transaction_id), ref ftransaction_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime date_detected
        {
            get => fdate_detected;
            set => SetPropertyValue<DateTime>(nameof(date_detected), ref fdate_detected, value);
        }

        [ModelDefault("DisplayFormat", "#,##0.##")]
        [PersistentAlias("[dropped_amount]/100.0")]
        public double DroppedAmount => dropped_amount / 100.0;

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "#,##0.##")]
        [Browsable(false)]
        public long dropped_amount
        {
            get => fdropped_amount;
            set => SetPropertyValue(nameof(dropped_amount), ref fdropped_amount, value);
        }

        [ModelDefault("DisplayFormat", "#,##0.##")]
        [PersistentAlias("[escrow_amount]/100.0")]
        public double EscrowAmount => escrow_amount / 100.0;

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        public long escrow_amount
        {
            get => fescrow_amount;
            set => SetPropertyValue(nameof(escrow_amount), ref fescrow_amount, value);
        }

        [ModelDefault("DisplayFormat", "#,##0.##")]
        [PersistentAlias("[posted_amount]/100.0")]
        public double PostedAmount => posted_amount / 100.0;

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        public long posted_amount
        {
            get => fposted_amount;
            set => SetPropertyValue(nameof(posted_amount), ref fposted_amount, value);
        }

        [ModelDefault("DisplayFormat", "#,##0.##")]
        [PersistentAlias("[retreived_amount]/100.0")]
        public double RetreivedAmount => retreived_amount / 100.0;

        [ModelDefault("AllowEdit", "False")]
        [Browsable(false)]
        public long retreived_amount
        {
            get => fretreived_amount;
            set => SetPropertyValue(nameof(retreived_amount), ref fretreived_amount, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime recovery_date
        {
            get => frecovery_date;
            set => SetPropertyValue<DateTime>(nameof(recovery_date), ref frecovery_date, value);
        }

        [Association("EscrowJamReferencesApplicationUser_initialising_user")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser initialising_user
        {
            get => finitialising_user;
            set => SetPropertyValue(nameof(initialising_user), ref finitialising_user, value);
        }

        [Association("EscrowJamReferencesApplicationUser_authorising_user")]
        [ModelDefault("AllowEdit", "False")]
        public ApplicationUser authorising_user
        {
            get => fauthorising_user;
            set => SetPropertyValue(nameof(authorising_user), ref fauthorising_user, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public string additional_info
        {
            get => fadditional_info;
            set => SetPropertyValue(nameof(additional_info), ref fadditional_info, value);
        }

        public EscrowJam(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
