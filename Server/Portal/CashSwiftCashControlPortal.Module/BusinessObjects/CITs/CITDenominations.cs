
//BusinessObjects.CITs.CITDenominations


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.CITs
{
    [FriendlyKeyProperty("currency_id.code")]
    [DefaultProperty("DenomAmount")]
    [VisibleInReports]
    [VisibleInDashboards]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class CITDenominations : XPLiteObject
    {
        private Guid fid;
        private CIT fcit_id;
        private DateTime fdatetime;
        private ApplicationConfiguration.Currency fcurrency_id;
        private int fdenom;
        private long fcount;
        private long fsubtotal;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("CITDenominationsReferencesCIT")]
        [RuleRequiredField(DefaultContexts.Save)]
        [ModelDefault("AllowEdit", "False")]
        public CIT cit_id
        {
            get => fcit_id;
            set => SetPropertyValue(nameof(cit_id), ref fcit_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime datetime
        {
            get => fdatetime;
            set => SetPropertyValue<DateTime>(nameof(datetime), ref fdatetime, value);
        }

        [Size(3)]
        [ModelDefault("AllowEdit", "False")]
        [Association("CITDenominationsReferencesCurrency")]
        public ApplicationConfiguration.Currency currency_id
        {
            get => fcurrency_id;
            set => SetPropertyValue(nameof(currency_id), ref fcurrency_id, value);
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

        public double SubTotalAmount => subtotal / 100.0;

        public double DenomAmount => denom / 100.0;

        public CITDenominations(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
