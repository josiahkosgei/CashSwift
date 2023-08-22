using Caliburn.Micro;

namespace CashSwiftDeposit.ViewModels
{
    internal class NoteJamScreenViewModel : Screen
    {
        private string _NoteJamErrorTitleText;
        private string _NoteJamErrorDescriptionText;
        private string clearNoteJamCaption;

        public ApplicationViewModel ApplicationViewModel { get; }

        public string NoteJamErrorTitleText
        {
            get => _NoteJamErrorTitleText;
            set
            {
                _NoteJamErrorTitleText = value;
                NotifyOfPropertyChange(() => NoteJamErrorTitleText);
            }
        }

        public string NoteJamErrorDescriptionText
        {
            get => _NoteJamErrorDescriptionText;
            set
            {
                _NoteJamErrorDescriptionText = value;
                NotifyOfPropertyChange(() => NoteJamErrorDescriptionText);
            }
        }

        public string ClearNoteJamCaption
        {
            get => clearNoteJamCaption;
            set
            {
                clearNoteJamCaption = value;
                NotifyOfPropertyChange(nameof(ClearNoteJamCaption));
            }
        }

        public NoteJamScreenViewModel(ApplicationViewModel applicationViewModel)
        {
            ApplicationViewModel = applicationViewModel;
            InitialiseScreen();
        }

        //protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken) => await base.OnDeactivateAsync(close, cancellationToken);

        private void InitialiseScreen()
        {
            NoteJamErrorDescriptionText = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("NoteJamErrorDescriptionText", "sys_NoteJamErrorDescriptionText", "Drop");
            NoteJamErrorTitleText = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("NoteJamErrorTitleText", "sys_NoteJamErrorTitleText", "Reject");
            ClearNoteJamCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("ClearNoteJamCaption", "sys_ClearNoteJamCaption", "Clear Jam");
        }

        public bool CanClearNoteJam => ApplicationViewModel.CanClearNoteJam;

        public void ClearNoteJam() => ApplicationViewModel.ClearNoteJam();
    }
}
