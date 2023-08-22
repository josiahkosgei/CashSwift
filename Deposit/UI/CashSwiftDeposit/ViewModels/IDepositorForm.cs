using System.Collections.Generic;

namespace CashSwiftDeposit.ViewModels
{
    public interface IDepositorForm
    {
        void SelectFormField(int fieldID);

        void SelectFormField(FormListItem field);

        void FormHome(bool success);

        void FormClose(bool success);

        List<FormListItem> GetFields();

        string SaveForm();

        int FormValidation();

        string GetErrors();
    }
}
