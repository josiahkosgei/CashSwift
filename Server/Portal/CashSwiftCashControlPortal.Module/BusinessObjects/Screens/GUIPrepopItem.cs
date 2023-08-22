
//BusinessObjects.Screens.GUIPrepopItem


using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Screens
{
    [NavigationItem("Screen Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class GUIPrepopItem : XPLiteObject
    {
        private Guid fid;
        private string fname;
        private string fdescription;
        private UserTextItem fValue;
        private bool fEnabled;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The name was already registered within the system.")]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        public string Name
        {
            get => fname;
            set => SetPropertyValue(nameof(Name), ref fname, value);
        }

        [Size(255)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [DisplayName("Description")]
        public string Description
        {
            get => fdescription;
            set => SetPropertyValue(nameof(Description), ref fdescription, value);
        }

        [Association("GUIPrepopItem_ValueReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_GUIPrepropItem.value'")]
        [Persistent("Value")]
        public UserTextItem Value
        {
            get => fValue;
            set => SetPropertyValue(nameof(Value), ref fValue, value);
        }

        [DefaultValue(true)]
        public bool Enabled
        {
            get => fEnabled;
            set => SetPropertyValue(nameof(Enabled), ref fEnabled, value);
        }

        [Association("GUIPrepopList_ItemReferencesGUIPrepopItem")]
        public XPCollection<GUIPrepopList_Item> GUIPrepopList_Items => GetCollection<GUIPrepopList_Item>(nameof(GUIPrepopList_Items));

        public GUIPrepopItem(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
