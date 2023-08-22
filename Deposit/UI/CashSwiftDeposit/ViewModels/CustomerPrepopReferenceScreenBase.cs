using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CashSwiftDeposit.ViewModels
{
    public abstract class CustomerPrepopReferenceScreenBase :
      DepositorCustomerScreenBaseViewModel,
      ICustomerComboBoxInputScreen
    {
        private GuiScreenListScreen _GuiScreenList_Screen;
        private GUIPrepopList _GUIPrepopList;
        private string _EditComboBoxButtonCaption;
        private string _CancelEditComboBoxButtonCaption;
        private bool _ComboBoxGridIsVisible;
        private bool _TextBoxGridIsVisible;
        private bool _KeyboardGridIsVisible;
        private bool _AllowFreeText;
        private bool _IsComboBoxEditMode;
        private bool _EditComboBoxIsVisible;
        private bool _ComboBoxButtonsIsVisible;
        private bool _CancelEditComboBoxIsVisible;

        public ObservableCollection<string> CustomerComboBoxInput { get; set; }

        public string SelectedCustomerComboBoxInput
        {
            get => CustomerInput;
            set
            {
                if (value != null)
                    CustomerInput = value;
                NotifyOfPropertyChange(() => SelectedCustomerComboBoxInput);
            }
        }

        public GuiScreenListScreen GuiScreenListScreen
        {
            get => _GuiScreenList_Screen;
            set
            {
                _GuiScreenList_Screen = value;
                NotifyOfPropertyChange(() => GuiScreenListScreen);
            }
        }
        public GUIPrepopList GUIPrepopList
        {
            get => _GUIPrepopList;
            set
            {
                _GUIPrepopList = value;
                NotifyOfPropertyChange(() => GUIPrepopList);
            }
        }

        public string EditComboBoxButtonCaption
        {
            get => _EditComboBoxButtonCaption;
            set
            {
                _EditComboBoxButtonCaption = value;
                NotifyOfPropertyChange(() => EditComboBoxButtonCaption);
            }
        }

        public string CancelEditComboBoxButtonCaption
        {
            get => _CancelEditComboBoxButtonCaption;
            set
            {
                _CancelEditComboBoxButtonCaption = value;
                NotifyOfPropertyChange(() => CancelEditComboBoxButtonCaption);
            }
        }

        public bool ComboBoxGridIsVisible
        {
            get => _ComboBoxGridIsVisible;
            set
            {
                _ComboBoxGridIsVisible = value;
                NotifyOfPropertyChange(() => ComboBoxGridIsVisible);
            }
        }

        public bool TextBoxGridIsVisible
        {
            get => _TextBoxGridIsVisible;
            set
            {
                _TextBoxGridIsVisible = value;
                NotifyOfPropertyChange(() => TextBoxGridIsVisible);
            }
        }

        public bool KeyboardGridIsVisible
        {
            get => _KeyboardGridIsVisible;
            set
            {
                _KeyboardGridIsVisible = value;
                NotifyOfPropertyChange(() => KeyboardGridIsVisible);
            }
        }

        public bool AllowFreeText
        {
            get
            {
                GUIPrepopList guiPrepopList = GUIPrepopList;
                return guiPrepopList == null || guiPrepopList.AllowFreeText;
            }
            set
            {
                _AllowFreeText = value;
                ComboBoxButtonsIsVisible = _AllowFreeText;
                NotifyOfPropertyChange(() => AllowFreeText);
            }
        }

        public bool IsComboBoxEditMode
        {
            get => _IsComboBoxEditMode;
            set
            {
                _IsComboBoxEditMode = value;
                KeyboardGridIsVisible = _IsComboBoxEditMode;
                TextBoxGridIsVisible = _IsComboBoxEditMode;
                CancelEditComboBoxIsVisible = _IsComboBoxEditMode;
                ComboBoxGridIsVisible = !_IsComboBoxEditMode;
                EditComboBoxIsVisible = !_IsComboBoxEditMode;
                NotifyOfPropertyChange(() => IsComboBoxEditMode);
            }
        }

        public bool EditComboBoxIsVisible
        {
            get => _EditComboBoxIsVisible;
            set
            {
                if (_EditComboBoxIsVisible == value)
                    return;
                _EditComboBoxIsVisible = value;
                NotifyOfPropertyChange(() => EditComboBoxIsVisible);
            }
        }

        public bool ComboBoxButtonsIsVisible
        {
            get => _ComboBoxButtonsIsVisible;
            set
            {
                if (_ComboBoxButtonsIsVisible == value)
                    return;
                _ComboBoxButtonsIsVisible = value;
                NotifyOfPropertyChange(() => ComboBoxButtonsIsVisible);
            }
        }

        public bool CancelEditComboBoxIsVisible
        {
            get => _CancelEditComboBoxIsVisible;
            set
            {
                if (_CancelEditComboBoxIsVisible == value)
                    return;
                _CancelEditComboBoxIsVisible = value;
                NotifyOfPropertyChange(() => CancelEditComboBoxIsVisible);
            }
        }

        public CustomerPrepopReferenceScreenBase(
          string customerInput,
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required,
          bool enableIdleTimer = true,
          double timeoutInterval = 0.0)
          : base(screenTitle, applicationViewModel, required, enableIdleTimer, timeoutInterval)
        {
            CustomerPrepopReferenceScreenBase referenceScreenBase = this;
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                IsComboBoxEditMode = true;
                CustomerInput = customerInput;
               GuiScreenListScreen = applicationViewModel.CurrentGUIScreen.GuiScreenListScreens.FirstOrDefault((x => x.GuiScreenList.id == applicationViewModel.CurrentTransaction.TransactionType.GUIScreenList.id));
      // GuiScreenListScreen = depositorDbContext.GuiScreenListScreens.FirstOrDefault(x => x.screen == applicationViewModel.CurrentGUIScreen.id && x.GuiScreenList.id == applicationViewModel.CurrentTransaction.TransactionType.GUIScreenList.id);
                EditComboBoxButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(nameof(EditComboBoxButtonCaption), "sys_EditComboBoxButtonCaption", "Edit");
                CancelEditComboBoxButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(nameof(CancelEditComboBoxButtonCaption), "sys_CancelEditComboBoxButtonCaption", "Choose");
                int num1;
                //                if ((bool)(GuiScreenListScreen?.GUIPrepopList?.enabled))
                if (GuiScreenListScreen?.GUIPrepopList != null && (bool)!GuiScreenListScreen?.GUIPrepopList?.enabled)
                {
                    GuiScreenListScreen screenListScreen = GuiScreenListScreen;
                    if (screenListScreen == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        int? count = screenListScreen.GUIPrepopList?.GUIPrepopListItems?.Count;
                        int num2 = 0;
                        num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
                    }
                }
                else
                    num1 = 0;
                if (num1 == 0)
                    return;
                GUIPrepopList = GuiScreenListScreen.GUIPrepopList;
                GUIPrepopList guiPrepopList = GUIPrepopList;
                List<string> list;
                if (guiPrepopList == null)
                {
                    list = null;
                }
                else
                {
                    ICollection<GUIPrepopListItem> guiPrepopListItem = guiPrepopList.GUIPrepopListItems;
                    if (guiPrepopListItem == null)
                    {
                        list = null;
                    }
                    else
                    {
                        IEnumerable<GUIPrepopListItem> source1 = guiPrepopListItem.Where(x => (bool)x.GUIPrepopItemNavigation.enabled);
                        if (source1 == null)
                        {
                            list = null;
                        }
                        else
                        {
                            IOrderedEnumerable<GUIPrepopListItem> source2 = source1.OrderBy(x => x.List_Order);
                            list = source2 != null ? source2.Select(x => ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("CustomerPrepopReferenceScreenBase.CustomerComboBoxInput", new Guid?(x.GUIPrepopItemNavigation.value), "Empty ListItem")).ToList() : null;
                        }
                    }
                }
                CustomerComboBoxInput = new ObservableCollection<string>(list);
                SetComboBoxDefault();
                AllowFreeText = GUIPrepopList.AllowFreeText;
                int num3;
                if (!IsInputNull(CustomerInput))
                {
                    ObservableCollection<string> customerComboBoxInput = CustomerComboBoxInput;

                    num3 = customerComboBoxInput != null ? (!customerComboBoxInput.Contains(CustomerInput) ? 1 : 0) : 0;
                }
                else
                    num3 = 0;
                IsComboBoxEditMode = num3 != 0;
                _KeyboardGridIsVisible = true;
                NotifyOfPropertyChange(() => CanEditComboBox);
                NotifyOfPropertyChange(() => CanCancelEditComboBox);
                NotifyOfPropertyChange(() => KeyboardGridIsVisible);
            }
        }

        protected void SetComboBoxDefault(bool overrideWithDefault = false)
        {
            GUIPrepopList guiPrepopList = GUIPrepopList;
            if ((guiPrepopList != null ? ((bool)guiPrepopList.UseDefault ? 1 : 0) : 0) == 0 || !overrideWithDefault && !string.IsNullOrWhiteSpace(CustomerInput))
                return;
            SelectedCustomerComboBoxInput = CustomerComboBoxInput[GUIPrepopList.DefaultIndex.Clamp(0, GUIPrepopList.GUIPrepopListItems.Count - 1)];
        }

        public bool CanEditComboBox => AllowFreeText && !IsComboBoxEditMode;

        public void EditComboBox()
        {
            IsComboBoxEditMode = true;
            NotifyOfPropertyChange(() => CanEditComboBox);
            NotifyOfPropertyChange(() => CanCancelEditComboBox);
        }

        public bool CanCancelEditComboBox => AllowFreeText && IsComboBoxEditMode;

        public void CancelEditComboBox()
        {
            IsComboBoxEditMode = false;
            SetComboBoxDefault(true);
            NotifyOfPropertyChange(() => CanEditComboBox);
            NotifyOfPropertyChange(() => CanCancelEditComboBox);
        }
    }
}
