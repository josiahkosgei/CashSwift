using Caliburn.Micro;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.ViewModels.RearScreen;
using DeviceManager;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("C0AC72F5-37FF-4CC8-BE9F-842A3787D393")]
    internal class MenuBackendATMViewModel : ATMScreenViewModelBase
    {
        public MenuBackendATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
              ICashSwiftWindowConductor Conductor,
          object callingObject)
          : base(screenTitle, applicationViewModel, Conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>(MenuBackendATMViewModel_Activated);
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, applicationViewModel.CurrentUser, "BACKEND_MENU_SHOW", false))
            {
                UserLoginViewModel screen = new UserLoginViewModel(ApplicationViewModel, Conductor, CallingObject, this, "BACKEND_MENU_SHOW");
                Conductor.ShowDialog(screen);
            }
            else
            {
                Conductor.ShowDialog(this);
            }
        }

        private void MenuBackendATMViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (isInitialised)
                return;
            if (ApplicationViewModel.DeviceManager.DeviceManagerMode == DeviceManagerMode.ESCROW_JAM)
            {
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel?.CurrentUser, "ESCROWJAM_INITIALISER"))
                {
                    EscrowJamStatusReportScreenViewModel nextObject = new EscrowJamStatusReportScreenViewModel(ApplicationViewModel, Conductor, this, true);
                    var userLoginViewModel = new UserLoginViewModel(ApplicationViewModel, Conductor, CallingObject, nextObject, "ESCROWJAM_AUTHORISER", splitAuthorise: true);

                    MenuDeviceStatusMenuATMViewModel MenuDeviceStatusMenuATMViewModel = new MenuDeviceStatusMenuATMViewModel(ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_DeviceStatusScreenTitle", "Device Management", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this);

                    ATMSelectionItem<object> atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/clear_escrow_jam.png", ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_EscrowJamCommand_Caption", "Clear Escrow Jam"), userLoginViewModel);
                    Screens.Add(atmSelectionItem);

                }

                isInitialised = true;
                Conductor.ShowDialog(this);
            }
            else
            {
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "DEVICESTATUS_MENU_SHOW"))
                {
                    MenuDeviceStatusMenuATMViewModel managementAtmViewModel = new MenuDeviceStatusMenuATMViewModel(ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_DeviceStatusScreenTitle", "Device Management", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this);
                    ATMSelectionItem<object> atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/pie-chart-5.png", ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_DeviceStatusScreenTitle", "Device Management", ApplicationViewModel.CurrentLanguage), managementAtmViewModel);
                    Screens.Add(atmSelectionItem);
                }
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "TRANSACTION_MENU_SHOW"))
                {
                    MenuTransactionATMViewModel managementAtmViewModel = new MenuTransactionATMViewModel(ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_TransactionManagementScreenTitle", "Transactions", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this);
                    ATMSelectionItem<object> atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/change.png", ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_TransactionManagementScreenTitle", "Transactions", ApplicationViewModel.CurrentLanguage), managementAtmViewModel);
                    Screens.Add(atmSelectionItem);
                }
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "CIT_MENU_SHOW"))
                {
                    MenuCITManagementATMViewModel managementAtmViewModel = new MenuCITManagementATMViewModel(ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_CITManagementScreenTitle", "CIT Management", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this);
                    ATMSelectionItem<object> atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/safebox.png", ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_CITManagementScreenTitle", "CIT Management", ApplicationViewModel.CurrentLanguage), managementAtmViewModel);
                    Screens.Add(atmSelectionItem);
                }
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "USER_MANAGEMENT_MENU_SHOW"))
                {
                    MenuUserManagementATMViewModel managementAtmViewModel = new MenuUserManagementATMViewModel(ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_UserManagementScreenTitle_Caption", "User Management", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this);
                    ATMSelectionItem<object> atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/users-1.png", ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_UserManagementScreenTitle_Caption", "User Management", ApplicationViewModel.CurrentLanguage), managementAtmViewModel);
                    if (managementAtmViewModel.Screens.Count() > 0)
                        Screens.Add(atmSelectionItem);
                }

                isInitialised = true;
                Conductor.ShowDialog(this);

            }

        }
    }
}
