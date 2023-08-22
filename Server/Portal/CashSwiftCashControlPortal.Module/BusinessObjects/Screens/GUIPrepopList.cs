
//BusinessObjects.Screens.GUIPrepopList


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
    public class GUIPrepopList : XPLiteObject
    {
        private Guid fid;
        private string fname;
        private string fdescription;
        private bool fenabled;
        private bool fAllowFreeText;
        private int fDefaultIndex;
        private bool fUseDefault;

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
        [DisplayName("Name")]
        [Persistent("name")]
        public string name
        {
            get => fname;
            set => SetPropertyValue(nameof(name), ref fname, value);
        }

        [Size(255)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [DisplayName("Description")]
        public string description
        {
            get => fdescription;
            set => SetPropertyValue(nameof(description), ref fdescription, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        public bool AllowFreeText
        {
            get => fAllowFreeText;
            set => SetPropertyValue(nameof(AllowFreeText), ref fAllowFreeText, value);
        }

        public int DefaultIndex
        {
            get => fDefaultIndex;
            set => SetPropertyValue<int>(nameof(DefaultIndex), ref fDefaultIndex, value);
        }

        public bool UseDefault
        {
            get => fUseDefault;
            set => SetPropertyValue(nameof(UseDefault), ref fUseDefault, value);
        }

        [Association("GuiScreenList_ScreenReferencesGUIPrepopList")]
        public XPCollection<GuiScreenList_Screen> GuiScreenList_Screens => GetCollection<GuiScreenList_Screen>(nameof(GuiScreenList_Screens));

        [Association("GUIPrepopList_ItemReferencesGUIPrepopList")]
        public XPCollection<GUIPrepopList_Item> GUIPrepopList_Items => GetCollection<GUIPrepopList_Item>(nameof(GUIPrepopList_Items));

        public GUIPrepopList(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
