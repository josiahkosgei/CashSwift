
//BusinessObjects.Screens.GuiScreenList_Screen


using CashSwiftCashControlPortal.Module.BusinessObjects.Validation;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Screens
{
    [FriendlyKeyProperty("gui_screen_list")]
    [DefaultProperty("screen")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class GuiScreenList_Screen : XPLiteObject
    {
        private Guid fid;
        private bool fenabled;
        private GUIScreen fscreen;
        private GUIScreenList fgui_screen_list;
        private int fscreen_order;
        private bool frequired;
        private ValidationList fvalidation_list_id;
        private GUIPrepopList fguiprepoplist_id;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [Association("GuiScreenList_ScreenReferencesGUIScreen")]
        [RuleRequiredField(DefaultContexts.Save)]
        public GUIScreen screen
        {
            get => fscreen;
            set => SetPropertyValue(nameof(screen), ref fscreen, value);
        }

        [Indexed("screen_order", Name = "UX_GuiScreenList_Screen_Order", Unique = true)]
        [Association("GuiScreenList_ScreenReferencesGUIScreenList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public GUIScreenList gui_screen_list
        {
            get => fgui_screen_list;
            set => SetPropertyValue(nameof(gui_screen_list), ref fgui_screen_list, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int screen_order
        {
            get => fscreen_order;
            set => SetPropertyValue<int>(nameof(screen_order), ref fscreen_order, value);
        }

        public bool required
        {
            get => frequired;
            set => SetPropertyValue(nameof(required), ref frequired, value);
        }

        [Association("GuiScreenList_ScreenReferencesValidationList")]
        public ValidationList validation_list_id
        {
            get => fvalidation_list_id;
            set => SetPropertyValue(nameof(validation_list_id), ref fvalidation_list_id, value);
        }

        [Association("GuiScreenList_ScreenReferencesGUIPrepopList")]
        public GUIPrepopList guiprepoplist_id
        {
            get => fguiprepoplist_id;
            set => SetPropertyValue(nameof(guiprepoplist_id), ref fguiprepoplist_id, value);
        }

        public GuiScreenList_Screen(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
