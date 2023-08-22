using CashSwift.Library.Standard.Statuses;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace CashSwiftDeposit.ViewModels
{
    public class DeviceStatusReportScreenViewModel : DepositorScreenViewModelBase
    {
        private DispatcherTimer dispTimer = new DispatcherTimer(DispatcherPriority.Send, Application.Current.Dispatcher);

        public string MachineName => Environment.MachineName;

        public string CashSwiftGUIVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string DeviceManagerVersion { get; set; }

        public string CashSwiftUtilVersion => Assembly.GetAssembly(typeof(XMLSerialization)).GetName().Version.ToString();

        public string ControllerStatus { get; set; }

        public string TransactionStatus { get; set; }

        public string BAStatus { get; set; }

        public string BAType { get; set; }

        public string BagStatus { get; set; }

        public string BagNumber { get; set; }

        public string BagPercentFull { get; set; }

        public string BagNoteLevel { get; set; }

        public string BagNoteCapacity { get; set; }

        public string SensorBag { get; set; }

        public string SensorDoor { get; set; }

        public string EscrowType { get; set; }

        public string EscrowStatus { get; set; }

        public string EscrowPosition { get; set; }

        public string ApplicationStatus { get; set; }

        public string ApplicationState { get; set; }

        public string DeviceState { get; set; }

        public DeviceStatusReportScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          object callingObject,
          ICashSwiftWindowConductor conductor)
          : base(screenTitle, applicationViewModel, callingObject, conductor)
        {
            DeviceManagerVersion = applicationViewModel.DeviceManager.DeviceManagerVersion.ToString();
            InitialiseDeviceReport(applicationViewModel);
            dispTimer.Interval = TimeSpan.FromSeconds(1.0);
            dispTimer.Tick += new EventHandler(dispTimer_Tick);
            dispTimer.IsEnabled = true;
        }

        private void dispTimer_Tick(object sender, EventArgs e)
        {
            InitialiseDeviceReport(ApplicationViewModel);
            NotifyOfPropertyChange("ControllerStatus");
            NotifyOfPropertyChange("TransactionStatus");
            NotifyOfPropertyChange("BAStatus");
            NotifyOfPropertyChange("BAType");
            NotifyOfPropertyChange("BagStatus");
            NotifyOfPropertyChange("BagNumber");
            NotifyOfPropertyChange("BagPercentFull");
            NotifyOfPropertyChange("BagNoteLevel");
            NotifyOfPropertyChange("BagNoteCapacity");
            NotifyOfPropertyChange("SensorBag");
            NotifyOfPropertyChange("SensorDoor");
            NotifyOfPropertyChange("EscrowType");
            NotifyOfPropertyChange("EscrowStatus");
            NotifyOfPropertyChange("EscrowPosition");
            NotifyOfPropertyChange("ApplicationStatus");
            NotifyOfPropertyChange("DeviceState");
        }

        private void InitialiseDeviceReport(ApplicationViewModel applicationViewModel)
        {
            ControllerStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.ControllerState.ToString()?.ToUpper();
            TransactionStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Transaction?.Status.ToString()?.ToUpper();
            BAStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.NoteAcceptor?.Status.ToString()?.ToUpper();
            BAType = applicationViewModel?.ApplicationStatus?.ControllerStatus?.NoteAcceptor?.Type.ToString()?.ToUpper();
            BagStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.BagState.ToString()?.ToUpper();
            BagNumber = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.BagNumber?.ToString()?.ToUpper();
            BagPercentFull = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.PercentFull.ToString()?.ToUpper();
            BagNoteLevel = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.NoteLevel.ToString()?.ToUpper();
            BagNoteCapacity = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.NoteCapacity.ToString()?.ToUpper();
            SensorBag = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Sensor?.Bag.ToString()?.ToUpper();
            SensorDoor = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Sensor?.Door.ToString()?.ToUpper();
            EscrowType = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Escrow?.Type.ToString()?.ToUpper();
            EscrowStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Escrow?.Status.ToString()?.ToUpper();
            EscrowPosition = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Escrow?.Position.ToString()?.ToUpper();
            ApplicationStatus = applicationViewModel?.CurrentApplicationState.ToString()?.ToUpper();
            ApplicationState = applicationViewModel?.ApplicationStatus?.CashSwiftDeviceState.ToString()?.ToUpper();
            DeviceState = applicationViewModel?.DeviceManager?.CurrentState.ToString()?.ToUpper();
        }
    }
}
