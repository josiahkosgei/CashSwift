using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CashSwiftDeposit.ViewModels
{
    public class DepositorCustomerScreenBaseViewModel : TimeoutScreenBase
    {
        private GUIScreenText _currentGUIScreenText;
        private string _customerInput;
        private string _cancelCaption;
        private string _nextCaption;
        private string _backCaption;
        private string _GetPreviousPage_Caption;
        private string _GetNextPageButton_Caption;
        private bool _fullInstructionsExpanderIsVisible = false;
        private string _fullInstructions;
        private string _screenTitleInstruction;
        private string _ShowFullInstructions_Caption;
        private string _HideFullInstructions_Caption;
        private string _FullInstructionsTitle;
        private KeyboardType _keyboard = KeyboardType.ALPHANUMERIC;
        private string _errorText;
        private bool _canNext = true;
        private bool _canCancel = true;
        private bool _canShowFullInstructions = true;
        private bool _alphanumericKeyboardIsVisible = false;
        private bool _numericKeypadIsVisible = false;
        private bool _fullAlphanumericKeyboardIsVisible = false;
        public DepositorCustomerScreenBaseViewModel(
              string screenTitle,
              ApplicationViewModel applicationViewModel,
              bool required,
              bool enableIdleTimer = true,
              double timeoutInterval = 0.0)
              : base(screenTitle, applicationViewModel, timeoutInterval)
        {
            if (applicationViewModel == null)
                throw new NullReferenceException("applicationViewModel cannot be null in DepositorCustomerScreenBaseViewModel.DepositorCustomerScreenBaseViewModel()");
            EnableIdleTimer = enableIdleTimer;
            TimeOutInterval = timeoutInterval > 0.0 ? timeoutInterval : (ApplicationViewModel.DeviceConfiguration.USER_SCREEN_TIMEOUT > 0 ? ApplicationViewModel.DeviceConfiguration.USER_SCREEN_TIMEOUT : 30.0);
            applicationViewModel?.SetLanguage();
            Required = required;
            InitialiseScreen();
        }


        public GUIScreenText CurrentGUIScreenText
        {
            get => _currentGUIScreenText;
            set
            {
                _currentGUIScreenText = value;
                NotifyOfPropertyChange(() => CurrentGUIScreenText);
            }
        }

        public string CustomerInput
        {
            get => _customerInput;
            set
            {
                _customerInput = !string.IsNullOrWhiteSpace(value) ? value : PrefillText;
                NotifyOfPropertyChange(() => CustomerInput);
            }
        }

        public string PrefillText { get; set; }

        public string CancelCaption
        {
            get => _cancelCaption;
            set
            {
                _cancelCaption = value;
                NotifyOfPropertyChange(() => CancelCaption);
            }
        }

        public string NextCaption
        {
            get => _nextCaption;
            set
            {
                _nextCaption = value;
                NotifyOfPropertyChange(() => NextCaption);
            }
        }

        public string BackCaption
        {
            get => _backCaption;
            set
            {
                _backCaption = value;
                NotifyOfPropertyChange(() => BackCaption);
            }
        }

        public string GetPreviousPageCaption
        {
            get => _GetPreviousPage_Caption;
            set
            {
                _GetPreviousPage_Caption = value;
                NotifyOfPropertyChange(() => GetPreviousPageCaption);
            }
        }

        public string GetNextPageCaption
        {
            get => _GetNextPageButton_Caption;
            set
            {
                _GetNextPageButton_Caption = value;
                NotifyOfPropertyChange(() => GetNextPageCaption);
            }
        }

        public bool FullInstructionsExpanderIsVisible
        {
            get => _fullInstructionsExpanderIsVisible;
            set
            {
                _fullInstructionsExpanderIsVisible = value;
                NotifyOfPropertyChange(() => FullInstructionsExpanderIsVisible);
            }
        }

        public string FullInstructions
        {
            get => _fullInstructions;
            set
            {
                _fullInstructions = value;
                NotifyOfPropertyChange(() => FullInstructions);
            }
        }

        public string ScreenTitleInstruction
        {
            get => _screenTitleInstruction;
            set
            {
                _screenTitleInstruction = value;
                NotifyOfPropertyChange(() => ScreenTitleInstruction);
            }
        }

        public string ShowFullInstructionsCaption
        {
            get => _ShowFullInstructions_Caption;
            set
            {
                _ShowFullInstructions_Caption = value;
                NotifyOfPropertyChange(() => ShowFullInstructionsCaption);
            }
        }

        public string HideFullInstructionsCaption
        {
            get => _HideFullInstructions_Caption;
            set
            {
                _HideFullInstructions_Caption = value;
                NotifyOfPropertyChange(() => HideFullInstructionsCaption);
            }
        }

        public string FullInstructionsTitle
        {
            get => _FullInstructionsTitle;
            set
            {
                _FullInstructionsTitle = value;
                NotifyOfPropertyChange(() => FullInstructionsTitle);
            }
        }

        public virtual bool AlphanumericKeyboardIsVisible
        {
            get
            {
                return _alphanumericKeyboardIsVisible;
            }
            set
            {
                _alphanumericKeyboardIsVisible = Keyboard == KeyboardType.ALPHANUMERIC;
                NotifyOfPropertyChange(() => FullInstructionsTitle);
            }
        }

        public virtual bool FullAlphanumericKeyboardIsVisible
        {
            get
            {
                return _fullAlphanumericKeyboardIsVisible;
            }
            set
            {
                _fullAlphanumericKeyboardIsVisible = Keyboard == KeyboardType.FULLALPHANUMERIC;
                NotifyOfPropertyChange(() => FullAlphanumericKeyboardIsVisible);
            }
        }

        public virtual bool NumericKeypadIsVisible
        {
            get
            {
                return _numericKeypadIsVisible;
            }
            set
            {
                _numericKeypadIsVisible = Keyboard == KeyboardType.NUMERIC;
                NotifyOfPropertyChange(() => NumericKeypadIsVisible);
            }
        }

        public KeyboardType Keyboard
        {
            get => _keyboard;
            set
            {
                _keyboard = value;
                RefreshKeyboard();
            }
        }

        public virtual bool CanNext
        {
            get => _canNext;
            set
            {
                _canNext = value;
                NotifyOfPropertyChange(() => CanNext);
            }
        }

        public bool CanCancel
        {
            get => _canCancel;
            set
            {
                _canCancel = value;
                NotifyOfPropertyChange(() => CanCancel);
            }
        }

        public bool CanShowFullInstructions
        {
            get => _canShowFullInstructions;
            set
            {
                _canShowFullInstructions = value;
                NotifyOfPropertyChange(() => CanShowFullInstructions);
            }
        }

        public bool Required { get; set; }

        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                NotifyOfPropertyChange(() => ErrorText);
            }
        }

        private void InitialiseScreen()
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())

                CurrentGUIScreenText = ApplicationViewModel?.CurrentGUIScreen?.GUIScreenText;
            // Keyboard = guiScreen.keyboard.HasValue ? (KeyboardType)guiScreen.keyboard.Value : KeyboardType.ALPHANUMERIC;
            ApplicationViewModel applicationViewModel1 = ApplicationViewModel;
            int? nullable1;
            int num1;
            if (applicationViewModel1 == null)
            {
                num1 = 0;
            }
            else
            {
                GUIScreen currentGuiScreen = applicationViewModel1.CurrentGUIScreen;
                if (currentGuiScreen == null)
                {
                    num1 = 0;
                }
                else
                {
                    nullable1 = currentGuiScreen.keyboard;
                    num1 = nullable1.HasValue ? 1 : 0;
                }
            }
            int num2;
            if (num1 == 0)
            {
                num2 = 2;
            }
            else
            {
                ApplicationViewModel applicationViewModel2 = ApplicationViewModel;
                int? nullable2;
                if (applicationViewModel2 == null)
                {
                    nullable1 = new int?();
                    nullable2 = nullable1;
                }
                else
                {
                    GUIScreen currentGuiScreen = applicationViewModel2.CurrentGUIScreen;
                    if (currentGuiScreen == null)
                    {
                        nullable1 = new int?();
                        nullable2 = nullable1;
                    }
                    else
                        nullable2 = currentGuiScreen.keyboard;
                }
                nullable1 = nullable2;
                num2 = nullable1.Value;
            }

            Keyboard = (KeyboardType)num2;
            ScreenTitle = string.IsNullOrWhiteSpace(ScreenTitle) ? ApplicationViewModel.CashSwiftTranslationService?.TranslateUserText("ScreenTitle", CurrentGUIScreenText?.screen_title, ApplicationViewModel?.CurrentGUIScreen?.name) : ScreenTitle;
            CancelCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateUserText("CancelCaption", CurrentGUIScreenText?.btn_cancel_caption, "Cancel");
            BackCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateUserText("BackCaption", CurrentGUIScreenText?.btn_back_caption, "Back");
            NextCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateUserText("NextCaption", CurrentGUIScreenText?.btn_accept_caption, "Next");
            GetPreviousPageCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("GetPreviousPageCaption", "sys_GetPreviousPage_Caption", "Prev");
            GetNextPageCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("GetNextPageCaption", "sys_GetNextPage_Caption", "Next");
            FullInstructions = CustomerInputScreenReplace(ApplicationViewModel.CashSwiftTranslationService?.TranslateUserText("FullInstructions", CurrentGUIScreenText?.full_instructions, null));
            ScreenTitleInstruction = CustomerInputScreenReplace(ApplicationViewModel.CashSwiftTranslationService?.TranslateUserText("ScreenTitleInstruction", CurrentGUIScreenText?.screen_title_instruction, null));
            ShowFullInstructionsCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("ShowFullInstructionsCaption", "sys_ShowFullInstructions_Caption", "Help");
            HideFullInstructionsCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("HideFullInstructionsCaption", "sys_Dialog_OK_Caption", "OK");
            FullInstructionsTitle = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("FullInstructionsTitle", "sys_FullInstructionsExpander_TitleCaption", "Instructions");
            if (!string.IsNullOrWhiteSpace(FullInstructions))
                return;
            CanShowFullInstructions = false;
        }


        protected string CustomerInputScreenReplace(string s) => s != null ? s.CashSwiftReplace(ApplicationViewModel)?.Replace("{screen_title}", ScreenTitle)?.Replace("{btn_accept_caption}", NextCaption)?.Replace("{btn_back_caption}", BackCaption)?.Replace("{btn_cancel_caption}", CancelCaption)?.Replace("{btn_page_next_caption}", ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(nameof(CustomerInputScreenReplace), "sys_GetNextPage_Caption", "More"))?.Replace("{btn_page_previous_caption}", ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(nameof(CustomerInputScreenReplace), "sys_GetPreviousPageButton_Caption", "Prev"))?.Replace("{btn_escrow_reject_caption}", ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(nameof(CustomerInputScreenReplace), "sys_EscrowRejectButton_Caption", "Reject"))?.Replace("{btn_escrow_drop_caption}", ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(nameof(CustomerInputScreenReplace), "sys_EscrowDropButton_Caption", "Drop")) : null;

        protected virtual void RefreshKeyboard()
        {
            NotifyOfPropertyChange(() => Keyboard);
            NotifyOfPropertyChange(() => AlphanumericKeyboardIsVisible);
            NotifyOfPropertyChange(() => FullAlphanumericKeyboardIsVisible);
            NotifyOfPropertyChange(() => NumericKeypadIsVisible);
        }

        internal void PrintErrorText(string errorText) => ErrorText = string.Format("[{0:HH:mm:ss.fff}] {1}", DateTime.Now, errorText);

        internal void ClearErrorText() => ErrorText = "";

        internal bool ClientValidation(string valueToValidate)
        {
            try
            {
                if (Required && IsInputNull(valueToValidate))
                {
                    PrintErrorText(ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(ClientValidation), "sys_ValidationRequiredFieldError", "Field is required")?.ToUpperInvariant());
                    return false;
                }
                using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                {
                    GUIScreen guiScreen = depositorDbContext.GUIScreens.Where(z => z.id == this.ApplicationViewModel.CurrentGUIScreen.id).FirstOrDefault();
                    ValidationList validationList = guiScreen != null ? guiScreen.GuiScreenListScreens.Where(x => x.gui_screen_list == ApplicationViewModel.CurrentTransaction.TransactionType.tx_type_guiscreenlist).FirstOrDefault()?.ValidationList : null;
                    if (validationList != null)
                    {
                        List<ValidationListValidationItem> listValidationItemList;
                        if (validationList == null)
                        {
                            listValidationItemList = null;
                        }
                        else
                        {
                            ICollection<ValidationListValidationItem> listValidationItem = validationList.ValidationListValidationItems;
                            listValidationItemList = listValidationItem != null ? listValidationItem.ToList() : null;
                        }
                        foreach (ValidationListValidationItem listValidationItem in listValidationItemList)
                        {
                            if ((bool)listValidationItem.enabled && listValidationItem.ValidationItem.enabled && listValidationItem.ValidationItem.ValidationType.enabled && listValidationItem.ValidationItem.ValidationType.name == "Regex")
                            {
                                if (listValidationItem.ValidationItem.ValidationItemValues.Count <= 0 || !ClientValidationRules.RegexValidation(valueToValidate, listValidationItem.ValidationItem.ValidationItemValues.OrderBy(x => x.order).ToList()[0].value.Replace("\r\n", "\n").Replace("\n", "")))
                                {
                                    try
                                    {
                                        PrintErrorText(ApplicationViewModel.CashSwiftTranslationService.TranslateUserText(GetType().Name + ".ClientValidation Validation.ErrorMessage", listValidationItem?.ValidationItem?.ValidationText?.error_message, "Validation Failed"));
                                    }
                                    catch (Exception ex)
                                    {
                                        ApplicationViewModel.Log.WarningFormat(GetType().Name, "Validation Error Message", "TranslationError", "Failed to print error message for ValidationItem {0}", listValidationItem?.ValidationItem?.name);
                                        PrintErrorText("Validation Failed");
                                    }
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.ErrorFormat(GetType().Name, 31, ApplicationErrorConst.ERROR_INVALID_DATA.ToString(), "Error during regex validation: {0}", ex.MessageString());
                PrintErrorText("Invalid Regex Exception. Contact Administrator");
                return false;
            }
        }

        protected bool IsInputNull(string valueToValidate) => string.IsNullOrWhiteSpace(valueToValidate) || string.Equals(valueToValidate, PrefillText, StringComparison.InvariantCultureIgnoreCase);

        public void ShowFullInstructions() => FullInstructionsExpanderIsVisible = true;

        public void HideFullInstructions() => FullInstructionsExpanderIsVisible = false;
    }
}
