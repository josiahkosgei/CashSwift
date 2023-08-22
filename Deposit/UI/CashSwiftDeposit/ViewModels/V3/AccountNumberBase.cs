using System.ComponentModel;
using System.Threading.Tasks;

namespace CashSwiftDeposit.ViewModels.V3
{
    public class AccountNumberBase : DepositorCustomerScreenBaseViewModel
    {
        public AccountNumberBase(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            CustomerInput = applicationViewModel.CurrentTransaction.AccountNumber;
        }

        public void Back()
        {
            ApplicationViewModel.CurrentTransaction.AccountNumber = CustomerInput;
            ApplicationViewModel.NavigatePreviousScreen();
        }

        public void Cancel() => ApplicationViewModel.CancelSessionOnUserInput();

        public virtual void Next()
        {
            lock (ApplicationViewModel.NavigationLock)
            {
                if (!CanNext)
                    return;
                CanNext = false;
                ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
                BackgroundWorker backgroundWorker = new BackgroundWorker()
                {
                    WorkerReportsProgress = false
                };
                backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
                backgroundWorker.RunWorkerAsync();
            }
        }

        public async Task<bool> ValidateAsync(string accountNumber)
        {
            var cb_result = await ApplicationViewModel.ValidateAccountNumberAsync(CustomerInput, ApplicationViewModel.CurrentTransaction?.CurrencyCode.ToUpper(), ApplicationViewModel.CurrentTransaction.TransactionType.id);
            if (cb_result != null && cb_result.IsSuccess)
            {
                ApplicationViewModel.CurrentTransaction.AccountNumber = CustomerInput;
                ApplicationViewModel.CurrentTransaction.AccountName = cb_result.AccountName;
                return true;
            }
            PrintErrorText(cb_result.PublicErrorMessage);
            return false;
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (ClientValidation(CustomerInput) && Task.Run(() => ValidateAsync(CustomerInput)).Result)
            {
                ApplicationViewModel.NavigateNextScreen();
            }
            else
            {
                ApplicationViewModel.CloseDialog(false);
                CanNext = true;
            }
        }
    }
}
