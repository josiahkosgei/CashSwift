
//BusinessObjects.Transactions.DenominationDetail


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [FriendlyKeyProperty("currency_id.code")]
    [DefaultProperty("DenomAmount")]
    [VisibleInReports]
    [VisibleInDashboards]
    public class DenominationDetail : XPLiteObject
    {
        private Guid fid;
        private Transaction ftx_id;
        private int fdenom;
        private long fcount;
        private long fsubtotal;
        private DateTime fdatetime;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("DenominationDetailReferencesTransaction")]
        [ModelDefault("AllowEdit", "False")]
        public Transaction tx_id
        {
            get => ftx_id;
            set => SetPropertyValue(nameof(tx_id), ref ftx_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int denom
        {
            get => fdenom;
            set => SetPropertyValue<int>(nameof(denom), ref fdenom, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public long count
        {
            get => fcount;
            set => SetPropertyValue(nameof(count), ref fcount, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public long subtotal
        {
            get => fsubtotal;
            set => SetPropertyValue(nameof(subtotal), ref fsubtotal, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime datetime
        {
            get => fdatetime;
            set => SetPropertyValue<DateTime>(nameof(datetime), ref fdatetime, value);
        }

        public DenominationDetail(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public double SubTotalAmount => subtotal / 100.0;

        public double DenomAmount => denom / 100.0;
    }
}
