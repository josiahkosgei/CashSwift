// ViewModels.ReferenceAccountNumberInputScreenViewModel


using CashSwift.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using CashSwiftDeposit.Models;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("DDF37150-7D4B-40EE-9B33-E6ADC937C29A")]
    internal class ReferenceAccountNumberInputScreenViewModel : DepositorCustomerScreenBaseViewModel
    {
        public ReferenceAccountNumberInputScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            CustomerInput = applicationViewModel.CurrentTransaction.ReferenceAccount;
            ApplicationViewModel.CurrentTransaction.AccountNumber = ApplicationViewModel.CurrentTransaction.TransactionType.default_account;
            ApplicationViewModel.CurrentTransaction.AccountName = ApplicationViewModel.CurrentTransaction.TransactionType.default_account_name;
            try
            {
                ScreenTitle = ApplicationViewModel.CashSwiftTranslationService.TranslateUserText(GetType().Name + ".ReferenceAccountNumberInputScreenViewModel  ScreenTitle", applicationViewModel?.CurrentTransaction?.TransactionType?.TransactionText.reference_account_number_caption, "Reference Number");
            }
            catch (Exception ex)
            {
            }
        }

        public void Next()
        {
            lock (ApplicationViewModel.NavigationLock)
            {
                if (!CanNext)
                    return;
                CanNext = false;
                ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.WorkerReportsProgress = false;
                backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Task.Run(() => ValidateAsync(CustomerInput)).Result)
            {
                ApplicationViewModel.NavigateNextScreen();
            }
            else
            {
                ApplicationViewModel.CloseDialog(false);
                CanNext = true;
            }
        }

        public void Cancel() => ApplicationViewModel.CancelSessionOnUserInput();

        public void Back()
        {
            ApplicationViewModel.CurrentTransaction.ReferenceAccount = CustomerInput;
            ApplicationViewModel.NavigatePreviousScreen();
        }

        public async Task<bool> ValidateAsync(string refAaccountNumber)
        {
            ReferenceAccountNumberInputScreenViewModel inputScreenViewModel = this;
            if (!inputScreenViewModel.ClientValidation(refAaccountNumber))
                return false;
            inputScreenViewModel.ApplicationViewModel.CurrentTransaction.ReferenceAccount = refAaccountNumber;
            AppTransaction currentTransaction = inputScreenViewModel.ApplicationViewModel.CurrentTransaction;
            int num;
            if (currentTransaction == null)
            {
                num = 0;
            }
            else
            {
                bool? referenceAccount = currentTransaction.TransactionType?.validate_reference_account;
                bool flag = true;
                num = referenceAccount.GetValueOrDefault() == flag & referenceAccount.HasValue ? 1 : 0;
            }
            if (num == 0)
                return true;
            ReferenceAccountNumberValidationResponse validationResponse = await inputScreenViewModel.ApplicationViewModel.ValidateReferenceAccountNumberAsync(inputScreenViewModel.ApplicationViewModel.CurrentTransaction.AccountNumber, refAaccountNumber, inputScreenViewModel.ApplicationViewModel.CurrentTransaction?.TransactionType?.cb_tx_type);
            if (validationResponse != null && validationResponse.IsSuccess && validationResponse.CanTransact)
            {
                inputScreenViewModel.ApplicationViewModel.CurrentTransaction.ReferenceAccountName = validationResponse.AccountName;
                return true;
            }
            inputScreenViewModel.PrintErrorText(validationResponse?.PublicErrorMessage);
            return false;
        }
    }
}
