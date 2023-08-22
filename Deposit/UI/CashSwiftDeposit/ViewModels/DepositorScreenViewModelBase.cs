using CashSwiftDataAccess.Data;
using CashSwiftDeposit.ViewModels.RearScreen;

namespace CashSwiftDeposit.ViewModels
{
    public class DepositorScreenViewModelBase : Caliburn.Micro.Conductor<object>
    {
        public ApplicationViewModel ApplicationViewModel;
        protected DepositorDBContext DBContext = new DepositorDBContext();

        protected string ScreenTitle { get; set; }

        protected object CallingObject { get; set; }

        public ICashSwiftWindowConductor Conductor { get; }

        public string CancelButton_Caption { get; set; }

        public string BackButton_Caption { get; set; }

        public string NextButton_Caption { get; set; }

        public string GetFirstPageButton_Caption { get; set; }

        public string GetPreviousPageButton_Caption { get; set; }

        public string GetNextPageButton_Caption { get; set; }

        public string GetLastPageButton_Caption { get; set; }

        public DepositorScreenViewModelBase(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          object callingObject,
          ICashSwiftWindowConductor conductor)
        {
            ScreenTitle = screenTitle;
            ApplicationViewModel = applicationViewModel;
            CallingObject = callingObject;
            Conductor = conductor;
            CancelButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.CancelButton_Caption", "sys_CancelButton_Caption", "Cancel");
            BackButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.BackButton_Caption", "sys_BackButton_Caption", "Back");
            NextButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.BackButton_Caption", "sys_NextButton_Caption", "Next");
            GetFirstPageButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetPreviousPageButton_Caption", "sys_GetFirstPageButton_Caption", "First");
            GetPreviousPageButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetPreviousPageButton_Caption", "sys_GetPreviousPageButton_Caption", "Prev");
            GetNextPageButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetNextPageButton_Caption", "sys_GetNextPageButton_Caption", "More");
            GetLastPageButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetNextPageButton_Caption", "sys_GetLastPageButton_Caption", "Last");
        }

        public void Back() => Conductor.ShowDialog(CallingObject);

        public void Cancel() => Conductor.CloseDialog();
    }
}
