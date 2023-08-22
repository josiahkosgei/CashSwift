
//BusinessObjects.Screens.GUIScreenText


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
    [FriendlyKeyProperty("GUIScreen.name")]
    [DefaultProperty("GUIScreen.name")]
    public class GUIScreenText : XPLiteObject
    {
        private Guid fid;
        private GUIScreen fguiscreen_id;
        private UserTextItem fscreen_title;
        private UserTextItem fscreen_title_instruction;
        private UserTextItem ffull_instructions;
        private UserTextItem fbtn_accept_caption;
        private UserTextItem fbtn_back_caption;
        private UserTextItem fbtn_cancel_caption;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Persistent("guiscreen_id")]
        [Browsable(false)]
        public GUIScreen GUIScreen
        {
            get => fguiscreen_id;
            set
            {
                if (fguiscreen_id == value)
                    return;
                GUIScreen fguiscreenId = fguiscreen_id;
                fguiscreen_id = value;
                if (IsLoading)
                    return;
                if (fguiscreenId != null && fguiscreenId.GUIScreenText == this)
                    fguiscreenId.GUIScreenText =  null;
                if (fguiscreen_id != null)
                    fguiscreen_id.GUIScreenText = this;
                OnChanged(nameof(GUIScreen));
            }
        }

        [Association("GUIScreenText_ScreenTitleReferencesUserTextItem")]
        [RuleRequiredField(DefaultContexts.Save)]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_GUIScreen.screen_title'")]
        [Persistent("screen_title")]
        public UserTextItem ScreenTitle
        {
            get => fscreen_title;
            set => SetPropertyValue(nameof(ScreenTitle), ref fscreen_title, value);
        }

        [Association("GUIScreenText_ScreenTitleInstructionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_GUIScreen.screen_title_instruction'")]
        [Persistent("screen_title_instruction")]
        public UserTextItem ScreenTitleInstruction
        {
            get => fscreen_title_instruction;
            set => SetPropertyValue(nameof(ScreenTitleInstruction), ref fscreen_title_instruction, value);
        }

        [Association("GUIScreenText_FullInstructionsReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_GUIScreen.full_instructions'")]
        [Persistent("full_instructions")]
        public UserTextItem FullInstructions
        {
            get => ffull_instructions;
            set => SetPropertyValue(nameof(FullInstructions), ref ffull_instructions, value);
        }

        [Association("GUIScreenText_AcceptButtonCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_GUIScreen.btn_accept_caption'")]
        [Persistent("btn_accept_caption")]
        public UserTextItem AcceptButtonCaption
        {
            get => fbtn_accept_caption;
            set => SetPropertyValue(nameof(AcceptButtonCaption), ref fbtn_accept_caption, value);
        }

        [Association("GUIScreenText_BackButtonCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_GUIScreen.btn_back_caption'")]
        [Persistent("btn_back_caption")]
        public UserTextItem BackButtonCaption
        {
            get => fbtn_back_caption;
            set => SetPropertyValue(nameof(BackButtonCaption), ref fbtn_back_caption, value);
        }

        [Association("GUIScreenText_CancelButtonCaptionReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_GUIScreen.btn_cancel_caption'")]
        [Persistent("btn_cancel_caption")]
        public UserTextItem CancelButtonCaption
        {
            get => fbtn_cancel_caption;
            set => SetPropertyValue(nameof(CancelButtonCaption), ref fbtn_cancel_caption, value);
        }

        public GUIScreenText(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
