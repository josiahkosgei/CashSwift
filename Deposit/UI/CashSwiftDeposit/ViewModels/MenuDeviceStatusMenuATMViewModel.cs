using Caliburn.Micro;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CashSwiftDeposit.ViewModels
{
    internal class MenuDeviceStatusMenuATMViewModel : ATMScreenViewModelBase
    {
        public MenuDeviceStatusMenuATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
              ICashSwiftWindowConductor conductor,
          object callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>(MenuDeviceStatusATMViewModel_Activated);
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, ApplicationViewModel.CurrentUser, "DEVICESTATUS_MENU_SHOW", false))
                ErrorText = string.Format("User permission rejected to user {0} for activity {1}, navigating to previous menu.", applicationViewModel.CurrentUser?.username, "DEVICESTATUS_MENU_SHOW");
            else
                ApplicationViewModel.ShowDialog(this);
        }

        private void MenuDeviceStatusATMViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (isInitialised)
                return;
            Permission userPermission = ApplicationViewModel.GetUserPermission(ApplicationViewModel.CurrentUser, "DEVICE_SUMMARY");
            if (userPermission != null)
            {
                DeviceStatusReportScreenViewModel nextObject = new DeviceStatusReportScreenViewModel(Application.Current.FindResource("DeviceStatusScreenTitle") as string, ApplicationViewModel, (object)this, Conductor);
                object obj = !userPermission.standalone_authentication_required ? nextObject : (object)new UserLoginViewModel(ApplicationViewModel, Conductor, CallingObject, (object)nextObject, "DEVICE_SUMMARY", true);
                Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/presentation-5.png", Application.Current.FindResource("DeviceSummaryCommand_Caption") as string, obj));
            }
            if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "DEVICE_CONTROLLER_SHOW"))
                Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/settings-2.png", Application.Current.FindResource("ShowDeviceControllerCommand_Caption") as string, new AdminButtonCommandATMScreenCommandViewModel(ATMMenuCommandButton.Controller_ShowController, "", ApplicationViewModel, Conductor, this)));
            if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "DEVICE_POWER_MENU_SHOW"))
            {
                IList<ATMSelectionItem<object>> screens = Screens;
                if (!(Application.Current.FindResource("DevicePowerMenuScreenTitle_Caption") is string selectionText))
                    selectionText = "Device Power Management";
                if (!(Application.Current.FindResource("DevicePowerMenuScreenTitle_Caption") is string screenTitle))
                    screenTitle = "Device Power Management";
                MenuDeviceShutdownMenuATMViewModel menuAtmViewModel = new MenuDeviceShutdownMenuATMViewModel(screenTitle, ApplicationViewModel, Conductor, this);
                ATMSelectionItem<object> atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/pie-chart-5.png", selectionText, menuAtmViewModel);
                screens.Add(atmSelectionItem);
            }
            isInitialised = true;
        }
    }
}
