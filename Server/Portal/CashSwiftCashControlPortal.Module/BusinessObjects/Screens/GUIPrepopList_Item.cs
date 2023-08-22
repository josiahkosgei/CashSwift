
//BusinessObjects.Screens.GUIPrepopList_Item


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Screens
{
    [FriendlyKeyProperty("GUIPrepopList")]
    [DefaultProperty("GUIPrepopItem")]
    public class GUIPrepopList_Item : XPLiteObject
    {
        private Guid fid;
        private GUIPrepopList fList;
        private GUIPrepopItem fItem;
        private int fList_Order;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("GUIPrepopList_ItemReferencesGUIPrepopList")]
        public GUIPrepopList List
        {
            get => fList;
            set => SetPropertyValue(nameof(List), ref fList, value);
        }

        [Indexed("List", Name = "UX_GUIPrepopList_Item", Unique = true)]
        [Association("GUIPrepopList_ItemReferencesGUIPrepopItem")]
        public GUIPrepopItem Item
        {
            get => fItem;
            set => SetPropertyValue(nameof(Item), ref fItem, value);
        }

        [Indexed("List", Name = "UX_GUIPrepopList_ListOrder", Unique = true)]
        public int List_Order
        {
            get => fList_Order;
            set => SetPropertyValue<int>(nameof(List_Order), ref fList_Order, value);
        }

        public GUIPrepopList_Item(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
