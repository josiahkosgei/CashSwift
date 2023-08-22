using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("60768315-94CE-4E14-B26B-3D57212FC9CE")]
    public class NarrationScreenInputScreenViewModel : CustomerPrepopReferenceScreenBase
    {
        public NarrationScreenInputScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(applicationViewModel?.CurrentTransaction?.Narration, screenTitle, applicationViewModel, required)
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
            ApplicationViewModel.CurrentTransaction.Narration = CustomerInput;
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
                ApplicationViewModel.CurrentTransaction.Narration = CustomerInput;
                ApplicationViewModel.NavigateNextScreen();
            }
            else
            {
                ApplicationViewModel.CloseDialog(false);
                CanNext = true;
            }
        }

        public bool Validate() => ClientValidation(CustomerInput);
    }
}
