using Caliburn.Micro;
using CashSwift.Library.Standard.Statuses;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Linq;

namespace CashSwiftDeposit.ViewModels
{
    public class EscrowJamStatusReportScreenViewModel : FormViewModelBase
    {
        private bool canNext = false;

        public EscrowJamStatusReportScreenViewModel(
          ApplicationViewModel applicationViewModel,
          ICashSwiftWindowConductor conductor,
          object callingObject,
          bool isNewEntry)
          : base(applicationViewModel, conductor, callingObject, isNewEntry)
        {
            ScreenTitle = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(GetType().Name + ".Constructor ScreenTitle", "sys_EscrowJamFormScreenTitle", "Clear Escrow Jam");
            NextCaption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(GetType().Name + ".Constructor NextCaption", "sys_EndEscrowJamRecoveryCommand_Caption", "Complete");
            CancelCaption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.CancelCaption", "sys_CancelButton_Caption", "Cancel");
            BackCaption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.BackCaption", "sys_BackButton_Caption", "Back");
            ActivateItemAsync(new FormListViewModel(this));
            Activated += new EventHandler<ActivationEventArgs>(EscrowJamStatusReportScreenViewModel_Activated);
            ApplicationViewModel.DeviceManager.EscrowJamEndRequestEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamEndRequestEvent);
        }

        private void DeviceManager_EscrowJamEndRequestEvent(object sender, EventArgs e) => CanNext = true;

        private void EscrowJamStatusReportScreenViewModel_Activated(
          object sender,
          ActivationEventArgs e)
        {
            if (ApplicationViewModel.EscrowJam == null)
            {
                Transaction transaction = DBContext.Transactions.OrderByDescending(x => x.tx_start_date).FirstOrDefault();
                if (!transaction.tx_completed || transaction.tx_error_code == 85)
                {
                    ApplicationViewModel.EscrowJam = new EscrowJam()
                    {
                        id = Guid.NewGuid(),
                        date_detected = DateTime.Now
                    };
                    transaction.EscrowJams.Add(ApplicationViewModel.EscrowJam);
                    ApplicationViewModel.SaveToDatabase(DBContext);
                }
            }
            CanNext = ApplicationViewModel.DeviceManager.CurrentState == DeviceManagerState.ESCROWJAM_END_REQUEST;
            ApplicationViewModel.ClearEscrowJam();
        }

        public void Back() => ApplicationViewModel.ShowDialog(CallingObject);

        public void Cancel() => ApplicationViewModel.CloseDialog(true);

        public bool CanNext
        {
            get => canNext;
            set
            {
                canNext = value;
                NotifyOfPropertyChange(() => CanNext);
            }
        }

        public void Next()
        {
            CanNext = false;
            ApplicationViewModel.ShowDialog(new EscrowJamFormViewModel(ApplicationViewModel, Conductor, new OutOfOrderScreenViewModel(ApplicationViewModel), false));
        }
    }
}
