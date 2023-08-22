
//BusinessObjects.Transactions.TransactionTypeList_TransactionTypeListItem


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [FriendlyKeyProperty("txtype_list")]
    [DefaultProperty("txtype_list_item")]
    [VisibleInReports]
    [VisibleInDashboards]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class TransactionTypeList_TransactionTypeListItem : XPLiteObject
    {
        private Guid fid;
        private TransactionTypeListItem ftxtype_list_item;
        private TransactionTypeList ftxtype_list;
        private int flist_order;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("TransactionTypeList_TransactionTypeListItemReferencesTransactionTypeListItem")]
        [RuleRequiredField(DefaultContexts.Save)]
        public TransactionTypeListItem txtype_list_item
        {
            get => ftxtype_list_item;
            set => SetPropertyValue(nameof(txtype_list_item), ref ftxtype_list_item, value);
        }

        [Indexed("txtype_list_item", Name = "UX_TransactionTypeList_TransactionTypeListItem_Item", Unique = true)]
        [Association("TransactionTypeList_TransactionTypeListItemReferencesTransactionTypeList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public TransactionTypeList txtype_list
        {
            get => ftxtype_list;
            set => SetPropertyValue(nameof(txtype_list), ref ftxtype_list, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int list_order
        {
            get => flist_order;
            set => SetPropertyValue<int>(nameof(list_order), ref flist_order, value);
        }

        public TransactionTypeList_TransactionTypeListItem(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
