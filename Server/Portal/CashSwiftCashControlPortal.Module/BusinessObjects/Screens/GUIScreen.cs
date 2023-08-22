
//BusinessObjects.Screens.GUIScreen


using CashSwift.Library.Standard.Statuses;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Screens
{
    [NavigationItem("Screen Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class GUIScreen : XPLiteObject
    {
        private int fid;
        private string fname;
        private string fdescription;
        private GUIScreenType ftype;
        private bool fenabled;
        private KeyboardType fkeyboard;
        private bool fis_masked;
        private string fprefill_text;
        private string finput_mask;
        private GUIScreenText fgui_text;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public int id
        {
            get => fid;
            set => SetPropertyValue<int>(nameof(id), ref fid, value);
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

        [Association("GUIScreenReferencesGUIScreenType")]
        [RuleRequiredField(DefaultContexts.Save)]
        public GUIScreenType type
        {
            get => ftype;
            set => SetPropertyValue(nameof(type), ref ftype, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        public KeyboardType keyboard
        {
            get => fkeyboard;
            set => SetPropertyValue(nameof(keyboard), ref fkeyboard, value);
        }

        public bool is_masked
        {
            get => fis_masked;
            set => SetPropertyValue(nameof(is_masked), ref fis_masked, value);
        }

        [Size(50)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string prefill_text
        {
            get => fprefill_text;
            set => SetPropertyValue(nameof(prefill_text), ref fprefill_text, value);
        }

        [Size(50)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string input_mask
        {
            get => finput_mask;
            set => SetPropertyValue(nameof(input_mask), ref finput_mask, value);
        }

        [Association("GuiScreenList_ScreenReferencesGUIScreen")]
        public XPCollection<GuiScreenList_Screen> GuiScreenList_Screens => GetCollection<GuiScreenList_Screen>(nameof(GuiScreenList_Screens));

        [RuleRequiredField(DefaultContexts.Save)]
        [Persistent("gui_text")]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public GUIScreenText GUIScreenText
        {
            get => fgui_text;
            set
            {
                if (fgui_text == value)
                    return;
                GUIScreenText fguiText = fgui_text;
                fgui_text = value;
                if (IsLoading)
                    return;
                if (fguiText != null && fguiText.GUIScreen == this)
                    fguiText.GUIScreen =  null;
                if (fgui_text != null)
                    fgui_text.GUIScreen = this;
                OnChanged(nameof(GUIScreenText));
            }
        }

        public GUIScreen(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            GUIScreenText = new GUIScreenText(Session);
        }
    }
}
