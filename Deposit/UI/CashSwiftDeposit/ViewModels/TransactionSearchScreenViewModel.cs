﻿// ViewModels.TransactionSearchScreenViewModel


using CashSwift.API.Messaging.CDM.GUIControl.AccountsLists;
using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.API.Messaging.Models;
using CashSwift.Library.Standard.Statuses;
using CashSwiftDataAccess.Data;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("65FA2238-C8E3-49DD-BC1B-C9EF19DD58BB")]
    internal class TransactionSearchScreenViewModel : CustomerSearchScreenBaseViewModel
    {
        public TransactionSearchScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            using (new DepositorDBContext())
            {
                ScrollList();
                PerformSearch();
            }
        }

        public void Cancel() => ApplicationViewModel.CancelSessionOnUserInput();

        public override void PerformSelection()
        {
            Account SelectedTransactionListItem = SelectedFilteredList.Value as Account;
            if (SelectedTransactionListItem == null)
                throw new NullReferenceException(GetType().Name + ".PerformSelection SelectedTransactionListItem");
            using (new DepositorDBContext())
            {
                string str1 = SelectedTransactionListItem?.account_name ?? "";
                string str2 = SelectedTransactionListItem?.account_number ?? "";
                if (ApplicationViewModel.CurrentTransaction.TransactionType.validate_default_account)
                {
                    var result = Task.Run(() => ValidateAsync(SelectedTransactionListItem.account_number, SelectedTransactionListItem.currency)).Result;
                    if (result == null || !result.IsSuccess)
                    {
                        ErrorText = result != null ? result?.PublicErrorMessage : "Transaction Type is offline. Please try again later";
                        ApplicationViewModel.Log.ErrorFormat(GetType().Name, 99, ApplicationErrorConst.ERROR_TRANSACTION_ACCOUNT_INVALID.ToString(), "cb={0},sv={1}", result.PublicErrorMessage, result?.PublicErrorMessage);
                        ApplicationViewModel.CloseDialog(false);
                        return;
                    }
                    str1 = result.AccountName;
                    str2 = SelectedTransactionListItem.account_number;
                }
                ApplicationViewModel.CurrentTransaction.AccountNumber = str2;
                ApplicationViewModel.CurrentTransaction.AccountName = str1;
                ApplicationViewModel.NavigateNextScreen();
            }
        }

        public async Task<AccountNumberValidationResponse> ValidateAsync(
          string accountNumber,
          string currency)
        {
            TransactionSearchScreenViewModel searchScreenViewModel = this;
            return await searchScreenViewModel.ApplicationViewModel.ValidateAccountNumberAsync(accountNumber, currency, searchScreenViewModel.ApplicationViewModel.CurrentTransaction.TransactionType.id);
        }
    }
}
