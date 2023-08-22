using Caliburn.Micro;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CashSwiftDeposit.ViewModels.RearScreen
{
    public class RearScreenShellViewModel : Conductor<object>, ICashSwiftWindowConductor
    {
        private IWindowManager windowManager;

        private RearScreenMainViewModel RearScreenMainViewModel { get; set; }

        public RearScreenShellViewModel(
          IWindowManager theWindowManager,
          ApplicationViewModel applicationViewModel)
        {
            windowManager = theWindowManager;
            ApplicationViewModel = applicationViewModel;
            ApplicationViewModel.PropertyChanged += new PropertyChangedEventHandler(ApplicationViewModel_PropertyChanged);
            RearScreenMainViewModel = new RearScreenMainViewModel(this, applicationViewModel);
            ActivateItemAsync(RearScreenMainViewModel);
        }

        public ApplicationViewModel ApplicationViewModel { get; }

        private void ApplicationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(e.PropertyName == "AdminMode"))
                return;
            if (ApplicationViewModel.AdminMode)
            {
                MenuBackendATMViewModel backendAtmViewModel = new MenuBackendATMViewModel("Main Menu", ApplicationViewModel, this, null);
                ApplicationViewModel.ShowDialogBox(new OutOfOrderFatalScreenViewModel());
            }
            else
                CloseDialog(true);
        }

        // public override async  void ActivateItemAsync(object item, CancellationToken cancellationToken)
        //protected override Task OnActivateAsync(object item, CancellationToken cancellationToken)
        //{
        //    ApplicationViewModel.Log.TraceFormat(nameof(RearScreenShellViewModel), "Init", "ShowDialogBox", "activating item {0}", (object)JsonConvert.SerializeObject(item, Formatting.Indented, new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore}));
        //    base.ActivateItemAsync(item, cancellationToken);
        //}

        public void CloseDialog(bool generateScreen = true)
        {
            ApplicationViewModel.Log.InfoFormat(nameof(RearScreenShellViewModel), "Init", nameof(CloseDialog), "showing screen master screen");
            ActivateItemAsync(RearScreenMainViewModel);
            ApplicationViewModel.CloseDialog(true);
        }

        public void ShowDialog(object screen)
        {
            ApplicationViewModel.Log.InfoFormat(nameof(RearScreenShellViewModel), "Init", nameof(ShowDialog), "showing screen {0}", JsonConvert.SerializeObject(screen, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (ApplicationViewModel.AdminMode)
            {
                ActivateItemAsync(screen);
            }
            else
            {
                ApplicationViewModel.Log.Warning(nameof(RearScreenShellViewModel), "Invalid  sceen navigation", nameof(ShowDialog), "Not in AdminMode: Cannot show screen " + JsonConvert.SerializeObject(screen, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                ApplicationViewModel.AdminMode = false;
            }
        }

        public void ShowDialogBox(object screen)
        {
            ApplicationViewModel.Log.InfoFormat(nameof(RearScreenShellViewModel), "Init", nameof(ShowDialogBox), "showing screen {0}", JsonConvert.SerializeObject(screen, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            ActivateItemAsync(screen);
        }
    }
}
