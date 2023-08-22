// ViewModels.TransactionListScreenViewModel


using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.API.Messaging.Models;
using CashSwift.Library.Standard.Statuses;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.Models.Submodule;
using CashSwiftDeposit.Utils;
using CashSwiftUtil.Imaging;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("F6BEA4EB-B0C9-4EB3-B225-1F83F73BFD70")]
    internal class TransactionListScreenViewModel : CustomerListScreenBaseViewModel
    {
        public TransactionListScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                FullList = ApplicationViewModel.TransactionTypesAvailable.Select(x => new ATMSelectionItem<object>(ImageManipuation.GetBitmapFromBytes(x.Icon), ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("TransactionListScreenViewModel.listItem_caption", DBContext.TransactionTexts.FirstOrDefault(y => y.tx_item == x.id)?.listItem_caption, "No Text"), x)).ToList();
                GetFirstPage();
            }
        }

        public void Cancel() => ApplicationViewModel.CancelSessionOnUserInput();

        public override void PerformSelection()
        {
            ErrorText = "Please login to access this transaction";
            if (!(SelectedFilteredList.Value is TransactionTypeListItem transactionTypeListItem))
                throw new NullReferenceException(GetType().Name + ".PerformSelection SelectedTransactionListItem");
            if (transactionTypeListItem.init_user_required)
                ApplicationViewModel.ShowDialog(new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, this, this, "USER_DEPOSIT", loginSuccessCallBack: new UserLoginViewModel.LoginSuccessCallBack(HandleLogin)));
            else
                PerformSelectionFinal();
        }

        private void HandleLogin(ApplicationUser applicationUser, bool isAuth)
        {
            if (!(SelectedFilteredList.Value is TransactionTypeListItem transactionTypeListItem))
                throw new NullReferenceException(GetType().Name + ".PerformSelection SelectedTransactionListItem");
            if (isAuth)
            {
                if (applicationUser != null)
                {
                    ApplicationViewModel.ValidatingUser = applicationUser;
                    PerformSelectionFinal();
                }
                else
                {
                    SelectedFilteredList = null;
                    ApplicationViewModel.CurrentUser = null;
                    ApplicationViewModel.ValidatingUser = null;
                }
            }
            else if (applicationUser != null)
            {
                ApplicationViewModel.CurrentUser = applicationUser;
                if (transactionTypeListItem.auth_user_required)
                    ApplicationViewModel.ShowDialog(new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, this, this, "USER_DEPOSIT", true, loginSuccessCallBack: new UserLoginViewModel.LoginSuccessCallBack(HandleLogin)));
                else
                    PerformSelectionFinal();
            }
            else
            {
                SelectedFilteredList = null;
                ApplicationViewModel.CurrentUser = null;
                ApplicationViewModel.ValidatingUser = null;
            }
        }

        private void PerformSelectionFinal()
        {
            TransactionTypeListItem SelectedTransactionListItem = SelectedFilteredList.Value as TransactionTypeListItem;
            if (SelectedTransactionListItem == null)
                throw new NullReferenceException(GetType().Name + ".PerformSelection SelectedTransactionListItem");
            using (new DepositorDBContext())
            {
                ClearErrorText();
                string str1 = "";
                string str2 = SelectedTransactionListItem?.default_account ?? "";
                if (!string.IsNullOrWhiteSpace(SelectedTransactionListItem.default_account) && SelectedTransactionListItem.validate_default_account)
                {
                    var result = Task.Run(() => ValidateAsync(SelectedTransactionListItem.default_account, SelectedTransactionListItem.default_account_currency, SelectedTransactionListItem.id)).Result;
                    if (result == null || !result.IsSuccess)
                    {
                        ErrorText = "Transaction Type is offline. Please try again later";
                        ApplicationViewModel.Log.ErrorFormat(GetType().Name, 99, ApplicationErrorConst.ERROR_TRANSACTION_ACCOUNT_INVALID.ToString(), "cb={0},sv={1}", result.PublicErrorMessage, result?.PublicErrorMessage);
                        ApplicationViewModel.CloseDialog(false);
                        return;
                    }
                    str1 = result.AccountName;
                    str2 = SelectedTransactionListItem.default_account;
                }
                ApplicationViewModel.CreateTransaction(SelectedTransactionListItem);
                ApplicationViewModel.CurrentTransaction.AccountNumber = str2;
                ApplicationViewModel.CurrentTransaction.AccountName = str1;
                ApplicationViewModel.CurrentTransaction.Transaction.init_user = ApplicationViewModel.CurrentUser?.id;
                ApplicationViewModel.CurrentTransaction.Transaction.auth_user = ApplicationViewModel.ValidatingUser?.id;
                CashSwiftTranslationService translationService = ApplicationViewModel.CashSwiftTranslationService;
                string str3;
                if (translationService == null)
                {
                    str3 = null;
                }
                else
                {
                    string s = translationService.TranslateUserText(GetType().Name + ".PerformSelection disclaimer", ApplicationViewModel?.CurrentTransaction?.TransactionType?.TransactionText?.disclaimer, null);
                    str3 = s != null ? s.CashSwiftReplace(ApplicationViewModel) : null;
                }
                string message = str3;
                string title = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(GetType().Name + ".PerformSelection", "sys_DisclaimerTitle_Caption", "Disclaimer");
                if (!string.IsNullOrWhiteSpace(message))
                    ApplicationViewModel.ShowUserMessageScreen(title, message);
                else
                    ApplicationViewModel.NavigateNextScreen();
            }
        }

        public async Task<AccountNumberValidationResponse> ValidateAsync(
          string accountNumber,
          string currency,
          int txType)
        {
            return await ApplicationViewModel.ValidateAccountNumberAsync(accountNumber, currency, txType);
        }
    }
}
