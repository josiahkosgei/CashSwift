// ViewModels.OutOfOrderScreenViewModel


using Caliburn.Micro;
using CashSwift.Library.Standard.Statuses;
using CashSwiftDeposit.Models;
using System;

namespace CashSwiftDeposit.ViewModels
{
    public class OutOfOrderScreenViewModel : Screen
    {
        public ApplicationViewModel ApplicationViewModel { get; }

        public string OutofOrderErrorTitleText
        {
            get
            {
                try
                {
                    return ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(OutofOrderErrorTitleText), "sys_OutofOrderErrorTitleText", ApplicationViewModel.CurrentLanguage) ?? throw new NullReferenceException("Invalid translation: TranslateSystemText is null");
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.WarningFormat(GetType().Name, nameof(OutofOrderErrorTitleText), "TranslationError", "Error translating: {0}>>{1}", ex.Message, ex?.InnerException?.Message);
                    return "[Translation Error]";
                }
            }
        }

        public string OutofOrderErrorDescriptionText
        {
            get
            {
                try
                {
                    return ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(OutofOrderErrorDescriptionText), "sys_OutofOrderErrorDescriptionText", ApplicationViewModel.CurrentLanguage) ?? throw new NullReferenceException("Invalid translation: TranslateSystemText is null");
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.WarningFormat(GetType().Name, nameof(OutofOrderErrorDescriptionText), "TranslationError", "Error translating: {0}>>{1}", ex.Message, ex?.InnerException?.Message);
                    return "[Translation Error]";
                }
            }
        }

        public OutOfOrderScreenViewModel(ApplicationViewModel applicationViewModel)
        {
            ApplicationViewModel = applicationViewModel;
            Activated += new EventHandler<ActivationEventArgs>(OutOfOrderScreenViewModel_Activated);
        }

        private void OutOfOrderScreenViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (UptimeMonitor.CurrentUptimeMode < UptimeModeType.OUT_OF_ORDER)
                UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.OUT_OF_ORDER);
            ApplicationViewModel.AdminMode = false;
            ApplicationViewModel.InitialiseUsersAndPermissions();
        }

        public void AdminButton()
        {
            ApplicationViewModel.AdminMode = true;
            MenuBackendATMViewModel backendAtmViewModel = new MenuBackendATMViewModel("Main Menu", ApplicationViewModel, ApplicationViewModel, this);
        }
    }
}
