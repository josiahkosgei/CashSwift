using Caliburn.Micro;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Windows;

namespace CashSwiftDeposit.ViewModels
{
    internal class MenuTransactionATMViewModel : ATMScreenViewModelBase
    {
        public MenuTransactionATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
              ICashSwiftWindowConductor conductor,
          object callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>(MenuTransactionViewModel_Activated);
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, applicationViewModel.CurrentUser, "TRANSACTION_MENU_SHOW", false))
                ErrorText = string.Format("User permission rejected to user {0} for activity {1}, navigating to previous menu.", applicationViewModel.CurrentUser?.username, "TRANSACTION_MENU_SHOW");
            else
                ApplicationViewModel.ShowDialog(this);
        }

        private void MenuTransactionViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (!isInitialised)
            {
                TransactionReportScreenViewModel nextObject = new TransactionReportScreenViewModel(Application.Current.FindResource("TransactionListScreenTitle") as string, ApplicationViewModel, this, Conductor);
                object obj = ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel?.CurrentUser, "TRANSACTION_LIST_VIEW") ? nextObject : (object)new UserLoginViewModel(ApplicationViewModel, Conductor, CallingObject, nextObject, "TRANSACTION_LIST_VIEW");
                Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/coins.png", Application.Current.FindResource("TransactionlistCommand_Caption") as string, obj));
            }
            isInitialised = true;
        }
    }
}