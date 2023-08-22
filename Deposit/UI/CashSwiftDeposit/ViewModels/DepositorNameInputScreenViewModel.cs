using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("1DA2D457-711D-429D-B755-E9B44E1F33B1")]
    public class DepositorNameInputScreenViewModel : CustomerPrepopReferenceScreenBase
    {
        public DepositorNameInputScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(applicationViewModel?.CurrentTransaction?.DepositorName, screenTitle, applicationViewModel, required)
        {
            AlphanumericKeyboardIsVisible = true;
        }

        public override bool AlphanumericKeyboardIsVisible
        {
            get
            {
                return base.AlphanumericKeyboardIsVisible;
            }
        }

        public override bool FullAlphanumericKeyboardIsVisible
        {
            get
            {
                return base.FullAlphanumericKeyboardIsVisible;
            }
        }

        public override bool NumericKeypadIsVisible
        {
            get
            {
                return base.NumericKeypadIsVisible;
            }
        }
        public void Cancel() => ApplicationViewModel.CancelSessionOnUserInput();

        public void Back()
        {
            ApplicationViewModel.CurrentTransaction.DepositorName = CustomerInput;
            ApplicationViewModel.NavigatePreviousScreen();
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
            if (Validate())
            {
                ApplicationViewModel.CurrentTransaction.DepositorName = CustomerInput;
                ApplicationViewModel.NavigateNextScreen();
            }
            else
            {
                ApplicationViewModel.CloseDialog(false);
                CanNext = true;
            }
        }

        private bool Validate() => ClientValidation(CustomerInput);
    }
}
