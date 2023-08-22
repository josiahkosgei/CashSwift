
//BusinessObjects.ApplicationConfiguration.CurrencyList_Currency


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration
{
    [FriendlyKeyProperty("currency_item")]
    [DefaultProperty("currency_list")]
    [VisibleInReports]
    [VisibleInDashboards]
    [Indices("currency_list;currency_item")]
    public class CurrencyList_Currency : XPLiteObject
    {
        private Guid fid;
        private CurrencyList fcurrency_list;
        private Currency fcurrency_item;
        private int fcurrency_order;
        private long fmax_value;
        private int fmax_count;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed("currency_order", Name = "UX_Currency_CurrencyList_Order", Unique = true)]
        [Association("CurrencyList_CurrencyReferencesCurrencyList")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Currency List")]
        [Persistent("currency_list")]
        public CurrencyList currency_list
        {
            get => fcurrency_list;
            set => SetPropertyValue(nameof(currency_list), ref fcurrency_list, value);
        }

        [Size(3)]
        [Association("CurrencyList_CurrencyReferencesCurrency")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Currency")]
        [Persistent("currency_item")]
        public Currency currency_item
        {
            get => fcurrency_item;
            set => SetPropertyValue(nameof(currency_item), ref fcurrency_item, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [DisplayName("Order")]
        [Persistent("currency_order")]
        public int currency_order
        {
            get => fcurrency_order;
            set => SetPropertyValue<int>(nameof(currency_order), ref fcurrency_order, value);
        }

        [DisplayName("Max Value")]
        [Persistent("max_value")]
        public long max_value
        {
            get => fmax_value;
            set => SetPropertyValue(nameof(max_value), ref fmax_value, value);
        }

        [DisplayName("Max Count")]
        [Persistent("max_count")]
        public int max_count
        {
            get => fmax_count;
            set => SetPropertyValue<int>(nameof(max_count), ref fmax_count, value);
        }

        public CurrencyList_Currency(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
