using Caliburn.Micro;
using CashSwiftDataAccess.Data;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;

namespace CashSwiftDeposit.ViewModels
{
    internal class MenuUserManagementATMViewModel : ATMScreenViewModelBase
    {
        public MenuUserManagementATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
              ICashSwiftWindowConductor conductor,
          object callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>(MenuUserManagementATMViewModel_Activated);
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, ApplicationViewModel.CurrentUser, "USER_MANAGEMENT_MENU_SHOW", false))
                ErrorText = string.Format("User permission rejected to user {0} for activity {1}, navigating to previous menu.", applicationViewModel.CurrentUser?.username, "USER_MANAGEMENT_MENU_SHOW");
            else
                ApplicationViewModel.ShowDialog(this);
        }

        private void MenuUserManagementATMViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (isInitialised)
                return;
            if (!ApplicationViewModel.CurrentUser.is_ad_user && ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "USER_CHANGE_PASSWORD"))
            {
                using (new DepositorDBContext())
                    Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/pin-code.png", ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuUserManagementATMViewModel_Activated), "sys_User_ChangePasswordCommand_Caption", "Change Password", ApplicationViewModel.CurrentLanguage), new UserChangePasswordFormViewModel(ApplicationViewModel, ApplicationViewModel.CurrentUser, null, Conductor, CallingObject, CallingObject)));
            }
            isInitialised = true;
        }
    }
}