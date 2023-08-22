using CashSwiftDataAccess.Entities;
using System.Collections.ObjectModel;

namespace CashSwiftDeposit.ViewModels
{
    public interface ICustomerComboBoxInputScreen
    {
        ObservableCollection<string> CustomerComboBoxInput { get; set; }

        string SelectedCustomerComboBoxInput { get; set; }

        GuiScreenListScreen GuiScreenListScreen { get; set; }

        GUIPrepopList GUIPrepopList { get; set; }

        bool AllowFreeText { get; set; }

        bool ComboBoxGridIsVisible { get; set; }

        bool TextBoxGridIsVisible { get; set; }

        bool KeyboardGridIsVisible { get; set; }

        bool ComboBoxButtonsIsVisible { get; set; }

        bool EditComboBoxIsVisible { get; set; }

        bool CancelEditComboBoxIsVisible { get; set; }

        bool IsComboBoxEditMode { get; set; }

        string EditComboBoxButtonCaption { get; set; }

        string CancelEditComboBoxButtonCaption { get; set; }

        bool CanEditComboBox { get; }

        void EditComboBox();

        bool CanCancelEditComboBox { get; }

        void CancelEditComboBox();
    }
}
