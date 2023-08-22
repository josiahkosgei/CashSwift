using Caliburn.Micro;
using CashAccSysDeviceManager;
using CashSwift.API.Messaging;
using CashSwift.API.Messaging.CDM.GUIControl.AccountsLists;
using CashSwift.API.Messaging.CDM.GUIControl.Clients;
using CashSwift.API.Messaging.Integration;
using CashSwift.API.Messaging.Integration.Controllers;
using CashSwift.API.Messaging.Integration.ServerPing;
using CashSwift.API.Messaging.Integration.Transactions;
using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using CashSwift.API.Messaging.Models;
using CashSwift.Library.Standard.Logging;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.Models.Submodule;
using CashSwiftDeposit.UserControls;
using CashSwiftDeposit.Utils;
using CashSwiftDeposit.Utils.AlertClasses;
using CashSwiftDeposit.ViewModels.RearScreen;
using CashSwiftUtil.Licensing;
using DeviceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Activity = CashSwiftDataAccess.Entities.Activity;
using DeviceManagerMode = DeviceManager.DeviceManagerMode;

namespace CashSwiftDeposit.ViewModels
{
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
    public class ApplicationViewModel : Conductor<object>, ICashSwiftWindowConductor
    {

        public bool DebugNoDevice { get; set; } = false;
        public bool DebugNoCoreBanking { get; set; } = false;
        public bool DebugDisableSafeSensor { get; set; } = false;
        public bool DebugDisableBagSensor { get; set; } = false;
        public bool DebugDisablePrinter { get; set; } = false;
        private bool _adminMode = false;

        private ApplicationUser _currentUser;
        private ApplicationUser _validatingUser;
        private CashAccSysDeviceManager.CashAccSysDeviceManager _deviceManager;
        private ApplicationState _currentApplicationState = ApplicationState.STARTUP;
        private DispatcherTimer statusTimer = new DispatcherTimer(DispatcherPriority.Send);


        public static CashSwiftTranslationService CashSwiftTranslationService { get; set; }

        public object NavigationLock { get; set; } = new object();

        public object BagOpenLock { get; private set; } = new object();

        public object BagReplacedLock { get; private set; } = new object();

        public object DoorClosedLock { get; private set; } = new object();

        internal EscrowJam EscrowJam { get; set; }


        public bool AdminMode
        {
            get => _adminMode;
            set
            {
                if (_adminMode == value)
                    return;
                _adminMode = value;
                try
                {
                    if (value)
                        UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.ADMIN);
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(ApplicationViewModel), ex.GetType().Name, nameof(AdminMode), ex.MessageString(), Array.Empty<object>());
                }
                NotifyOfPropertyChange(() => AdminMode);
            }
        }

        private DepositorPrinter Printer { get; set; }

        public static DepositorLogger Log { get; set; }

        public static DepositorLogger AlertLog { get; set; }

        public Random Rand { get; }

        public string CurrentLanguage { get; private set; } = "en-GB";

        public ApplicationUser CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                NotifyOfPropertyChange(() => CurrentUser);
            }
        }

        public ApplicationUser ValidatingUser
        {
            get => _validatingUser;
            set
            {
                _validatingUser = value;
                NotifyOfPropertyChange(() => ValidatingUser);
            }
        }

        public ApplicationModel ApplicationModel { get; }

        public AppSession CurrentSession { get; set; }

        public AppTransaction CurrentTransaction => CurrentSession?.Transaction;

        public CashAccSysDeviceManager.CashAccSysDeviceManager CashAccSysDeviceManager { get; private set; }

        //public IDeviceManager DeviceManager
        //{
        //    get => _deviceManager;
        //    set
        //    {
        //        _deviceManager = value;
        //        CashAccSysDeviceManager = value as CashAccSysDeviceManager.CashAccSysDeviceManager;
        //    }
        //}
        public CashAccSysDeviceManager.CashAccSysDeviceManager DeviceManager
        {
            get => _deviceManager;
            set
            {
                _deviceManager = value;
                CashAccSysDeviceManager = value as CashAccSysDeviceManager.CashAccSysDeviceManager;
            }
        }
        public List<Language> LanguagesAvailable => ApplicationModel.LanguageList;

        public List<Currency> CurrenciesAvailable => ApplicationModel.CurrencyList;

        public List<TransactionTypeListItem> TransactionTypesAvailable => ApplicationModel.TransactionList;

        public ApplicationState CurrentApplicationState
        {
            get => _currentApplicationState;
            set
            {
                if (DeviceManager != null)
                {
                    switch (DeviceManager.DeviceManagerMode)
                    {
                        case DeviceManagerMode.NONE:
                            if (value.ToString().StartsWith("CIT"))
                            {
                                DeviceManager.DeviceManagerMode = DeviceManagerMode.CIT;
                                break;
                            }
                            break;
                        case DeviceManagerMode.CIT:
                            if (!value.ToString().StartsWith("CIT"))
                            {
                                DeviceManager.DeviceManagerMode = DeviceManagerMode.NONE;
                                break;
                            }
                            break;
                    }
                }
                Log.DebugFormat(GetType().Name, "ApplicationStateChanged", "ApplicationState", "Changed from {0} to {1}", _currentApplicationState, value);
                _currentApplicationState = value;
                NotifyOfPropertyChange(() => CurrentApplicationState);
                switch (value)
                {
                    case ApplicationState.SPLASH:
                        UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.ACTIVE);
                        break;
                    case ApplicationState.CIT_START:
                        UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.CIT);
                        break;
                    case ApplicationState.CIT_END:
                        UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.OUT_OF_ORDER);
                        break;
                }
            }
        }

        public CashSwiftDeviceStatus ApplicationStatus { get; set; } = new CashSwiftDeviceStatus();

        public CIT lastCIT
        {
            get
            {
                using (DepositorDBContext DBContext = new DepositorDBContext())
                {
                    Device device = ApplicationModel.GetDevice(DBContext);
                    return DBContext.CITs.Where(y => y.device_id == device.id).OrderByDescending(x => x.cit_date).FirstOrDefault();
                }
            }
        }

        public AlertManager AlertManager { get; set; }

        public static DeviceConfiguration DeviceConfiguration { get; set; }

        public LicenseMechanism License { get; private set; }

        private StartupViewModel StartupViewModel { get; }

        public ApplicationViewModel(StartupViewModel startupModel)
        {
            PropertyChanged += new PropertyChangedEventHandler(ApplicationViewModel_PropertyChanged);
            ApplicationStatus.PropertyChanged += new PropertyChangedEventHandler(ApplicationStatus_PropertyChanged);
            StartupViewModel = startupModel;
            Log = new DepositorLogger(this);
            DeviceConfiguration = DeviceConfiguration.Initialise();
            AlertLog = new DepositorLogger(this, "DepositorCommunicationService");
            AlertManager = startupModel.AlertManager;
            AlertManager.Log = Log;
            Log.Info(GetType().Name, "Application Startup", "Constructor", "Initialising Application");
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                InitialiseLicense();
                statusTimer.Interval = TimeSpan.FromSeconds(DeviceConfiguration.SERVER_POLL_INTERVAL);
                statusTimer.Tick += new EventHandler(statusTimer_Tick);
                statusTimer.IsEnabled = true;
                ApplicationModel = new ApplicationModel(this);
                ApplicationModel.DatabaseStorageErrorEvent += new EventHandler<EventArgs>(ApplicationModel_DatabaseStorageErrorEvent);
                SetCashSwiftDeviceState(CashSwiftDeviceState.DEVICE_MANAGER);
                InitialiseApp();
                Rand = new Random();
                CurrentApplicationState = ApplicationState.STARTUP_COMPLETE;
                if (CurrentApplicationState == ApplicationState.STARTUP_COMPLETE && ApplicationStatus.CashSwiftDeviceState == CashSwiftDeviceState.NONE)
                {
                    Log.Info(GetType().Name, "Application Startup", "ApplicationState.STARTUP_COMPLETE", "Application started successfully");
                    AlertManager.SendAlert(new AlertDeviceStartupSuccess(ApplicationModel.GetDevice(DBContext), DateTime.Now));
                }
                Debug();
                InitialiseSecondScreen();
            }
        }

        private void ApplicationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (DeviceConfiguration.USE_REAR_SCREEN || !(e.PropertyName == "AdminMode"))
                return;
            if (AdminMode)
            {
                MenuBackendATMViewModel backendAtmViewModel = new MenuBackendATMViewModel("Main Menu", this, this, null);
                ShowDialogBox(new OutOfOrderFatalScreenViewModel());
            }
            else
                CloseDialog(true);
        }

        private void statusTimer_Tick(object sender, EventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = false
            };
            backgroundWorker.DoWork += new DoWorkEventHandler(statusWorker_DoWork);
            backgroundWorker.RunWorkerAsync();
        }

        private void Debug()
        {
        }

        private void InitialiseSecondScreen()
        {
        }

        private void InitialiseLicense()
        {
            using (new DepositorDBContext())
            {
                CashSwiftTranslationService = new CashSwiftTranslationService(this, License?.License);
                try
                {
                    License = new LicenseMechanism();
                    CashSwiftTranslationService = new CashSwiftTranslationService(this, License?.License);
                    UnSetCashSwiftDeviceState(CashSwiftDeviceState.LICENSE);
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(ApplicationViewModel), 1, "License Error", ex.MessageString());
                    SetCashSwiftDeviceState(CashSwiftDeviceState.LICENSE);
                    NotifyOfPropertyChange(() => ApplicationStatus);
                    NotifyOfPropertyChange(() => CurrentApplicationState);
                }
            }
        }

        public void InitialiseApp()
        {
            Log.Info(GetType().Name, nameof(InitialiseApp), "Initialisation", "Initialising Application");
            CheckHDDSpace();
            InitialiseLicense();
            InitialiseDevice();
            if (CurrentApplicationState == ApplicationState.STARTUP)
            {
                SystemStartupChecks();
                InitialiseEmailManager();
                if (!DebugDisablePrinter)
                    InitialisePrinter();
            }
            AlertManager?.InitialiseAlertManager();
            InitialiseUsersAndPermissions();
            InitialiseFolders();
            CurrentScreenIndex = 0;
            InitialiseCurrencyList();
            InitialiseScreenList();
            InitialiseLanguageList();
            InitialiseTransactionTypeList();
            InitialiseCoreBankingAsync().AsResult();
            SetLanguage(DeviceConfiguration.UI_CULTURE);
            ConnectToDevice();
            HandleIncompleteSession();
            NavigateFirstScreen();
        }

        private void statusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitialiseLicense();
            InitialiseCoreBankingAsync().AsResult();
            CheckDeviceLockStatus();
            CheckHDDSpace();
        }

        private void CheckHDDSpace()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                DriveInfo driveInfo = DriveInfo.GetDrives().FirstOrDefault(x => x.RootDirectory.FullName.Equals("c:\\", StringComparison.InvariantCultureIgnoreCase));
                if (driveInfo == null)
                    return;
                Console.WriteLine(driveInfo.Name);
                if (driveInfo.IsReady)
                {
                    long num = 1073741824;
                    if (driveInfo.AvailableFreeSpace < num)
                    {
                        if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.HDD_FULL))
                        {
                            SetCashSwiftDeviceState(CashSwiftDeviceState.HDD_FULL);
                            double availableSpace = driveInfo.AvailableFreeSpace / 1073741824.0;
                            double minimumSpace = num / 1073741824.0;
                            Log.InfoFormat(GetType().Name, "Device", "HDD FULL", "Setting CashSwiftDeviceState.HDD_FULL as HDD space of {0:0.##} GB < {1:0.##} GB", availableSpace, minimumSpace);
                            AlertManager.SendAlert(new AlertHDDFull(availableSpace, minimumSpace, ApplicationModel.GetDevice(DBContext), DateTime.Now));
                        }
                    }
                    else if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.HDD_FULL))
                        UnSetCashSwiftDeviceState(CashSwiftDeviceState.HDD_FULL);
                }
            }
        }

        private void UnSetCashSwiftDeviceState(CashSwiftDeviceState state)
        {
            Log.Debug(GetType().Name, "Unset", "CashSwiftDeviceState", "{0}", new object[1]
      {
         state.ToString()
      });
            ApplicationStatus.CashSwiftDeviceState &= ~state;
            UptimeMonitor.UnSetCurrentUptimeComponentState(state);
        }

        private void SetCashSwiftDeviceState(CashSwiftDeviceState state)
        {
            Log.Debug(GetType().Name, "Set", "CashSwiftDeviceState", "{0}", new object[1]
      {
         state.ToString()
      });
            ApplicationStatus.CashSwiftDeviceState |= state;
            UptimeMonitor.SetCurrentUptimeComponentState(state);
        }

        private void CheckDeviceLockStatus()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Device device = ApplicationModel.GetDevice(DBContext);
                if (device == null)
                    return;
                DeviceManager.Enabled = device.enabled;
            }
        }

        private async Task InitialiseCoreBankingAsync()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                if (DebugNoCoreBanking)
                {
                    if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                        UnSetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                }
                else
                {
                    AppSession currentSession = CurrentSession;
                    bool allowConnectionError = currentSession == null || !currentSession.CountingStarted;
                    try
                    {
                        Device device = ApplicationModel.GetDevice(DBContext);
                        Guid deviceID = device.id;
                        Guid app_id = ApplicationModel.GetDevice(DBContext).app_id;
                        IntegrationServiceClient IntegrationClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, app_id, device.app_key, null);
                        IntegrationServiceClient integrationServiceClient = IntegrationClient;
                        IntegrationServerPingRequest request = new IntegrationServerPingRequest();
                        Guid guid = Guid.NewGuid();
                        request.SessionID = guid.ToString();
                        guid = Guid.NewGuid();
                        request.MessageID = guid.ToString();
                        request.AppID = app_id;
                        request.AppName = device.machine_name;
                        request.Language = CurrentLanguage;
                        request.MessageDateTime = DateTime.Now;
                        IntegrationServerPingResponse response = await integrationServiceClient.ServerPingAsync(request);
                        CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                        ApplicationStatus.CoreBankingStatus = new CoreBankingStatus()
                        {
                            ServerOnline = response.ServerOnline
                        };
                        int num;
                        if (allowConnectionError && !response.ServerOnline)
                        {
                            CashSwiftDeviceStatus applicationStatus = ApplicationStatus;
                            num = (applicationStatus != null ? (applicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION) ? 1 : 0) : 0) == 0 ? 1 : 0;
                        }
                        else
                            num = 0;
                        if (num != 0)
                        {
                            Log.ErrorFormat(GetType().Name, 92, ApplicationErrorConst.ERROR_CORE_BANKING.ToString(), "Could not connect to core banking with error: {0}>Server Error>{1}", response.PublicErrorMessage, response.ServerErrorMessage);
                            if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                                Log.Debug(GetType().Name, "Device", "Device State Changed", "Setting CashSwiftDeviceState.SERVER_CONNECTION");
                        }
                        else if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                            UnSetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                        device = null;
                        deviceID = new Guid();
                        app_id = new Guid();
                        IntegrationClient = null;
                        response = null;
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat(GetType().Name, 92, ApplicationErrorConst.ERROR_CORE_BANKING.ToString(), "Could not connect to core banking with exception: {0}>>{1}", ex.Message, ex?.InnerException?.Message);
                        if (allowConnectionError)
                        {
                            if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                                SetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                        }
                        else
                            Log.DebugFormat(GetType().Name, "Device", "Device State Changed", "{0} = {1}. Ignoring Setting CashSwiftDeviceState.SERVER_CONNECTION", "allowConnectionError", allowConnectionError);
                    }
                }
            }
        }

        public void InitialiseUsersAndPermissions() => LogoffUsers();

        private void InitialiseEmailManager()
        {
        }

        private void InitialiseFolders() => Directory.CreateDirectory(DeviceConfiguration.TRANSACTION_LOG_FOLDER);

        private void InitialiseApplicationModel() => ApplicationModel.InitialiseApplicationModel();

        private void SystemStartupChecks()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.Debug(GetType().Name, nameof(SystemStartupChecks), "Initialisation", "Performing startup checks");
                Log.Debug(GetType().Name, "SystemStartupDatabaseCheck", "Initialisation", "Performing database check");
                if (!ApplicationModel.TestConnection())
                {
                    Log.Fatal(GetType().Name, 88, ApplicationErrorConst.ERROR_DATABASE_OFFLINE.ToString(), "Could not connect to database during system startup, terminating...");
                    OnApplicationStartupFailedEvent(this, ApplicationErrorConst.ERROR_DATABASE_OFFLINE, "Could not connect to database during system startup, terminating...");
                }
                else if (ApplicationModel.GetDevice(DBContext) == null)
                {
                    Log.Fatal(GetType().Name, 88, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "This Device does not exist in the system, terminating...");
                    OnApplicationStartupFailedEvent(this, ApplicationErrorConst.ERROR_DEVICE_DOES_NOT_EXIST, "This Device does not exist in the system, terminating...");
                }
                InitialiseApplicationModel();
                Log.Info(GetType().Name, "SystemStartupChecks Result", "Initialisation", "SUCCESS");
            }
        }

        private void InitialisePrinter()
        {
            Log.Debug(GetType().Name, nameof(InitialisePrinter), "Initialisation", "Initialising Printer");
            if (CurrentApplicationState != ApplicationState.STARTUP)
                return;
            try
            {
                Printer = new DepositorPrinter(this, Log, DeviceConfiguration.RECEIPT_PRINTERPORT);
                Log.Info(GetType().Name, "InitialisePrinter Result", "Initialisation", "SUCCESS");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 75, ApplicationErrorConst.ERROR_PRINTER_ERROR.ToString(), "Could not connect to database during system startup: {0}>>{1}", ex.Message, ex?.InnerException?.Message);
                Printer = null;
            }
        }

        private void InitialiseDevice()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Device device = ApplicationModel.GetDevice(DBContext);
                if (device == null)
                {
                    Log.FatalFormat(GetType().Name, 97, ApplicationErrorConst.ERROR_DEVICE_DOES_NOT_EXIST.ToString(), "Device with machine name = {0} does not exists in the local database.", Environment.MachineName);
                    SetCashSwiftDeviceState(CashSwiftDeviceState.DATABASE);
                }
                DeviceConfiguration = DeviceConfiguration.Initialise();
                Log.Debug(GetType().Name, nameof(InitialiseDevice), "Initialisation", "Initialising Depositor Device");
                try
                {
                    if (CurrentApplicationState == ApplicationState.STARTUP)
                    {
                        if (string.IsNullOrWhiteSpace(device.mac_address))
                        {
                            device.mac_address = ExtentionMethods.GetDefaultMacAddress();
                            SaveToDatabase(DBContext);
                        }
                        if (DeviceConfiguration.CONTROLLER_TYPE == "CashAccSys")
                            DeviceManager = new CashAccSysDeviceManager.CashAccSysDeviceManager(DeviceConfiguration.DEVICECONTROLLER_HOST, DeviceConfiguration.DEVICECONTROLLER_PORT, device?.mac_address, 1234, DeviceConfiguration.FIX_DEVICE_PORT, DeviceConfiguration.FIX_CONTROLLER_PORT, DeviceConfiguration.BAGFULL_WARN_PERCENT, DeviceConfiguration.SENSOR_INVERT_DOOR, DeviceConfiguration.CONTROLLER_LOG_DIRECTORY);

                        //else if (ApplicationViewModel.DeviceConfiguration.CONTROLLER_TYPE == "Methodex")
                        //  this.DeviceManager = (IDeviceManager) new MethodexDeviceManager.MethodexDeviceManager(ApplicationViewModel.DeviceConfiguration.DEVICECONTROLLER_HOST, ApplicationViewModel.DeviceConfiguration.DEVICECONTROLLER_PORT, device?.mac_address, 1234, ApplicationViewModel.DeviceConfiguration.BAGFULL_WARN_PERCENT, ApplicationViewModel.DeviceConfiguration.SENSOR_INVERT_DOOR, ApplicationViewModel.DeviceConfiguration.CONTROLLER_LOG_DIRECTORY);

                        if (DeviceManager == null)
                            throw new Exception("Error creating DeviceManager: _deviceManager is null");
                        DeviceManager.ConnectionEvent -= new EventHandler<StringResult>(DeviceManager_ConnectionEvent);
                        DeviceManager.RaiseControllerStateChangedEvent -= new EventHandler<ControllerStateChangedEventArgs>(DeviceManager_RaiseControllerStateChangedEvent);
                        DeviceManager.RaiseDeviceStateChangedEvent -= new EventHandler<DeviceStateChangedEventArgs>(DeviceManager_RaiseDeviceStateChangedEvent);
                        DeviceManager.StatusReportEvent -= new EventHandler<DeviceStatusChangedEventArgs>(DeviceManager_StatusReportEvent);
                        DeviceManager.NotifyCurrentTransactionStatusChangedEvent -= new EventHandler<EventArgs>(DeviceManager_NotifyCurrentTransactionStatusChangedEvent);
                        DeviceManager.TransactionStartedEvent -= new EventHandler<DeviceTransaction>(DeviceManager_TransactionStartedEvent);
                        DeviceManager.CashInStartedEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_CashInStartedEvent);
                        DeviceManager.CountStartedEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_CountStartedEvent);
                        DeviceManager.CountPauseEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_CountPauseEvent);
                        DeviceManager.CountEndEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_CountEndEvent);
                        DeviceManager.TransactionStatusEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_TransactionStatusEvent);
                        DeviceManager.TransactionEndEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_TransactionEndEvent);
                        DeviceManager.CITResultEvent -= new EventHandler<CITResult>(DeviceManager_CITResultEvent);
                        DeviceManager.BagClosedEvent -= new EventHandler<EventArgs>(DeviceManager_BagClosedEvent);
                        DeviceManager.BagOpenedEvent -= new EventHandler<EventArgs>(DeviceManager_BagOpenedEvent);
                        DeviceManager.BagRemovedEvent -= new EventHandler<EventArgs>(DeviceManager_BagRemovedEvent);
                        DeviceManager.BagPresentEvent -= new EventHandler<EventArgs>(DeviceManager_BagPresentEvent);
                        DeviceManager.DoorClosedEvent -= new EventHandler<EventArgs>(DeviceManager_DoorClosedEvent);
                        DeviceManager.DoorOpenEvent -= new EventHandler<EventArgs>(DeviceManager_DoorOpenEvent);
                        DeviceManager.BagFullAlertEvent -= new EventHandler<ControllerStatus>(DeviceManager_BagFullAlertEvent);
                        DeviceManager.BagFullWarningEvent -= new EventHandler<ControllerStatus>(DeviceManager_BagFullWarningEvent);
                        DeviceManager.DeviceLockedEvent -= new EventHandler<EventArgs>(DeviceManager_DeviceLockedEvent);
                        DeviceManager.DeviceUnlockedEvent -= new EventHandler<EventArgs>(DeviceManager_DeviceUnlockedEvent);
                        DeviceManager.EscrowJamStartEvent -= new EventHandler<EventArgs>(DeviceManager_EscrowJamStartEvent);
                        DeviceManager.EscrowJamClearWaitEvent -= new EventHandler<EventArgs>(DeviceManager_EscrowJamClearWaitEvent);
                        DeviceManager.EscrowJamEndRequestEvent -= new EventHandler<EventArgs>(DeviceManager_EscrowJamEndRequestEvent);
                        DeviceManager.EscrowJamEndEvent -= new EventHandler<EventArgs>(DeviceManager_EscrowJamEndEvent);
                        DeviceManager.EscrowRejectEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_EscrowRejectEvent);
                        DeviceManager.EscrowDropEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_EscrowDropEvent);
                        DeviceManager.EscrowOperationCompleteEvent -= new EventHandler<DeviceTransactionResult>(DeviceManager_EscrowOperationCompleteEvent);
                        DeviceManager.NoteJamStartEvent -= new EventHandler<EventArgs>(DeviceManager_NoteJamStartEvent);
                        DeviceManager.NoteJamClearWaitEvent -= new EventHandler<EventArgs>(DeviceManager_NoteJamClearWaitEvent);
                        DeviceManager.NoteJamEndRequestEvent -= new EventHandler<EventArgs>(DeviceManager_NoteJamEndRequestEvent);
                        DeviceManager.NoteJamEndEvent -= new EventHandler<EventArgs>(DeviceManager_NoteJamEndEvent);
                        DeviceManager.ConnectionEvent += new EventHandler<StringResult>(DeviceManager_ConnectionEvent);
                        DeviceManager.RaiseControllerStateChangedEvent += new EventHandler<ControllerStateChangedEventArgs>(DeviceManager_RaiseControllerStateChangedEvent);
                        DeviceManager.RaiseDeviceStateChangedEvent += new EventHandler<DeviceStateChangedEventArgs>(DeviceManager_RaiseDeviceStateChangedEvent);
                        DeviceManager.StatusReportEvent += new EventHandler<DeviceStatusChangedEventArgs>(DeviceManager_StatusReportEvent);
                        DeviceManager.NotifyCurrentTransactionStatusChangedEvent += new EventHandler<EventArgs>(DeviceManager_NotifyCurrentTransactionStatusChangedEvent);
                        DeviceManager.TransactionStartedEvent += new EventHandler<DeviceTransaction>(DeviceManager_TransactionStartedEvent);
                        DeviceManager.CashInStartedEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_CashInStartedEvent);
                        DeviceManager.CountStartedEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_CountStartedEvent);
                        DeviceManager.CountPauseEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_CountPauseEvent);
                        DeviceManager.CountEndEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_CountEndEvent);
                        DeviceManager.TransactionStatusEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_TransactionStatusEvent);
                        DeviceManager.TransactionEndEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_TransactionEndEvent);
                        DeviceManager.CITResultEvent += new EventHandler<CITResult>(DeviceManager_CITResultEvent);
                        DeviceManager.BagClosedEvent += new EventHandler<EventArgs>(DeviceManager_BagClosedEvent);
                        DeviceManager.BagOpenedEvent += new EventHandler<EventArgs>(DeviceManager_BagOpenedEvent);
                        DeviceManager.BagRemovedEvent += new EventHandler<EventArgs>(DeviceManager_BagRemovedEvent);
                        DeviceManager.BagPresentEvent += new EventHandler<EventArgs>(DeviceManager_BagPresentEvent);
                        DeviceManager.DoorClosedEvent += new EventHandler<EventArgs>(DeviceManager_DoorClosedEvent);
                        DeviceManager.DoorOpenEvent += new EventHandler<EventArgs>(DeviceManager_DoorOpenEvent);
                        DeviceManager.BagFullAlertEvent += new EventHandler<ControllerStatus>(DeviceManager_BagFullAlertEvent);
                        DeviceManager.BagFullWarningEvent += new EventHandler<ControllerStatus>(DeviceManager_BagFullWarningEvent);
                        DeviceManager.DeviceLockedEvent += new EventHandler<EventArgs>(DeviceManager_DeviceLockedEvent);
                        DeviceManager.DeviceUnlockedEvent += new EventHandler<EventArgs>(DeviceManager_DeviceUnlockedEvent);
                        DeviceManager.EscrowJamStartEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamStartEvent);
                        DeviceManager.EscrowJamClearWaitEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamClearWaitEvent);
                        DeviceManager.EscrowJamEndRequestEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamEndRequestEvent);
                        DeviceManager.EscrowJamEndEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamEndEvent);
                        DeviceManager.EscrowRejectEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_EscrowRejectEvent);
                        DeviceManager.EscrowDropEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_EscrowDropEvent);
                        DeviceManager.EscrowOperationCompleteEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_EscrowOperationCompleteEvent);
                        DeviceManager.NoteJamStartEvent += new EventHandler<EventArgs>(DeviceManager_NoteJamStartEvent);
                        DeviceManager.NoteJamClearWaitEvent += new EventHandler<EventArgs>(DeviceManager_NoteJamClearWaitEvent);
                        DeviceManager.NoteJamEndRequestEvent += new EventHandler<EventArgs>(DeviceManager_NoteJamEndRequestEvent);
                        DeviceManager.NoteJamEndEvent += new EventHandler<EventArgs>(DeviceManager_NoteJamEndEvent);
                    }
                    DeviceManager.Initialise();
                    EscrowJam escrowJam = DBContext.EscrowJams.OrderByDescending(x => x.date_detected).FirstOrDefault();
                    if (escrowJam == null || escrowJam.recovery_date.HasValue || DeviceManager.DeviceManagerMode == DeviceManagerMode.ESCROW_JAM)
                        return;
                    DeviceManager_EscrowJamStartEvent(this, EventArgs.Empty);
                    Log.WarningFormat(nameof(ApplicationViewModel), nameof(InitialiseDevice), "Escrow Jam", "Escrow jam detected from database id={0}>>tx_id={1}>>acc_no={2}>>posted_amount = {3} {4:#,##0.00}", escrowJam.id, escrowJam.transaction_id, escrowJam.Transaction.tx_account_number, escrowJam.Transaction.tx_currency, escrowJam.posted_amount / 100.0);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Failed to initiaise depositor: {0}", ex.MessageString());
                    OnApplicationStartupFailedEvent(this, ApplicationErrorConst.ERROR_APPLICATION_STARTUP_FAILED, ex.MessageString());
                }
            }
        }

        private void DeviceManager_NoteJamStartEvent(object sender, EventArgs e)
        {
            Log?.Error(GetType().Name, ApplicationErrorConst.ERROR_DEVICE_NOTEJAM.ToString(), nameof(DeviceManager_NoteJamStartEvent), "Note Jam Detected", Array.Empty<object>());
            CurrentApplicationState = ApplicationState.DEVICE_ERROR;
            if (CurrentSession != null)
                CurrentSession.CountingEnded = true;
            if (CurrentTransaction != null)
            {
                AlertManager.SendAlert(new AlertNoteJam(CurrentTransaction, CurrentSession.Device, DateTime.Now));
                CurrentTransaction.NoteJamDetected = true;
            }
            ShowDialogBox(new NoteJamScreenViewModel(this));
        }

        private void DeviceManager_NoteJamClearWaitEvent(object sender, EventArgs e)
        {
        }

        private void DeviceManager_NoteJamEndRequestEvent(object sender, EventArgs e)
        {
        }

        private void DeviceManager_NoteJamEndEvent(object sender, EventArgs e)
        {
            if (CurrentTransaction != null)
            {
                AlertManager.SendAlert(new AlertNoteJamClearSuccess(CurrentTransaction.Transaction, CurrentSession.Device, DateTime.Now));
                Log?.Info(GetType().Name, "EventHandling", nameof(DeviceManager_NoteJamEndEvent), "Note Jam Cleared");
                EndTransaction(ApplicationErrorConst.ERROR_DEVICE_NOTEJAM, "Note Jam");
            }
            EndSession(false, 227, ApplicationErrorConst.ERROR_DEVICE_NOTEJAM, "Note Jam detected, terminating transaction");
        }

        private void DeviceManager_EscrowJamStartEvent(object sender, EventArgs e)
        {
            Log?.WarningFormat(nameof(ApplicationViewModel), nameof(DeviceManager_EscrowJamStartEvent), "escrow Jam", "Escrow Jam Detected sender = {0}", sender);
            if (CurrentSession != null)
                CurrentSession.CountingEnded = true;
            if (CurrentTransaction != null)
            {
                if (CurrentTransaction.Transaction != null)
                {
                    CurrentTransaction.Transaction.escrow_jam = true;
                    EscrowJam = new EscrowJam()
                    {
                        id = Guid.NewGuid(),
                        date_detected = DateTime.Now,
                        dropped_amount = CurrentTransaction.DroppedAmountCents,
                        escrow_amount = CurrentTransaction.CountedAmountCents
                    };
                    CurrentTransaction.Transaction.EscrowJams.Add(EscrowJam);
                    CurrentTransaction.DroppedAmountCents = CurrentTransaction.TotalAmountCents;
                    CurrentTransaction.DroppedDenomination += CurrentTransaction.CountedDenomination;
                    CurrentSession.SaveToDatabase();
                }
                AlertManager.SendAlert(new AlertEscrowJam(CurrentTransaction, CurrentSession.Device, DateTime.Now));

                CurrentTransaction.EscrowJamDetected = true;
                EndTransaction(ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM, ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM.ToString() + ": Escrow Jam detected. DO NOT Post until after CIT");
                SetCashSwiftDeviceState(CashSwiftDeviceState.ESCROW_JAM);
                DeviceManager.OnEscrowJamStartEvent(this, EventArgs.Empty);
            }
            if (sender == this && DeviceManager.DeviceManagerMode != DeviceManagerMode.ESCROW_JAM)
            {
                Log.Warning(nameof(ApplicationViewModel), nameof(DeviceManager_EscrowJamStartEvent), "Escrow Jam", "Updating Escrow Jam on Controller DeviceManager.OnEscrowJamStartEvent");
                SetCashSwiftDeviceState(CashSwiftDeviceState.ESCROW_JAM);
                DeviceManager.OnEscrowJamStartEvent(this, EventArgs.Empty);
            }
            else if (EscrowJam == null)
            {
                EscrowJam escrowJam = new DepositorDBContext().EscrowJams.OrderByDescending(x => x.date_detected).FirstOrDefault();
                if (escrowJam != null && !escrowJam.recovery_date.HasValue)
                    EscrowJam = escrowJam;
            }
            Log.WarningFormat(nameof(ApplicationViewModel), nameof(DeviceManager_EscrowJamStartEvent), "Showing OutofOrder screen", "Showing out of order screen");

            ShowErrorDialog(new OutOfOrderScreenViewModel(this));
        }

        private void DeviceManager_EscrowJamClearWaitEvent(object sender, EventArgs e)
        {
        }

        private void DeviceManager_EscrowJamEndRequestEvent(object sender, EventArgs e)
        {
        }

        private void DeviceManager_EscrowJamEndEvent(object sender, EventArgs e)
        {
            ResetDevice();
            InitialiseApp();
            CloseDialog(true);
        }


        internal void ClearEscrowJam() => DeviceManager.ClearEscrowJam();

        public bool CanClearNoteJam
        {
            get
            {
                IDeviceManager deviceManager = DeviceManager;
                return deviceManager != null && deviceManager.CanClearNoteJam;
            }
        }

        internal void ClearNoteJam() => DeviceManager.ClearNoteJam();

        internal void EndEscrowJam() => DeviceManager.EndEscrowJam();

        private void InitialiseScreenList()
        {
            GUIScreens = new List<GUIScreen>();
            GUIScreens.AddRange(ApplicationModel.dbGUIScreens);
        }

        private void InitialiseLanguageList() => ApplicationModel.GenerateLanguageList();

        private void InitialiseCurrencyList() => ApplicationModel.GenerateCurrencyList();

        private void InitialiseTransactionTypeList() => ApplicationModel.GenerateTransactionTypeList();

        private void HandleIncompleteTransaction()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.Debug(GetType().Name, nameof(HandleIncompleteTransaction), "Transaction", "Checking for and maintaining any incomplete transaction");
                DbSet<Transaction> transactions = DBContext.Transactions;
                Expression<Func<Transaction, bool>> predicate = x => x.tx_completed == false;
                foreach (Transaction transaction in transactions.Where(predicate).ToList())
                {
                    transaction.tx_completed = true;
                    transaction.tx_end_date = new DateTime?(DateTime.Now);
                    transaction.tx_error_code = 85;
                    transaction.tx_error_message = "Incomplete transaction aborted";
                    transaction.tx_result = 85;
                }
                SaveToDatabase(DBContext);
            }
        }

        public void HandleIncompleteSession()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                HandleIncompleteTransaction();
                ApplicationModel.GetDevice(DBContext);
                DbSet<DepositorSession> depositorSessions = DBContext.DepositorSessions;
                Expression<Func<DepositorSession, bool>> predicate = x => x.complete == false;
                foreach (DepositorSession depositorSession in depositorSessions.Where(predicate).ToList())
                {
                    depositorSession.session_end = new DateTime?(DateTime.Now);
                    depositorSession.complete = true;
                    depositorSession.complete_success = false;
                    depositorSession.error_code = new int?(84);
                    depositorSession.error_message = "Session is incomplete";
                }
                SaveToDatabase(DBContext);
            }
        }

        private void StartSession()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                _deviceManager.SetCurrency(ApplicationModel.GetDevice(DBContext).CurrencyList.default_currency.ToUpper());
                CurrentSession = new AppSession(this);
                CurrentSession.TransactionLimitReachedEvent += new EventHandler<EventArgs>(CurrentSession_TransactionLimitReachedEvent);
                CurrentScreenIndex = 1;
                ShowScreen();
                CurrentApplicationState = ApplicationState.IDLE;
            }
        }

        public void EndSession()
        {
            if (CurrentSession == null)
                return;
            EndSession(true, 0, ApplicationErrorConst.ERROR_NONE);
        }

        public void EndSession(
          bool success,
          int errorcode,
          ApplicationErrorConst transactionError,
          string errormessage = "")
        {
            Log.InfoFormat(GetType().Name, nameof(EndSession), "Session", "Result = {0}", transactionError.ToString());
            if (CurrentTransaction != null)
                EndTransaction(transactionError, errormessage);
            CurrentSession?.EndSession(success, errorcode, transactionError, errormessage);
            CurrentSession =  null;
            InitialiseApp();
        }

        public int CurrentScreenIndex { get; set; }

        public List<GUIScreen> GUIScreens { get; set; }

        public GUIScreen CurrentGUIScreen => GUIScreens.ElementAtOrDefault(CurrentScreenIndex) ??  null;

        public object CurrentScreen { get; set; }

        public Guid? SessionID => CurrentSession?.SessionID;

        public void ShowScreen(int screenIndex, bool generateNewScreen = true)
        {
            try
            {
                using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                {
                    if (screenIndex == 0)
                        InitialiseScreenList();
                    if (ApplicationStatus.CashSwiftDeviceState == CashSwiftDeviceState.NONE || DebugNoDevice)
                    {
                        if (generateNewScreen)
                        {
                            CurrentScreenIndex = screenIndex < 0 ? 0 : screenIndex;
                            CurrentScreenIndex = screenIndex > GUIScreens.Count - 1 ? 0 : screenIndex;
                            depositorDbContext.GUIScreens.Attach(GUIScreens[CurrentScreenIndex]);
                            TypeInfo typeInfo = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.GUID == GUIScreens[CurrentScreenIndex].GUIScreenType.code);
                            GUIScreen guiScreen = GUIScreens[CurrentScreenIndex];
                            bool? required = depositorDbContext.GuiScreenListScreens.Where((x => x.GUIScreen.id == guiScreen.id)).FirstOrDefault()?.required;
                            var GUIScreenText = depositorDbContext.GUIScreens.Include(g => g.GUIScreenText).Where((x => x.id == guiScreen.id)).Select(d => d.GUIScreenText).FirstOrDefault();
                            string str = CashSwiftTranslationService.TranslateUserText("ShowScreen().screenTitle", GUIScreenText?.screen_title, "[Translation Error]");
                            object instance = Activator.CreateInstance(typeInfo, str, this, required);
                            ActivateItemAsync(instance);
                            Log.InfoFormat(GetType().Name, nameof(ShowScreen), "Navigation", "Showing screen: {0}", GUIScreens[CurrentScreenIndex]?.name);
                            if (CurrentScreen is DepositorCustomerScreenBaseViewModel)
                            {
                                if (CurrentScreen is DepositorCustomerScreenBaseViewModel currentScreen)
                                    currentScreen.Dispose();
                                CurrentScreen =  null;
                            }
                            CurrentScreen = instance;
                        }
                        else
                            ActivateItemAsync(CurrentScreen);
                    }
                    else
                    {
                        Log.WarningFormat(nameof(ApplicationViewModel), nameof(ShowScreen), "Showing OutofOrder screen", "{0} == CashSwiftDeviceState.NONE || debugNoDevice == {1}", ApplicationStatus.CashSwiftDeviceState, DebugNoDevice);
                        ShowErrorDialog(new OutOfOrderScreenViewModel(this));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(nameof(ShowScreen), 104, ApplicationErrorConst.ERROR_SCREEN_RENDER_FAILED.ToString(), "Error displaying screen {0}: {1}>>{2}>>{3}", screenIndex.ToString() ?? "", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                throw;
            }
        }

        public void ShowScreen() => ShowScreen(CurrentScreenIndex);

        public void ShowScreen(bool genScreen) => ShowScreen(CurrentScreenIndex, genScreen);

        public void NavigateNextScreen()
        {
            if (CurrentScreenIndex + 1 >= GUIScreens.Count)
                return;
            ShowScreen(++CurrentScreenIndex);
        }

        public void NavigatePreviousScreen()
        {
            if (CurrentScreenIndex - 1 < 0)
                return;
            ShowScreen(--CurrentScreenIndex);
        }

        public void NavigateFirstScreen()
        {
            CurrentScreenIndex = 0;
            ShowScreen(CurrentScreenIndex);
        }

        public void NavigateLastScreen()
        {
            CurrentScreenIndex = GUIScreens.Count - 1;
            ShowScreen(CurrentScreenIndex);
        }

        public void ShowDialog(object screen)
        {
            if (!AdminMode && ApplicationStatus.CashSwiftDeviceState != CashSwiftDeviceState.NONE)
                return;
            ActivateItemAsync(screen);
        }

        public void ShowDialogBox(object screen) => ActivateItemAsync(screen);

        public void ShowErrorDialog(object screen)
        {
            Log.Info(GetType().Name, "ShowDialogScreen", "Screen", screen.GetType().Name);
            if (ApplicationStatus.CashSwiftDeviceState == CashSwiftDeviceState.NONE)
                return;
            ActivateItemAsync(screen);
        }

        public void CloseDialog(bool generateScreen = true)
        {
            if (generateScreen)
            {
                Log.Info(GetType().Name, nameof(CloseDialog), "Screen", "Closing dialog screen");
                ShowScreen();
            }
            else
                ShowScreen(false);
        }

        public bool? CanCancelTransaction => throw new NotImplementedException();

        internal void CancelSessionOnUserInput()
        {
            Log.Info(GetType().Name, "cancelSession", "Session", "User has cancelled the session");
            string message = CashSwiftTranslationService?.TranslateSystemText("CancelSessionOnUserInput.message", "sys_Dialog_CancelTransaction_MessageText", "Would you like to cancel your current transaction?" + Environment.NewLine + Environment.NewLine + "To delete text, please use the \"Delete\" or \"Backspace\" buttons");
            string str = CashSwiftTranslationService?.TranslateSystemText("CancelSessionOnUserInput.message", "sys_Dialog_CancelTransaction_TitleCaption", "Cancel Transaction");
            DeviceConfiguration deviceConfiguration = DeviceConfiguration;
            int timeout = deviceConfiguration != null ? deviceConfiguration.USER_SCREEN_TIMEOUT : 15;
            string title = str;
            if (TimeoutDialogBox.ShowDialog(message, timeout, title, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                AlertManager.SendAlert(new AlertTransactionCancelled(Log, CurrentTransaction, CurrentSession.Device, DateTime.Now));
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 100, ApplicationErrorConst.ERROR_ALERT_SEND_FAILED.ToString(), "{0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
            }
            ResetDevice();
            EndSession(false, 1011, ApplicationErrorConst.WARN_DEPOSIT_CANCELED, "User has cancelled the session");
        }

        public void SplashScreen_Clicked()
        {
            Log.Info(GetType().Name, "splashScreen_Clicked", "Screen", "The splash screen has been clicked");
            HandleIncompleteSession();
            StartSession();
        }

        public void TermsAccepted(bool accepted = true)
        {
            CurrentSession.TermsAccepted = accepted;
            if (accepted)
            {
                Log.Info(GetType().Name, "termsAccepted", "User Input", "Terms Accepted");
                CurrentSession.TermsAccepted = true;
                NavigateNextScreen();
            }
            else
            {
                Log.Info(GetType().Name, "termsAccepted", "User Input", "Terms Rejected");
                EndSession(false, 20, ApplicationErrorConst.WARN_TERMS_REJECT_BY_USER, "Customer Rejected Terms and Conditions");
            }
        }

        public void SetCurrency(Currency value)
        {
            Log.Info(GetType().Name, nameof(SetCurrency), "User Input", value.code.ToUpper());
            CurrentTransaction.Currency = value;
            if (DebugNoDevice)
                return;
            DeviceManager.SetCurrency(CurrentTransaction.Currency.code.ToUpper());
            DeviceManager.SetCurrency(CurrentTransaction.Currency.code.ToUpper());
            DeviceManager.SetCurrency(CurrentTransaction.Currency.code.ToUpper());
            DeviceManager.SetCurrency(CurrentTransaction.Currency.code.ToUpper());
            DeviceManager.SetCurrency(CurrentTransaction.Currency.code.ToUpper());
        }

        internal void CreateTransaction(TransactionTypeListItem value)
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                depositorDbContext.TransactionTypeListItems.Attach(value);
                Log.Info(GetType().Name, "SetTransactionType", "User Input", value.name);
                GUIScreens.AddRange(ApplicationModel.GetTransactionTypeScreenList(value).ToList());
                if (CurrentSession.Transaction == null)
                    CurrentSession.CreateTransaction(value);
                CurrentTransaction.TransactionType = value;
            }
        }

        internal async Task<AccountsListResponse> SearchAccountListAsync(
          string searchText,
          TransactionTypeListItem txType,
          string currency,
          int PageNumber = 0,
          int PageSize = 1000)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountsListResponse accountsListResponse1;
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.Info(applicationViewModel.GetType().Name, "GetAccountList", "User Input", txType.name);
                AccountsListResponse response = new AccountsListResponse();
                Guid guid = Guid.NewGuid();
                string str1 = guid.ToString();
                if (applicationViewModel.DebugNoCoreBanking)
                {
                    AccountsListResponse accountsListResponse2 = new AccountsListResponse
                    {
                        RequestID = str1,
                        MessageDateTime = DateTime.Now,
                        ServerErrorCode = "0",
                        IsSuccess = true
                    };
                    guid = Guid.NewGuid();
                    accountsListResponse2.MessageID = guid.ToString();
                    accountsListResponse2.Accounts = new List<Account>()
          {
            new Account()
            {
              account_number = "1234567890",
              account_name = "Account1"
            },
            new Account()
            {
              account_number = "1234567891",
              account_name = "Account2"
            },
            new Account()
            {
              account_number = "1234567892",
              account_name = "Account3"
            },
            new Account()
            {
              account_number = "1234567893",
              account_name = "Account4"
            },
            new Account()
            {
              account_number = "1234567894",
              account_name = "Account5"
            },
            new Account()
            {
              account_number = "1234567895",
              account_name = "Account6"
            },
            new Account()
            {
              account_number = "1234567896",
              account_name = "Account7"
            },
            new Account()
            {
              account_number = "1234567897",
              account_name = "Account8"
            },
            new Account()
            {
              account_number = "1234567898",
              account_name = "Account9"
            },
            new Account()
            {
              account_number = "1234567899",
              account_name = "Account10"
            },
            new Account()
            {
              account_number = "1122334455",
              account_name = "Account11"
            },
            new Account()
            {
              account_number = "1112334567",
              account_name = "Account12"
            }
          };
                    response = accountsListResponse2;
                }
                else
                {
                    try
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Request", "TxType = {0} Currency = {1}", txType.name, currency);
                        Device device = GetDevice(DBContext);
                        DateTime now = DateTime.Now;
                        GUIControlServiceClient controlServiceClient = new GUIControlServiceClient(DeviceConfiguration.API_CDM_GUI_URI, device.app_id, device.app_key, null);
                        AccountsListRequest accountsListRequest = new AccountsListRequest
                        {
                            Currency = applicationViewModel.CurrentTransaction?.CurrencyCode,
                            Language = applicationViewModel.CurrentLanguage
                        };
                        AppSession currentSession = applicationViewModel.CurrentSession;
                        string str2;
                        if (currentSession == null)
                        {
                            str2 =  null;
                        }
                        else
                        {
                            guid = currentSession.SessionID;
                            str2 = guid.ToString();
                        }
                        accountsListRequest.SessionID = str2;
                        guid = Guid.NewGuid();
                        accountsListRequest.MessageID = guid.ToString();
                        accountsListRequest.AppName = device.machine_name;
                        accountsListRequest.MessageDateTime = DateTime.Now;
                        accountsListRequest.TransactionType = applicationViewModel.CurrentTransaction.TransactionType.id;
                        accountsListRequest.AppID = device.app_id;
                        accountsListRequest.DeviceID = device.id;
                        accountsListRequest.PageNumber = PageNumber;
                        accountsListRequest.PageSize = PageSize;
                        accountsListRequest.SearchText = searchText;
                        AccountsListRequest request = accountsListRequest;
                        response = await controlServiceClient.SearchAccountAsync(request);
                        applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                        if (response.IsSuccess)
                        {
                            Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Response", "{0}", response.ToString());
                            if (!DeviceConfiguration.ALLOW_CROSS_CURRENCY_TX)
                                response.Accounts.RemoveAll(p => !p.currency.Equals(currency));
                        }
                        if (applicationViewModel.ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                            applicationViewModel.UnSetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Account Validation Exception: {0}", ex.MessageString());
                        if (!applicationViewModel.ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                            applicationViewModel.SetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                        response.IsSuccess = false;
                    }
                }
                accountsListResponse1 = response;
            }
            return accountsListResponse1;
        }

        internal async Task<AccountsListResponse> GetAccountListAsync(
          TransactionTypeListItem txType,
          string currency,
          int PageNumber = 0,
          int PageSize = 1000)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountsListResponse accountListAsync;
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.Info(applicationViewModel.GetType().Name, "GetAccountList", "User Input", txType.name);
                AccountsListResponse response = new AccountsListResponse();
                Guid guid = Guid.NewGuid();
                string str1 = guid.ToString();
                if (applicationViewModel.DebugNoCoreBanking)
                {
                    AccountsListResponse accountsListResponse = new AccountsListResponse
                    {
                        RequestID = str1,
                        MessageDateTime = DateTime.Now,
                        ServerErrorCode = "0",
                        IsSuccess = true
                    };
                    guid = Guid.NewGuid();
                    accountsListResponse.MessageID = guid.ToString();
                    accountsListResponse.Accounts = new List<Account>()
          {
            new Account()
            {
              account_number = "1234567890",
              account_name = "Account1"
            },
            new Account()
            {
              account_number = "1234567891",
              account_name = "Account2"
            },
            new Account()
            {
              account_number = "1234567892",
              account_name = "Account3"
            },
            new Account()
            {
              account_number = "1234567893",
              account_name = "Account4"
            },
            new Account()
            {
              account_number = "1234567894",
              account_name = "Account5"
            },
            new Account()
            {
              account_number = "1234567895",
              account_name = "Account6"
            },
            new Account()
            {
              account_number = "1234567896",
              account_name = "Account7"
            },
            new Account()
            {
              account_number = "1234567897",
              account_name = "Account8"
            },
            new Account()
            {
              account_number = "1234567898",
              account_name = "Account9"
            },
            new Account()
            {
              account_number = "1234567899",
              account_name = "Account10"
            },
            new Account()
            {
              account_number = "1122334455",
              account_name = "Account11"
            },
            new Account()
            {
              account_number = "1112334567",
              account_name = "Account12"
            }
          };
                    response = accountsListResponse;
                }
                else
                {
                    try
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Request", "TxType = {0} Currency = {1}", txType.name, currency);
                        Device device = GetDevice(DBContext);
                        DateTime now = DateTime.Now;
                        GUIControlServiceClient controlServiceClient = new GUIControlServiceClient(DeviceConfiguration.API_CDM_GUI_URI, device.app_id, device.app_key, null);
                        AccountsListRequest accountsListRequest = new AccountsListRequest
                        {
                            Currency = applicationViewModel.CurrentTransaction?.CurrencyCode,
                            Language = applicationViewModel.CurrentLanguage
                        };
                        AppSession currentSession = applicationViewModel.CurrentSession;
                        string str2;
                        if (currentSession == null)
                        {
                            str2 =  null;
                        }
                        else
                        {
                            guid = currentSession.SessionID;
                            str2 = guid.ToString();
                        }
                        accountsListRequest.SessionID = str2;
                        guid = Guid.NewGuid();
                        accountsListRequest.MessageID = guid.ToString();
                        accountsListRequest.AppName = device.machine_name;
                        accountsListRequest.MessageDateTime = DateTime.Now;
                        accountsListRequest.TransactionType = applicationViewModel.CurrentTransaction.TransactionType.id;
                        accountsListRequest.AppID = device.app_id;
                        accountsListRequest.DeviceID = device.id;
                        accountsListRequest.PageNumber = PageNumber;
                        accountsListRequest.PageSize = PageSize;
                        AccountsListRequest request = accountsListRequest;
                        response = await controlServiceClient.GetAccountsListAsync(request);
                        applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                        if (response.IsSuccess)
                        {
                            Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Response", "{0}", response.ToString());
                            if (!DeviceConfiguration.ALLOW_CROSS_CURRENCY_TX)
                                response.Accounts.RemoveAll(p => !p.currency.Equals(currency));
                        }
                        if (applicationViewModel.ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                            applicationViewModel.UnSetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Account Validation Exception: {0}", ex.MessageString());
                        if (!applicationViewModel.ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                            applicationViewModel.SetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                        response.IsSuccess = false;
                    }
                }
                accountListAsync = response;
            }
            return accountListAsync;
        }

        internal async Task<AccountNumberValidationResponse> ValidateAccountNumberAsync(
          string accountNumber,
          string currency,
          int txType)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountNumberValidationResponse validationResponse1;
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.Info(applicationViewModel.GetType().Name, "ValidateAccountNumber", "User Input", accountNumber);
                AccountNumberValidationResponse response = new AccountNumberValidationResponse();
                if (applicationViewModel.DebugNoCoreBanking)
                {
                    if (accountNumber == "1234")
                    {
                        AccountNumberValidationResponse validationResponse2 = new AccountNumberValidationResponse
                        {
                            AccountName = "",
                            CanTransact = false,
                            MessageDateTime = DateTime.Now
                        };
                        Guid guid = Guid.NewGuid();
                        validationResponse2.RequestID = guid.ToString().ToUpper();
                        guid = Guid.NewGuid();
                        validationResponse2.MessageID = guid.ToString().ToUpper();
                        validationResponse2.IsSuccess = false;
                        validationResponse2.PublicErrorCode = 400.ToString() ?? "";
                        validationResponse2.PublicErrorMessage = "Account Does Not Exist";
                        response = validationResponse2;
                    }
                    else
                    {
                        AccountNumberValidationResponse validationResponse3 = new AccountNumberValidationResponse
                        {
                            AccountName = "Test Account",
                            CanTransact = true,
                            MessageDateTime = DateTime.Now
                        };
                        Guid guid = Guid.NewGuid();
                        validationResponse3.RequestID = guid.ToString().ToUpper();
                        guid = Guid.NewGuid();
                        validationResponse3.MessageID = guid.ToString().ToUpper();
                        validationResponse3.IsSuccess = true;
                        validationResponse3.PublicErrorCode = 200.ToString() ?? "";
                        validationResponse3.PublicErrorMessage = "Validated Successfully";
                        response = validationResponse3;
                    }
                }
                else
                {
                    try
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Request", "Account = {0} Currency = {1}", accountNumber, currency);
                        Device device = GetDevice(DBContext);
                        IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.app_id, device.app_key, null);
                        var request = new CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations.AccountNumberValidationRequest
                        {
                            AccountNumber = accountNumber,
                            AppID = device.app_id,
                            AppName = device.machine_name,
                            MessageID = Guid.NewGuid().ToString(),
                            MessageDateTime = DateTime.Now,
                            SessionID = applicationViewModel.SessionID.Value.ToString(),
                            DeviceID = device.id,
                            Currency = currency,
                            Language = applicationViewModel.CurrentLanguage,
                            TransactionType = txType
                        };

                        response = await integrationServiceClient.ValidateAccountNumberAsync(request);
                        applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                        if (response.IsSuccess)
                        {
                            Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Response", "AccountName = {0} ErrorCode = {1}, ErrorMessage = {2}", response.AccountName, response.ServerErrorCode, response.ServerErrorMessage);
                            if (!DeviceConfiguration.ALLOW_CROSS_CURRENCY_TX)
                            {
                                switch (response?.AccountCurrency)
                                {
                                    case null:
                                        break;
                                    default:
                                        if (response?.AccountCurrency?.ToUpper() != currency.ToUpper())
                                        {
                                            Log.InfoFormat(applicationViewModel.GetType().Name, "Transaction", "Cross Currency Not Allowed", "Cannot deposit {2} into {1} Account {0}.", accountNumber, response.AccountCurrency, currency);
                                            response.IsSuccess = false;
                                            response.PublicErrorMessage = string.Format("account indicated is a {0} account kindly enter the correct currency account", response.AccountCurrency);
                                            break;
                                        }
                                        break;
                                }
                            }
                        }
                        if (applicationViewModel.ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                            applicationViewModel.UnSetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Account Validation Exception: {0}", ex.MessageString());
                        if (!applicationViewModel.ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SERVER_CONNECTION))
                            applicationViewModel.SetCashSwiftDeviceState(CashSwiftDeviceState.SERVER_CONNECTION);
                        response.IsSuccess = false;
                        response.PublicErrorMessage = "Validation Error Occurred, please contact an administrator.";
                    }
                }
                Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Result", "Result = {0} AccountName = {1} CanTransact={2} Error = {3}", response.IsSuccess, response.AccountName, response.CanTransact, response?.ServerErrorMessage);
                validationResponse1 = response;
            }
            return validationResponse1;
        }

        internal async Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync(
          string accountNumber,
          string refAccountNumber,
          string transactionType)
        {
            ApplicationViewModel applicationViewModel = this;
            ReferenceAccountNumberValidationResponse validationResponse1;
            using (new DepositorDBContext())
            {
                ReferenceAccountNumberValidationResponse response = new ReferenceAccountNumberValidationResponse();
                if (applicationViewModel.DebugNoCoreBanking)
                {
                    if (refAccountNumber == "1234")
                    {
                        ReferenceAccountNumberValidationResponse validationResponse2 = new ReferenceAccountNumberValidationResponse
                        {
                            AccountName = "",
                            CanTransact = false,
                            MessageDateTime = DateTime.Now,
                            RequestID = Guid.NewGuid().ToString().ToUpper(),
                            MessageID = Guid.NewGuid().ToString().ToUpper(),
                            IsSuccess = false,
                            PublicErrorCode = 400.ToString() ?? "",
                            PublicErrorMessage = "Account Does Not Exist"
                        };
                        response = validationResponse2;
                    }
                    else
                    {
                        ReferenceAccountNumberValidationResponse validationResponse3 = new ReferenceAccountNumberValidationResponse
                        {
                            AccountName = "Test Account",
                            CanTransact = true,
                            MessageDateTime = DateTime.Now,
                            RequestID = Guid.NewGuid().ToString().ToUpper(),
                            MessageID = Guid.NewGuid().ToString().ToUpper(),
                            IsSuccess = true,
                            PublicErrorCode = 200.ToString() ?? "",
                            PublicErrorMessage = "Validated Successfully"
                        };
                        response = validationResponse3;
                    }
                }
                else
                {
                    try
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateReferenceAccountNumber", "Validation Request", "Account = {0} Type = {1}", refAccountNumber, applicationViewModel.CurrentTransaction.TransactionType.cb_tx_type);
                        Device device = applicationViewModel.CurrentSession.Device;
                        IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.app_id, device.app_key, null);
                        ReferenceAccountNumberValidationRequest request = new ReferenceAccountNumberValidationRequest
                        {
                            AccountNumber = accountNumber,
                            ReferenceAccountNumber = refAccountNumber,
                            AppID = device.app_id,
                            AppName = device.machine_name,
                            DeviceID = device.id,
                            MessageDateTime = DateTime.Now,
                            MessageID = Guid.NewGuid().ToString(),
                            SessionID = applicationViewModel.SessionID.Value.ToString(),
                            Currency = applicationViewModel.CurrentTransaction.CurrencyCode,
                            Language = applicationViewModel.CurrentLanguage,
                            TransactionType = applicationViewModel.CurrentTransaction.TransactionType.id
                        };
                        // ISSUE: explicit non-virtual call
                        response = await (integrationServiceClient.ValidateReferenceAccountNumberAsync(request));
                        applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Ref Account Validation Exception: {0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
                        response.IsSuccess = false;
                        response.PublicErrorMessage = "Validation Error Occurred, please contact an administrator.";
                    }
                }
                Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateReferenceAccountNumber", "ReferenceValidation Result", "Result = {0} AccountName = {1} CanTransact={2}", response.IsSuccess, response.AccountName, response.CanTransact);
                validationResponse1 = response;
            }
            return validationResponse1;
        }

        internal void ReferencesAccepted(bool success = true) => Log.Info(GetType().Name, "VerifyReferences", nameof(ReferencesAccepted), "User accepted the references");

        private void DeviceManager_DeviceLockedEvent(object sender, EventArgs e)
        {
            SetCashSwiftDeviceState(CashSwiftDeviceState.DEVICE_LOCK);
            Log.Debug(GetType().Name, "Device", "Device State Changed", "Setting CashSwiftDeviceState.DEVICE_LOCK");
        }

        private void DeviceManager_DeviceUnlockedEvent(object sender, EventArgs e)
        {
            if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.DEVICE_LOCK))
                return;
            UnSetCashSwiftDeviceState(CashSwiftDeviceState.DEVICE_LOCK);
        }

        private void DeviceManager_StatusReportEvent(object sender, DeviceStatusChangedEventArgs e)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Device device = ApplicationModel.GetDevice(DBContext);
                DeviceTransactionStatus transactionStatus;
                if (device != null)
                {
                    DeviceStatus DeviceStatus = DBContext.DeviceStatus.FirstOrDefault(x => x.device_id == device.id);
                    if (DeviceStatus == null)
                    {
                        DBContext.DeviceStatus.Add(new DeviceStatus()
                        {
                            device_id = device.id,
                            machine_name = Environment.MachineName.ToUpperInvariant(),
                            transaction_status = e.ControllerStatus.Transaction?.Status.ToString(),
                            transaction_type = e.ControllerStatus.Transaction?.Type.ToString(),
                            bag_note_capacity = e.ControllerStatus.Bag.NoteCapacity.ToString() ?? "",
                            bag_note_level = e.ControllerStatus.Bag.NoteLevel,
                            bag_number = e.ControllerStatus.Bag.BagNumber,
                            bag_percent_full = e.ControllerStatus.Bag.PercentFull,
                            bag_status = e.ControllerStatus.Bag.BagState.ToString() ?? "",
                            bag_value_capacity = new long?(e.ControllerStatus.Bag.ValueCapacity),
                            bag_value_level = new long?(e.ControllerStatus.Bag.ValueLevel),
                            ba_currency = e.ControllerStatus.NoteAcceptor.Currency,
                            ba_status = e.ControllerStatus.NoteAcceptor.Status.ToString(),
                            ba_type = e.ControllerStatus.NoteAcceptor.Type.ToString(),
                            controller_state = e.ControllerStatus.ControllerState.ToString(),
                            current_status = (int)ApplicationStatus.CashSwiftDeviceState,
                            escrow_position = e.ControllerStatus.Escrow.Position.ToString(),
                            escrow_status = e.ControllerStatus.Escrow.Status.ToString(),
                            escrow_type = e.ControllerStatus.Escrow.Type.ToString(),
                            id = Guid.NewGuid(),
                            modified = new DateTime?(DateTime.Now),
                            sensors_status = e.ControllerStatus.Sensor.Status.ToString(),
                            sensors_type = e.ControllerStatus.Sensor.Type.ToString(),
                            sensors_value = e.ControllerStatus.Sensor.Value,
                            sensors_bag = e.ControllerStatus.Sensor.Bag.ToString(),
                            sensors_door = e.ControllerStatus.Sensor.Door.ToString()
                        });
                    }
                    else
                    {
                        DeviceStatus.device_id = device.id;
                        DeviceStatus.machine_name = Environment.MachineName.ToUpperInvariant();
                        DeviceStatus.transaction_status = e.ControllerStatus.Transaction?.Status.ToString();
                        DeviceStatus.transaction_type = e.ControllerStatus.Transaction?.Type.ToString();
                        DeviceStatus.bag_note_capacity = e.ControllerStatus.Bag.NoteCapacity.ToString() ?? "";
                        DeviceStatus.bag_note_level = e.ControllerStatus.Bag.NoteLevel;
                        DeviceStatus.bag_number = e.ControllerStatus.Bag.BagNumber;
                        DeviceStatus.bag_percent_full = e.ControllerStatus.Bag.PercentFull;
                        DeviceStatus.bag_status = e.ControllerStatus.Bag.BagState.ToString() ?? "";
                        DeviceStatus.bag_value_capacity = new long?(e.ControllerStatus.Bag.ValueCapacity);
                        DeviceStatus.bag_value_level = new long?(e.ControllerStatus.Bag.ValueLevel);
                        DeviceStatus.ba_currency = e.ControllerStatus.NoteAcceptor.Currency;
                        DeviceStatus.ba_status = e.ControllerStatus.NoteAcceptor.Status.ToString();
                        DeviceStatus.ba_type = e.ControllerStatus.NoteAcceptor.Type.ToString();
                        DeviceStatus.controller_state = e.ControllerStatus.ControllerState.ToString();
                        DeviceStatus.current_status = (int)ApplicationStatus.CashSwiftDeviceState;
                        DeviceStatus.escrow_position = e.ControllerStatus.Escrow.Position.ToString();
                        DeviceStatus.escrow_status = e.ControllerStatus.Escrow.Status.ToString();
                        DeviceStatus.escrow_type = e.ControllerStatus.Escrow.Type.ToString();
                        DeviceStatus.modified = new DateTime?(DateTime.Now);
                        DeviceStatus.sensors_status = e.ControllerStatus.Sensor.Status.ToString();
                        DeviceStatus.sensors_type = e.ControllerStatus.Sensor.Type.ToString();
                        DeviceStatus.sensors_value = e.ControllerStatus.Sensor.Value;
                        DeviceStatus.sensors_bag = e.ControllerStatus.Sensor.Bag.ToString();
                        DeviceStatus.sensors_door = e.ControllerStatus.Sensor.Door.ToString();
                    }
                }
                SaveToDatabase(DBContext);
                if (e.ControllerStatus.Sensor.Door == DeviceSensorDoor.OPEN)
                {
                    if (DebugDisableSafeSensor)
                        UnSetCashSwiftDeviceState(CashSwiftDeviceState.SAFE);
                    else if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SAFE))
                        SetCashSwiftDeviceState(CashSwiftDeviceState.SAFE);
                }
                else if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.SAFE))
                    UnSetCashSwiftDeviceState(CashSwiftDeviceState.SAFE);
                if ((e.ControllerStatus.Bag.BagState == BagState.OK || e.ControllerStatus.Bag.BagState == BagState.CAPACITY) && e.ControllerStatus.Sensor.Bag != DeviceSensorBag.REMOVED)
                {
                    if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.BAG))
                        UnSetCashSwiftDeviceState(CashSwiftDeviceState.BAG);
                }
                else
                {
                    if (DebugDisableBagSensor)
                    {
                        UnSetCashSwiftDeviceState(CashSwiftDeviceState.BAG);
                    }
                    else
                    {
                        AppSession currentSession = CurrentSession;
                        bool flag = currentSession == null || !currentSession.CountingStarted;
                        if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.BAG) & flag)
                            SetCashSwiftDeviceState(CashSwiftDeviceState.BAG);
                    }
                    if ((CurrentApplicationState == ApplicationState.SPLASH || CurrentApplicationState == ApplicationState.STARTUP) && e.ControllerStatus.Bag.BagState == BagState.CLOSED)
                    {
                        Log.ErrorFormat(GetType().Name, 95, ApplicationErrorConst.ERROR_INCOMPLETE_CIT.ToString(), "Bag is closed on during {0}, finalising incomplete CIT", CurrentApplicationState.ToString());
                        CurrentApplicationState = ApplicationState.CIT_BAG_CLOSED;
                    }
                }
                if (e == null || e.DeviceManagerState != DeviceManagerState.OUT_OF_ORDER)
                {
                    if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.DEVICE_MANAGER))
                        UnSetCashSwiftDeviceState(CashSwiftDeviceState.DEVICE_MANAGER);
                }
                else if (DeviceManager.DeviceManagerMode == DeviceManagerMode.NONE && !ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.DEVICE_MANAGER))
                    SetCashSwiftDeviceState(CashSwiftDeviceState.DEVICE_MANAGER);
                if (CurrentApplicationState == ApplicationState.SPLASH || CurrentApplicationState == ApplicationState.STARTUP || CurrentApplicationState == ApplicationState.CIT_END)
                {
                    int num1;
                    if (e == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        ControllerState? controllerState1 = e.ControllerStatus?.ControllerState;
                        ControllerState controllerState2 = ControllerState.IDLE;
                        num1 = controllerState1.GetValueOrDefault() == controllerState2 & controllerState1.HasValue ? 1 : 0;
                    }
                    if (num1 != 0)
                    {
                        int num2;
                        if (e == null)
                        {
                            num2 = 0;
                        }
                        else
                        {
                            DeviceTransactionStatus? status = e.ControllerStatus?.Transaction?.Status;
                            transactionStatus = DeviceTransactionStatus.NONE;
                            num2 = status.GetValueOrDefault() == transactionStatus & status.HasValue ? 1 : 0;
                        }
                        if (num2 != 0)
                        {
                            int num3;
                            if (CashAccSysDeviceManager != null)
                            {
                                CashAccSysDeviceManager.CashAccSysDeviceManager sysDeviceManager = CashAccSysDeviceManager;
                                num3 = sysDeviceManager != null ? (sysDeviceManager.CashAccSysSerialFix.DE50Mode != 0 ? 1 : 0) : 1;
                            }
                            else
                                num3 = 0;
                            if (num3 != 0)
                            {
                                if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.CONTROLLER))
                                {
                                    SetCashSwiftDeviceState(CashSwiftDeviceState.CONTROLLER);
                                    Log.Debug(GetType().Name, "Device", "Device State Changed", "Setting CashSwiftDeviceState.CONTROLLER on DE50 in deposit");
                                    ResetDevice();
                                }
                            }
                            else if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.CONTROLLER))
                                UnSetCashSwiftDeviceState(CashSwiftDeviceState.CONTROLLER);
                        }
                        else
                        {
                            if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.CONTROLLER))
                                SetCashSwiftDeviceState(CashSwiftDeviceState.CONTROLLER);
                            ResetDevice();
                        }
                    }
                    else
                    {
                        if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.CONTROLLER))
                            SetCashSwiftDeviceState(CashSwiftDeviceState.CONTROLLER);
                        ResetDevice();
                    }
                }
                if (DeviceManager.DeviceManagerMode == DeviceManagerMode.ESCROW_JAM)
                {
                    if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.ESCROW_JAM))
                        SetCashSwiftDeviceState(CashSwiftDeviceState.ESCROW_JAM);
                    if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.DEVICE_MANAGER))
                        UnSetCashSwiftDeviceState(CashSwiftDeviceState.DEVICE_MANAGER);
                }
                else if (ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.ESCROW_JAM))
                    UnSetCashSwiftDeviceState(CashSwiftDeviceState.ESCROW_JAM);
            }
            ApplicationStatus.ControllerStatus = e.ControllerStatus;
            if (DeviceStatusChangedEvent == null)
                return;
            DeviceStatusChangedEvent(null, e);
        }

        private void DeviceManager_RaiseControllerStateChangedEvent(
          object sender,
          ControllerStateChangedEventArgs e)
        {
            if (CurrentApplicationState == ApplicationState.STARTUP)
                Log.DebugFormat(GetType().Name, "OnControllerStateChangedEvent", "EventHandling", "Controller state has changed during startup to {0}", e.ControllerState.ToString());
            else
                Log.DebugFormat(GetType().Name, "OnControllerStateChangedEvent", "EventHandling", "Controller state has changed to {0}", e.ControllerState.ToString());
        }

        private void DeviceManager_DoorOpenEvent(object sender, EventArgs e)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                if (CurrentApplicationState == ApplicationState.CIT_BAG_CLOSED)
                {
                    AlertManager.SendAlert(new AlertSafeDoorOpen(ApplicationModel.GetDevice(DBContext), DateTime.Now, true));
                    CurrentApplicationState = ApplicationState.CIT_DOOR_OPENED;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_DOOR_OPENED");
                }
                else
                {
                    Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "door opened outside of a CIT");
                    AlertManager.SendAlert(new AlertSafeDoorOpen(ApplicationModel.GetDevice(DBContext), DateTime.Now));
                }
            }
        }

        private void DeviceManager_DoorClosedEvent(object sender, EventArgs e)
        {
            lock (DoorClosedLock)
            {
                using (DepositorDBContext DBContext = new DepositorDBContext())
                {
                    Log.Debug(GetType().Name, nameof(DeviceManager_DoorClosedEvent), "EventHandling", "DoorClosedEvent");
                    if (CurrentApplicationState == ApplicationState.CIT_BAG_REPLACED)
                    {
                        AlertManager.SendAlert(new AlertSafeDoorClosed(ApplicationModel.GetDevice(DBContext), DateTime.Now, true));
                        if (lastCIT == null || lastCIT.complete)
                            return;
                        CurrentApplicationState = ApplicationState.CIT_DOOR_CLOSED;
                        Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REPLACED");
                        EndCIT();
                    }
                    else
                    {
                        Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "door closed outside of a CIT");
                        AlertManager.SendAlert(new AlertSafeDoorClosed(ApplicationModel.GetDevice(DBContext), DateTime.Now));
                    }
                }
            }
        }

        private void DeviceManager_BagRemovedEvent(object sender, EventArgs e)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.Debug(GetType().Name, nameof(DeviceManager_BagRemovedEvent), "EventHandling", "BagRemovedEvent");
                if (CurrentApplicationState == ApplicationState.CIT_DOOR_OPENED)
                {
                    CurrentApplicationState = ApplicationState.CIT_BAG_REMOVED;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REMOVED");
                    AlertManager.SendAlert(new AlertBagRemoved(ApplicationModel.GetDevice(DBContext), DateTime.Now, true));
                }
                else
                {
                    Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag removed outside of a CIT");
                    AlertManager.SendAlert(new AlertBagRemoved(ApplicationModel.GetDevice(DBContext), DateTime.Now));
                }
            }
        }

        private void DeviceManager_BagPresentEvent(object sender, EventArgs e)
        {
            lock (BagReplacedLock)
            {
                using (DepositorDBContext DBContext = new DepositorDBContext())
                {
                    if (lastCIT != null && !lastCIT.complete)
                    {
                        CurrentApplicationState = ApplicationState.CIT_BAG_REPLACED;
                        Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REMOVED");
                        AlertManager.SendAlert(new AlertBagInserted(ApplicationModel.GetDevice(DBContext), DateTime.Now, true));
                    }
                    else
                    {
                        Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag inserted outside of a CIT");
                        AlertManager.SendAlert(new AlertBagInserted(ApplicationModel.GetDevice(DBContext), DateTime.Now));
                    }
                }
            }
        }

        private void DeviceManager_BagOpenedEvent(object sender, EventArgs e)
        {
            lock (BagOpenLock)
            {
                if (DeviceManager.DeviceManagerMode != DeviceManagerMode.CIT)
                    return;
                using (DepositorDBContext DBContext = new DepositorDBContext())
                {
                    Guid cit_id = lastCIT.id;
                    CIT last_CIT = DBContext.CITs.Include(y => y.StartUser).Include(z => z.AuthUser).FirstOrDefault(x => x.id == cit_id);
                    Device device = ApplicationModel.GetDevice(DBContext);
                    Log.Debug(GetType().Name, nameof(DeviceManager_BagOpenedEvent), "EventHandling", "BagOpenedEvent");
                    if (last_CIT != null && !last_CIT.complete)
                    {
                        CurrentApplicationState = ApplicationState.CIT_END;
                        Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_END");
                        if (last_CIT != null)
                        {
                            last_CIT.complete = true;
                            last_CIT.cit_complete_date = new DateTime?(DateTime.Now);
                            SaveToDatabase(DBContext);
                            Task.Run(() => PostCITTransactionsAsync(last_CIT, DBContext));
                            DbSet<CIT> ciTs = DBContext.CITs;
                            Expression<Func<CIT, bool>> predicate = x => x.id != last_CIT.id && x.device_id == device.id && x.complete == false;
                            foreach (CIT cit in ciTs.Where(predicate).ToList())
                            {
                                cit.complete = true;
                                cit.cit_complete_date = new DateTime?(DateTime.Now);
                                cit.cit_error = 95;
                                cit.cit_error_message = "Incomplete CIT completed by a newer CIT";
                                AlertManager.SendAlert(new AlertCITFailed(cit, device, DateTime.Now));
                            }
                        }
                        SaveToDatabase(DBContext);
                        AlertManager.SendAlert(new AlertCITSuccess(last_CIT, device, DateTime.Now));
                        DeviceManager.DeviceManagerMode = DeviceManagerMode.NONE;
                        InitialiseApp();
                    }
                    if (last_CIT == null || !last_CIT.complete)
                        Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag opened outside of a CIT");
                }
            }
        }

        private async Task PostCITTransactionsAsync(CIT cit, DepositorDBContext DBContext)
        {
            if (DeviceConfiguration.CIT_ALLOW_POST)
            {
                try
                {
                    if (cit == null)
                        throw new NullReferenceException("null CIT from DB");
                    foreach (CITTransaction CITTransaction in (IEnumerable<CITTransaction>)cit.CITTransactions)
                    {
                        if (CITTransaction.amount > 0L)
                        {
                            if (CITTransaction.CIT.Device.GetCITSuspenseAccount(CITTransaction.currency) != null)
                            {
                                Log.InfoFormat(nameof(ApplicationViewModel), "Posting CITTransaction", "StartCIT", "Posting CITTransaction id={0}, account={1}, suspense={2}, currency={3}, amount={4:#,##0.##}", CITTransaction.id, CITTransaction.account_number, CITTransaction.suspense_account, CITTransaction.currency, CITTransaction.amount / 100.0);
                                var response = await PostCITTransactionToFinacleCoreBankingAsync(cit.id, CITTransaction);
                                CITTransaction.cb_date = new DateTime?(response.TransactionDateTime);
                                CITTransaction.cb_tx_number = response.TransactionID;
                                CITTransaction.cb_tx_status = response.PostResponseCode;
                                CITTransaction.cb_status_detail = response.PostResponseMessage;
                                int code;
                                CITTransaction.error_code = int.TryParse(response.ServerErrorCode, out code) ? code : -1;
                                CITTransaction.error_message = response.ServerErrorMessage;
                                Log.Info(nameof(ApplicationViewModel), "CITPost", nameof(PostCITTransactionsAsync), $"Response = {response.ToString()}");
                                response = null;
                            }
                            else
                                Log.WarningFormat(nameof(ApplicationViewModel), "Posting CITTransaction", "StartCIT", "Error posting CITTransaction id={0}, no CITSuspenseAccount for currency {1}", CITTransaction.id, CITTransaction.currency);
                        }
                        else
                            Log.Warning(nameof(ApplicationViewModel), "CITPost", nameof(PostCITTransactionsAsync), "Skipping CITPost on zero count");
                        SaveToDatabase(DBContext);
                    }
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("ApplicationViewModel.StartCIT", 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), "Error posting CIT {0}: {1}", lastCIT.id, ex.MessageString());
                }
            }
            else
                Log.Info(nameof(ApplicationViewModel), "CIT_ALLOW_POST", nameof(PostCITTransactionsAsync), "Not allowed by config");
            SaveToDatabase(DBContext);
            Log.Trace(nameof(ApplicationViewModel), "CITPost", nameof(PostCITTransactionsAsync), "End of Function");
        }

        private void DeviceManager_BagClosedEvent(object sender, EventArgs e) => Log.Info(GetType().Name, nameof(DeviceManager_BagClosedEvent), "EventHandling", "BagClosedEvent");

        private void DeviceManager_BagFullAlertEvent(object sender, ControllerStatus e)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.WarningFormat(GetType().Name, nameof(DeviceManager_BagFullAlertEvent), "EventHandling", "Percentage={0}%", e.Bag.PercentFull);
                AlertManager.SendAlert(new AlertBagFull(ApplicationModel.GetDevice(DBContext), DateTime.Now, e.Bag));
            }
        }

        private void DeviceManager_BagFullWarningEvent(object sender, ControllerStatus e)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.WarningFormat(GetType().Name, nameof(DeviceManager_BagFullWarningEvent), "EventHandling", "Percentage={0}%", e.Bag.PercentFull);
                AlertManager.SendAlert(new AlertBagFullWarning(ApplicationModel.GetDevice(DBContext), DateTime.Now, e.Bag));
            }
        }

        private void CurrentSession_TransactionLimitReachedEvent(object sender, EventArgs e)
        {
        }

        private void ApplicationStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                using (DepositorDBContext DBContext = new DepositorDBContext())
                {
                    if (e.PropertyName == "CashSwiftDeviceState")
                    {
                        if (ApplicationStatus.CashSwiftDeviceState != CashSwiftDeviceState.NONE)
                        {
                            DepositorLogger log = Log;
                            string name = GetType().Name;
                            object[] objArray = new object[1];
                            CashSwiftDeviceStatus applicationStatus = ApplicationStatus;
                            CashSwiftDeviceState cashSwiftDeviceState1;
                            string str;
                            if (applicationStatus == null)
                            {
                                str = null;
                            }
                            else
                            {
                                cashSwiftDeviceState1 = applicationStatus.CashSwiftDeviceState;
                                str = cashSwiftDeviceState1.ToString();
                            }
                            objArray[0] = str;
                            log.TraceFormat(name, "ApplicationStatus_Error_Locking", "Locking Device on Error", "CashSwiftDeviceState = {0}", objArray);
                            int num;
                            if (CurrentSession != null)
                            {
                                IDeviceManager deviceManager = DeviceManager;
                                num = deviceManager != null ? (deviceManager.DeviceManagerMode == DeviceManagerMode.NONE ? 1 : 0) : 0;
                            }
                            else
                                num = 0;

                            if (num != 0)
                            {
                                Log.Error(nameof(ApplicationViewModel), "Session Error", nameof(ApplicationStatus_PropertyChanged), $"DeviceManagerMode = {DeviceManager?.DeviceManagerMode}, CashSwiftDeviceState= {ApplicationStatus.CashSwiftDeviceState}");
                                int cashSwiftDeviceState2 = (int)ApplicationStatus.CashSwiftDeviceState;
                                cashSwiftDeviceState1 = ApplicationStatus.CashSwiftDeviceState;
                                string errormessage = cashSwiftDeviceState1.ToString();
                                EndSession(false, cashSwiftDeviceState2, ApplicationErrorConst.ERROR_SYSTEM, errormessage);
                            }
                            if (DeviceManager == null || DeviceManager.DeviceManagerMode != DeviceManagerMode.ESCROW_JAM)
                            {
                                Log.WarningFormat(nameof(ApplicationViewModel), nameof(ApplicationStatus_PropertyChanged), "Showing OutOfOrder screen", $"{DeviceManager?.DeviceManagerMode} != CashAccSysDeviceManager.DeviceManagerMode.ESCROW_JAM", DeviceManager?.DeviceManagerMode);
                                ShowErrorDialog(new OutOfOrderScreenViewModel(this));
                            }
                        }
                        else if (CurrentSession != null)
                        {
                            Log.Error(nameof(ApplicationViewModel), "Session Error", nameof(ApplicationStatus_PropertyChanged), $"Application has no errors: DeviceManagerMode = {DeviceManager?.DeviceManagerMode}, CashSwiftDeviceState= {ApplicationStatus.CashSwiftDeviceState}");
                            EndSession(false, (int)ApplicationStatus.CashSwiftDeviceState, ApplicationErrorConst.ERROR_SYSTEM, "State OK now, but still in session. CashSwiftDeviceState=" + ApplicationStatus.CashSwiftDeviceState.ToString());
                        }
                        else if (!AdminMode)
                        {
                            CurrentApplicationState = ApplicationState.SPLASH;
                            InitialiseApp();
                        }
                    }
                    DeviceStatus DeviceStatus = DBContext.DeviceStatus.FirstOrDefault(f => f.machine_name == Environment.MachineName);
                    if (DeviceStatus == null)
                    {
                        DeviceStatus = CashSwiftDepositCommonClasses.GenerateDeviceStatus(DBContext.DeviceStatus.FirstOrDefault(f => f.machine_name == Environment.MachineName).id, DBContext);
                    }
                    DeviceStatus.current_status = (int)ApplicationStatus.CashSwiftDeviceState;
                    DeviceStatus.modified = new DateTime?(DateTime.Now);
                    SaveToDatabase(DBContext);
                }
            }
            catch (Exception ex)
            {
                Log.Error(nameof(ApplicationViewModel), "ApplicationViewModel ", "ApplicationStatus_PropertyChanged", "Error Message: {0}", ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private void DeviceManager_RaiseDeviceStateChangedEvent(
          object sender,
          DeviceStateChangedEventArgs e)
        {
            Log?.DebugFormat(GetType().Name, "OnRaiseDeviceStateChangedEvent", "EventHandling", "Device status has changed to {0}", e.Data.ToString());
            if (CurrentApplicationState == ApplicationState.DEVICE_ERROR)
            {
                if (e.Data == DeviceState.JAM)
                    return;
                if (CurrentTransaction != null)
                {
                    AlertManager.SendAlert(new AlertNoteJamClearSuccess(CurrentTransaction.Transaction, CurrentSession.Device, DateTime.Now));
                    Log?.Info(GetType().Name, "OnRaiseDeviceStateChangedEvent", "EventHandling", "Note Jam Cleared");
                    EndTransaction(ApplicationErrorConst.ERROR_DEVICE_NOTEJAM, "Note Jam");
                }
                EndSession(false, 227, ApplicationErrorConst.ERROR_DEVICE_NOTEJAM, "Note Jam detected, terminating transaction");
            }
            else
            {
                if (e.Data != DeviceState.JAM)
                    return;
                Log?.Error(GetType().Name, 87, "OnRaiseDeviceStateChangedEvent", "Note Jam Detected");
                CurrentApplicationState = ApplicationState.DEVICE_ERROR;
                if (CurrentSession != null)
                    CurrentSession.CountingEnded = true;
                if (CurrentTransaction != null)
                {
                    AlertManager.SendAlert(new AlertNoteJam(CurrentTransaction, CurrentSession.Device, DateTime.Now));
                    CurrentTransaction.NoteJamDetected = true;
                }
                ShowDialog(new NoteJamScreenViewModel(this));
            }
        }

        public event EventHandler<EventArgs> NotifyCurrentTransactionStatusChangedEvent;

        private void DeviceManager_NotifyCurrentTransactionStatusChangedEvent(
          object sender,
          EventArgs e)
        {
            if (NotifyCurrentTransactionStatusChangedEvent == null)
                return;
            NotifyCurrentTransactionStatusChangedEvent(this, e);
        }

        private void DeviceManager_ConnectionEvent(object sender, StringResult e)
        {
            Log.DebugFormat(GetType().Name, "OnConnectionEvent", "EventHandling", "Connection state = {0}", e.data);
            if (CurrentApplicationState != ApplicationState.STARTUP)
                return;
            if (e.resultCode == 0)
            {
                Console.WriteLine("Connection to DC Successfull");
                Log.Debug(GetType().Name, "OnConnectionEvent", "EventHandling", "Connection to DC Successfull");
                if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.COUNTING_DEVICE))
                    return;
                UnSetCashSwiftDeviceState(CashSwiftDeviceState.COUNTING_DEVICE);
            }
            else
            {
                if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.COUNTING_DEVICE))
                    SetCashSwiftDeviceState(CashSwiftDeviceState.COUNTING_DEVICE);
                Console.WriteLine("Connection to DC FAILED: {0}", e.message);
                Console.WriteLine("Attempting to reconnect");
                DeviceManager.Connect();
            }
        }

        internal void DebugOnTransactionStartedEvent(object sender, DeviceTransaction e) => DeviceManager_TransactionStartedEvent(sender, e);

        private void DeviceManager_TransactionStartedEvent(object sender, DeviceTransaction e)
        {
            if (CurrentApplicationState == ApplicationState.COUNT_STARTED)
            {
                CurrentApplicationState = ApplicationState.COUNTING;
                CashInStart();
            }
            else
                Log.WarningFormat(GetType().Name, nameof(DeviceManager_TransactionStartedEvent), "Invalid State", "DeviceManager_TransactionStartedEvent invalid in current state: {0}", CurrentApplicationState);
        }

        public event EventHandler<DeviceTransactionResult> TransactionStatusEvent;

        private void DeviceManager_TransactionStatusEvent(object sender, DeviceTransactionResult e)
        {
            Log.InfoFormat(GetType().Name, nameof(DeviceManager_TransactionStatusEvent), "EventHandling", "Escrow Total = {0}, Safe Total = {1}", e?.data?.CurrentTransactionResult?.EscrowTotalDisplayString, e?.data?.CurrentTransactionResult?.TotalDroppedAmountDisplayString);
            if (!InSessionAndTransaction("DeviceManager_TransactionEndEvent()"))
                return;
            string sessionId = e?.data?.SessionID;
            Guid guid = CurrentSession.SessionID;
            string b1 = guid.ToString();
            if (string.Equals(sessionId, b1, StringComparison.InvariantCultureIgnoreCase))
            {
                string transactionId = e?.data?.TransactionID;
                guid = CurrentTransaction.Transaction.id;
                string b2 = guid.ToString();
                if (string.Equals(transactionId, b2, StringComparison.InvariantCultureIgnoreCase))
                {
                    AppTransaction currentTransaction1 = CurrentTransaction;
                    int num1;
                    if (currentTransaction1 == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        AppSession session = currentTransaction1.Session;
                        if (session == null)
                        {
                            num1 = 0;
                        }
                        else
                        {
                            int num2 = session.CountingStarted ? 1 : 0;
                            num1 = 1;
                        }
                    }
                    if (num1 != 0)
                    {
                        AppTransaction currentTransaction2 = CurrentTransaction;
                        int num3;
                        if (currentTransaction2 == null)
                        {
                            num3 = 0;
                        }
                        else
                        {
                            bool? countingStarted = currentTransaction2.Session?.CountingStarted;
                            bool flag = true;
                            num3 = countingStarted.GetValueOrDefault() == flag & countingStarted.HasValue ? 1 : 0;
                        }
                        if (num3 != 0)
                        {
                            CurrentSession.HasCounted = true;
                            if (e.level == ErrorLevel.SUCCESS)
                            {
                                AppTransaction currentTransaction3 = CurrentTransaction;
                                TransactionStatusResponseResult TransactionResult = new TransactionStatusResponseResult
                                {
                                    level = e.level,
                                    data = e.data.CurrentTransactionResult
                                };
                                currentTransaction3.HandleDenominationResult(TransactionResult);
                                if (TransactionStatusEvent == null)
                                    return;
                                TransactionStatusEvent(this, e);
                                return;
                            }
                            Log.Warning(GetType().Name, nameof(DeviceManager_TransactionStatusEvent), "InvalidOperation", "FAILED: e.level == ErrorLevel.SUCCESS");
                            return;
                        }
                    }
                    Log.Warning(GetType().Name, nameof(DeviceManager_TransactionStatusEvent), "InvalidOperation", "FAILED: CurrentTransaction?.Session?.CountingStarted != null && CurrentTransaction?.Session?.CountingStarted != true");
                }
                else
                {
                    DepositorLogger log = Log;
                    string name = GetType().Name;
                    object[] objArray = new object[3]
                    {
             e.data?.SessionID,
             e?.data?.TransactionID,
            null
                    };
                    guid = CurrentTransaction.Transaction.id;
                    objArray[2] =  guid.ToString();
                    log.WarningFormat(name, nameof(DeviceManager_TransactionStatusEvent), "InvalidOperation", "TransactionStatusEvent received for TransactionID{0} while CurrentTransactionID is {1}", objArray);
                    DeviceManager.ResetDevice(false);
                }
            }
            else
            {
                DepositorLogger log = Log;
                string name = GetType().Name;
                object[] objArray = new object[3]
                {
           e.data?.SessionID,
           e?.data?.SessionID,
          null
                };
                guid = CurrentSession.SessionID;
                objArray[2] =  guid.ToString();
                log.WarningFormat(name, nameof(DeviceManager_TransactionStatusEvent), "InvalidOperation", "TransactionStatusEvent received for SessionID{0} while CurrentSessionID is {1}", objArray);
                DeviceManager.ResetDevice(false);
            }
        }


        public event EventHandler<DeviceTransactionResult> CashInStartedEvent;

        private void DeviceManager_CashInStartedEvent(object sender, DeviceTransactionResult e)
        {
            if (DeviceConfiguration.START_COUNT_AUTOMATICALLY)
                Count();
            if (CashInStartedEvent == null)
                return;
            CashInStartedEvent(null, e);
        }

        public event EventHandler<DeviceTransactionResult> CountStartedEvent;

        private void DeviceManager_CountStartedEvent(object sender, DeviceTransactionResult e)
        {
            if (!InSessionAndTransaction("ApplicationViewModel.DeviceManager_CountStartedEvent()") || CountStartedEvent == null)
                return;
            CountStartedEvent(null, e);
        }

        public event EventHandler<DeviceTransactionResult> CountPauseEvent;

        private void DeviceManager_CountPauseEvent(object sender, DeviceTransactionResult e)
        {
            if (CurrentSession != null)
            {
                if (CurrentTransaction != null)
                {
                    if (CurrentApplicationState != ApplicationState.COUNTING || CountPauseEvent == null)
                        return;
                    CountPauseEvent(null, e);
                }
                else
                {
                    Log.WarningFormat(GetType().Name, "DropPauseEvent", "InvalidOperation", "DropPausedEvent received outside of a GUI transaction. Session={0} Transaction={1}", e.data?.SessionID, e?.data?.TransactionID);
                    DeviceManager.ResetDevice();
                }
            }
            else
            {
                Log.WarningFormat(GetType().Name, "DropPauseEvent", "InvalidOperation", "DropPausedEvent received outside of a GUI Session. Session={0}", e.data?.SessionID);
                DeviceManager.ResetDevice();
            }
        }

        public event EventHandler<DeviceTransactionResult> EscrowRejectEvent;

        private void DeviceManager_EscrowRejectEvent(object sender, DeviceTransactionResult e)
        {
            if (EscrowRejectEvent == null)
                return;
            EscrowRejectEvent(null, e);
            EscrowRejectEvent(null, e);
        }

        public event EventHandler<DeviceTransactionResult> EscrowDropEvent;

        private void DeviceManager_EscrowDropEvent(object sender, DeviceTransactionResult e)
        {
            if (EscrowDropEvent == null)
                return;
            EscrowDropEvent(null, e);
        }

        public event EventHandler<DeviceTransactionResult> EscrowOperationCompleteEvent;

        private void DeviceManager_EscrowOperationCompleteEvent(
          object sender,
          DeviceTransactionResult e)
        {
            if (EscrowOperationCompleteEvent == null)
                return;
            EscrowOperationCompleteEvent(null, e);
        }

        public event EventHandler<DeviceTransactionResult> CountEndEvent;

        private void DeviceManager_CountEndEvent(object sender, DeviceTransactionResult e)
        {
            if (!InSessionAndTransaction("ApplicationViewModel.DeviceManager_CountEndEvent()") || CountEndEvent == null)
                return;
            CountEndEvent(null, e);
        }

        private void DeviceManager_TransactionEndEvent(object sender, DeviceTransactionResult e)
        {
            Log.InfoFormat(GetType().Name, nameof(DeviceManager_TransactionEndEvent), "EventHandling", "Transaction Result: Currency = {0}, TotalDeposit = {1}, TotalDispense = {2}, Result = {3}", e?.data?.Currency, e?.data?.CurrentTransactionResult?.TotalDroppedAmountDisplayString, e?.data?.CurrentTransactionResult?.DispensedAmountDisplayString, e?.data?.CurrentTransactionResult?.ResultAmountDisplayString);
            if (!InSessionAndTransaction("DeviceManager_TransactionEndEvent()", false))
                return;
            string sessionId = e?.data?.SessionID;
            Guid guid = CurrentSession.SessionID;
            string b1 = guid.ToString();
            if (string.Equals(sessionId, b1, StringComparison.InvariantCultureIgnoreCase))
            {
                string transactionId = e?.data?.TransactionID;
                guid = CurrentTransaction.Transaction.id;
                string b2 = guid.ToString();
                if (string.Equals(transactionId, b2, StringComparison.InvariantCultureIgnoreCase))
                {
                    AppTransaction currentTransaction1 = CurrentTransaction;
                    int num1;
                    if (currentTransaction1 == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        AppSession session = currentTransaction1.Session;
                        if (session == null)
                        {
                            num1 = 0;
                        }
                        else
                        {
                            int num2 = session.CountingEnded ? 1 : 0;
                            num1 = 1;
                        }
                    }
                    int num3;
                    if (num1 != 0)
                    {
                        AppTransaction currentTransaction2 = CurrentTransaction;
                        if (currentTransaction2 == null)
                        {
                            num3 = 1;
                        }
                        else
                        {
                            bool? countingEnded = currentTransaction2.Session?.CountingEnded;
                            bool flag = true;
                            num3 = !(countingEnded.GetValueOrDefault() == flag & countingEnded.HasValue) ? 1 : 0;
                        }
                    }
                    else
                        num3 = 0;
                    if (num3 != 0)
                    {
                        CurrentTransaction.Session.CountingEnded = true;
                        CurrentSession.HasCounted = true;
                        if (e.level == ErrorLevel.SUCCESS)
                        {
                            if (CurrentTransaction != null)
                            {
                                AppTransaction currentTransaction3 = CurrentTransaction;
                                TransactionStatusResponseResult TransactionResult = new TransactionStatusResponseResult();
                                TransactionResult.level = e.level;
                                TransactionResult.data = e.data.CurrentTransactionResult;
                                currentTransaction3.HandleDenominationResult(TransactionResult);
                                ValidateTransactedAmount();
                            }
                            else
                                Log.Warning(GetType().Name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "FAILED: CurrentTransaction != null");
                        }
                        else
                            Log.Warning(GetType().Name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "FAILED: e.level == ErrorLevel.SUCCESS");
                    }
                    else
                        Log.Warning(GetType().Name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "FAILED: CurrentTransaction?.Session?.CountingEnded != null && CurrentTransaction?.Session?.CountingEnded != true");
                }
                else
                {
                    DepositorLogger log = Log;
                    string name = GetType().Name;
                    object[] objArray = new object[3]
                    {
             e.data?.SessionID,
             e?.data?.TransactionID,
            null
                    };
                    guid = CurrentTransaction.Transaction.id;
                    objArray[2] = guid.ToString();
                    log.WarningFormat(name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "TransactionStatusEvent received for TransactionID{0} while CurrentTransactionID is {1}", objArray);
                    DeviceManager.ResetDevice();
                }
            }
            else
            {
                DepositorLogger log = Log;
                string name = GetType().Name;
                object[] objArray = new object[3]
                {
           e.data?.SessionID,
           e?.data?.SessionID,
          null
                };
                guid = CurrentSession.SessionID;
                objArray[2] = guid.ToString();
                log.WarningFormat(name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "TransactionStatusEvent received for SessionID{0} while CurrentSessionID is {1}", objArray);
                DeviceManager.ResetDevice();
            }
        }

        private void DeviceManager_CITResultEvent(object sender, CITResult e)
        {
            using (new DepositorDBContext())
            {
                Log.Debug(GetType().Name, "OnCITResultEvent", "EventHandling", "CIT Result");
                if (DeviceManager.DeviceManagerMode == DeviceManagerMode.CIT)
                {
                    if (e.level != ErrorLevel.SUCCESS)
                        return;
                    CurrentApplicationState = ApplicationState.CIT_BAG_CLOSED;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_CLOSED");
                }
                else
                    Log.Warning(nameof(ApplicationViewModel), "InvalidOperationException", nameof(DeviceManager_CITResultEvent), "CITResult received outside CIT. DeviceManager.DeviceManagerMode = '{0}'", new object[1]
                    {
             DeviceManager.DeviceManagerMode
                    });
            }
        }

        private void Printer_StatusChangedEvent(
          object sender,
          DepositorPrinter.PrinterStateChangedEventArgs e)
        {
            if ((ApplicationStatus.CashSwiftDeviceState & CashSwiftDeviceState.PRINTER) == CashSwiftDeviceState.PRINTER)
            {
                if (e.state.HasError || !ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.PRINTER))
                    return;
                UnSetCashSwiftDeviceState(CashSwiftDeviceState.PRINTER);
            }
            else
            {
                if (e.state.HasError || ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.PRINTER))
                    return;
                SetCashSwiftDeviceState(CashSwiftDeviceState.PRINTER);
            }
        }

        private void ApplicationModel_DatabaseStorageErrorEvent(object sender, EventArgs e)
        {
            if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.DATABASE))
                return;
            UnSetCashSwiftDeviceState(CashSwiftDeviceState.DATABASE);
        }

        private void OnApplicationStartupFailedEvent(
          object sender,
          ApplicationErrorConst e,
          string errorMessage)
        {
            Log.ErrorFormat(GetType().Name, (int)e, e.ToString(), "{0:G}: {1}", e, errorMessage);
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Device device = ApplicationModel.GetDevice(DBContext);
                if (AlertManager != null)
                {
                    if (device != null)
                        AlertManager.SendAlert(new AlertDeviceStartupFailed(string.Format("{0:G}: {1}", e, errorMessage), device, DateTime.Now));
                }
            }
            Console.WriteLine(string.Format(string.Format("{0:G}: {1}", e, errorMessage)));
            Application.Current.Shutdown((int)e);
        }

        public event EventHandler<DeviceStatusChangedEventArgs> DeviceStatusChangedEvent;

        internal void ConnectToDevice()
        {
            Log.Debug(GetType().Name, nameof(ConnectToDevice), "EventHandling", "Connecting to the device");
            if (DebugNoDevice)
            {
                StringResult e = new StringResult
                {
                    resultCode = 0,
                    extendedResult = "ACCEPTED",
                    level = ErrorLevel.SUCCESS
                };
                DeviceManager_ConnectionEvent(this, e);
            }
            else
                DeviceManager.Connect();
        }

        internal void DeviceTransactionStart(long transactionLimitCents = 0, long transactionValueCents = 0)
        {
            if (CurrentSession == null)
                throw new NullReferenceException("CurrentSession cannot be null in ApplicationViewModel.TransactionStart()");
            if (CurrentTransaction == null)
                throw new NullReferenceException("CurrentTransaction cannot be null in ApplicationViewModel.TransactionStart()");
            if (DebugNoDevice)
                return;
            try
            {
                string currencyCode = CurrentTransaction.CurrencyCode;
                string accountNumber = CurrentTransaction?.AccountNumber;
                AppSession currentSession = CurrentSession;
                Guid guid;
                string sessionID;
                if (currentSession == null)
                {
                    sessionID =  null;
                }
                else
                {
                    guid = currentSession.SessionID;
                    sessionID = guid.ToString().ToUpperInvariant();
                }
                AppTransaction currentTransaction = CurrentTransaction;
                string transactionID;
                if (currentTransaction == null)
                {
                    transactionID =  null;
                }
                else
                {
                    guid = currentTransaction.Transaction.id;
                    transactionID = guid.ToString().ToUpperInvariant();
                }
                long transactionLimitCents1 = transactionLimitCents;
                long transactionValueCents1 = transactionValueCents;
                DeviceManager.TransactionStart(currencyCode, accountNumber, sessionID, transactionID, transactionLimitCents, transactionValueCents);
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine("DeviceTransaction already in progress");
                Log.WarningFormat(GetType().Name, "DeviceTransactionStart Failed", "DeviceTransactionStart Failed", "DeviceTransaction already in progress: {0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
                ResetDevice();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("something went wrong. Reset the DeviceManager");
                Log.WarningFormat(GetType().Name, "DeviceTransactionStart Failed", "DeviceTransactionStart Failed", "ERROR: {0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
                ResetDevice();
            }
        }

        internal void CashInStart()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "CashInStart()");
            AppTransaction currentTransaction = CurrentTransaction;
            int num1;
            if (currentTransaction == null)
            {
                num1 = 0;
            }
            else
            {
                AppSession session = currentTransaction.Session;
                if (session == null)
                {
                    num1 = 0;
                }
                else
                {
                    int num2 = session.CountingStarted ? 1 : 0;
                    num1 = 1;
                }
            }
            if (num1 == 0)
                throw new NullReferenceException("Session cannot be null when calling CashInStart()");
            CurrentTransaction.Session.CountingStarted = true;
            if (DebugNoDevice)
                return;
            DeviceManager.CashInStart();
        }

        public bool CanCount => DeviceManager.CanCount && InSessionAndTransaction("ApplicationViewModel.CanCount()", false);

        internal void Count(bool countNotes = true)
        {
            if (DebugNoDevice)
                return;
            Log.Info(GetType().Name, nameof(Count), "Device Management", "Request Count");
            if (!InSessionAndTransaction("ApplicationViewModel.Count()"))
                return;
            DeviceManager.Count();
        }

        internal void PauseCount()
        {
            if (DebugNoDevice || ApplicationStatus.ControllerStatus.ControllerState == ControllerState.DROP_PAUSED)
                return;
            Log.Info(GetType().Name, "Count", "Device Management", "PauseCount()");
            if (!InSessionAndTransaction("ApplicationViewModel.PauseCount()"))
                return;
            DeviceManager.PauseCount();
        }
        public bool CanPauseCount
        {
            get
            {
                int num;
                if (InSessionAndTransaction("ApplicationViewModel.CanPauseCount()", false) && DeviceManager.CanPauseCount)
                {
                    long? droppedAmountCents1 = CurrentTransaction?.DroppedAmountCents;
                    long? droppedAmountCents2 = DeviceManager?.DroppedAmountCents;
                    num = droppedAmountCents1.GetValueOrDefault() == droppedAmountCents2.GetValueOrDefault() & droppedAmountCents1.HasValue == droppedAmountCents2.HasValue ? 1 : 0;
                }
                else
                    num = 0;
                return num != 0;
            }
        }

        public bool CanEndCount => InSessionAndTransaction("ApplicationViewModel.CanEndCount()", false) && DeviceManager.CanEndCount;

        public bool CanEscrowDrop => InSessionAndTransaction("ApplicationViewModel.CanEscrowDrop()", false) && DeviceManager.CanEscrowDrop;

        internal void EscrowDrop()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "EscrowDrop()");
            if (DebugNoDevice)
                throw new NotImplementedException("Debug EscrowDrop() not implemented");
            if (!InSessionAndTransaction(GetType().Name + ".EscrowDrop()"))
                return;
            DeviceManager.EscrowDrop();
        }

        public bool CanEscrowReject => InSessionAndTransaction("ApplicationViewModel.CanEscrowReject()", false) && DeviceManager.CanEscrowReject;

        internal void EscrowReject()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "EscrowReject()");
            if (DebugNoDevice)
                throw new NotImplementedException("Debug EscrowReject() not implemented");
            if (!InSessionAndTransaction(GetType().Name + ".EscrowReject()"))
                return;
            DeviceManager.EscrowReject();
        }

        public bool CanTransactionEnd => InSessionAndTransaction("ApplicationViewModel.CanTransactionEnd", false) && DeviceManager.CanTransactionEnd;

        internal void DeviceTransactionEnd()
        {
            Log.Info(GetType().Name, "Count", "DeviceTransactionEnd()", "DeviceTransactionEnd()");
            DeviceManager.TransactionEnd();
        }

        internal void ValidateTransactedAmount()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "ValidateTransactedAmount()");
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                if (CurrentTransaction == null)
                    return;
                TransactionLimitListItem transactionLimits1 = CurrentTransaction.TransactionLimits;
                long? nullable1;
                if ((transactionLimits1 != null ? (transactionLimits1.show_funds_source ? 1 : 0) : 0) != 0)
                {
                    Decimal droppedDisplayAmount = CurrentTransaction.DroppedDisplayAmount;
                    nullable1 = CurrentTransaction.TransactionLimits?.funds_source_amount;
                    Decimal? nullable2 = nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault()) : new Decimal?();
                    Decimal valueOrDefault = nullable2.GetValueOrDefault();
                    if (droppedDisplayAmount > valueOrDefault & nullable2.HasValue)
                    {
                        DepositorLogger log = Log;
                        string name = GetType().Name;
                        object[] objArray = new object[2]
                        {
               CurrentTransaction.DroppedDenomination.TotalValue / 100L,
              null
                        };
                        TransactionLimitListItem transactionLimits2 = CurrentTransaction.TransactionLimits;
                        long? nullable3;
                        if (transactionLimits2 == null)
                        {
                            nullable1 = new long?();
                            nullable3 = nullable1;
                        }
                        else
                            nullable3 = new long?(transactionLimits2.funds_source_amount);
                        objArray[1] =  nullable3;
                        log.InfoFormat(name, "Count", "Device Management", "Transaction of {0} is above value limit of {1}, insert FundsSource screen", objArray);
                        ShowFundsSourceScreen(DBContext);
                        return;
                    }
                }
                TransactionLimitListItem transactionLimits3 = CurrentTransaction.TransactionLimits;
                if ((transactionLimits3 != null ? (transactionLimits3.prevent_overdeposit ? 1 : 0) : 0) != 0 && CurrentTransaction.IsOverDeposit)
                {
                    string title = CashSwiftTranslationService?.TranslateSystemText(GetType().Name + ".PerformSelection", "sys_Transaction_Limit_Exceeded_Title", "Transaction Limit Exceeded");
                    string format = CashSwiftTranslationService?.TranslateSystemText(GetType().Name + ".PerformSelection", "sys_Transaction_Limit_Exceeded_Message", "Transaction limit of {0} {1} has been exceeded. Please visit your nearest branch to finish the deposit.");
                    string upper = CurrentTransaction.CurrencyCode.ToUpper();
                    TransactionLimitListItem transactionLimits4 = CurrentTransaction.TransactionLimits;
                    long? nullable4;
                    if (transactionLimits4 == null)
                    {
                        nullable1 = new long?();
                        nullable4 = nullable1;
                    }
                    else
                        nullable4 = new long?(transactionLimits4.overdeposit_amount);

                    var local = (ValueType)nullable4;
                    string message = string.Format(format, upper, local);
                    ShowUserMessageScreen(title, message);
                    DepositorLogger log = Log;
                    string name = GetType().Name;
                    string ErrorName = ApplicationErrorConst.ERROR_TRANSACTION_LIMIT_EXCEEDED.ToString();
                    object[] objArray1 = new object[5];
                    Guid id = CurrentTransaction.Transaction.id;
                    objArray1[0] =  id.ToString().ToUpper();
                    objArray1[1] =  (CurrentTransaction?.CurrencyCode?.ToUpper());
                    objArray1[2] =  (CurrentTransaction?.DroppedDisplayAmount);
                    objArray1[3] =  (CurrentTransaction?.TransactionType?.default_account_currency?.ToUpper());
                    TransactionLimitListItem transactionLimits5 = CurrentTransaction.TransactionLimits;
                    long? nullable5;
                    if (transactionLimits5 == null)
                    {
                        nullable1 = new long?();
                        nullable5 = nullable1;
                    }
                    else
                        nullable5 = new long?(transactionLimits5.overdeposit_amount);
                    objArray1[4] =  nullable5;
                    log.ErrorFormat(name, 27, ErrorName, "Credit Blocked: Transaction [{0}] of Amount {1} {2:###,##0.00} is over the limit of {3} {4:###,##0.00}.", objArray1);
                    object[] objArray2 = new object[5];
                    id = CurrentTransaction.Transaction.id;
                    objArray2[0] =  id.ToString().ToUpper();
                    objArray2[1] =  (CurrentTransaction?.CurrencyCode?.ToUpper());
                    objArray2[2] =  (CurrentTransaction?.DroppedDisplayAmount);
                    objArray2[3] =  (CurrentTransaction?.TransactionType?.default_account_currency?.ToUpper());
                    TransactionLimitListItem transactionLimits6 = CurrentTransaction.TransactionLimits;
                    long? nullable6;
                    if (transactionLimits6 == null)
                    {
                        nullable1 = new long?();
                        nullable6 = nullable1;
                    }
                    else
                        nullable6 = new long?(transactionLimits6.overdeposit_amount);
                    objArray2[4] =  nullable6;
                    EndTransaction(ApplicationErrorConst.ERROR_TRANSACTION_LIMIT_EXCEEDED, string.Format("Credit Blocked: Transaction [{0}] of Amount {1} {2:###,##0.00} is over the limit of {3} {4:###,##0.00}.", objArray2));
                }
                else
                {
                    AppTransaction currentTransaction = CurrentTransaction;

                    if ((currentTransaction != null ? (currentTransaction.NoteJamDetected ? 1 : 0) : 0) != 0)
                    {
                        EndTransaction(ApplicationErrorConst.ERROR_DEVICE_NOTEJAM, ApplicationErrorConst.ERROR_DEVICE_NOTEJAM.ToString());
                    }
                    else if ((currentTransaction != null ? (currentTransaction.EscrowJamDetected ? 1 : 0) : 0) != 0)
                    {
                        EndTransaction(ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM, ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM.ToString());
                    }
                    else
                    {
                        Log.Info(GetType().Name, "Count", "Device Management", "Transaction below limit, continue as normal");
                        EndTransaction();
                        NavigateNextScreen();
                    }
                }
            }
        }

        internal void ShowFundsSourceScreen(DepositorDBContext DBContext)
        {
            if ((bool)!CurrentTransaction?.TransactionLimits?.show_funds_source)
                return;
            GUIScreen entity = DBContext.GUIScreens.Where(x => x.GUIScreenType.code == new Guid("33EC330E-FB51-4626-906D-1A3F77AAA5E2")).OrderBy(y => y.id).FirstOrDefault();
            if (entity != null)
            {
                GUIScreens.Insert(CurrentScreenIndex + 1, entity);
                ((IObjectContextAdapter)DBContext).ObjectContext.Detach(entity);
                NavigateNextScreen();
            }
            else
            {
                Log.Error(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "cannot find FundsSourceScreen in database");
                EndTransaction(ApplicationErrorConst.ERROR_DATABASE_GENERAL, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString());
            }
        }

        internal void ShowUserMessageScreen(string title, string message, bool required = false) => ActivateItemAsync(new UserMessageScreenViewModel(title, this, DeviceConfiguration.USER_SCREEN_TIMEOUT, required)
        {
            Message = message
        });

        internal void EndTransaction(ApplicationErrorConst result = ApplicationErrorConst.ERROR_NONE, string ErrorMessage = null)
        {
            Log.InfoFormat(GetType().Name, nameof(EndTransaction), "Device Management", "EndTransaction with result {0}", result.ToString());
            if (CurrentTransaction == null)
                return;
            if (result == ApplicationErrorConst.ERROR_NONE || result == ApplicationErrorConst.ERROR_DEVICE_NOTEJAM && DeviceConfiguration.POST_ON_NOTEJAM || result == ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM && DeviceConfiguration.POST_ON_ESCROWJAM || result == ApplicationErrorConst.WARN_DEPOSIT_TIMEOUT)
            {
                CurrentTransaction.EndDate = DateTime.Now;
                if (CurrentTransaction.DroppedAmountCents > 0L)
                {
                    try
                    {
                        lock (CurrentTransaction.PostingLock)
                        {
                            if (!CurrentTransaction.isPosting)
                            {
                                if (!CurrentTransaction.hasPosted)
                                {
                                    Log.InfoFormat(GetType().Name, "Attempt", "Posting", "Transaction [{0}] of Amount {1} {2:###,##0.00} is NOT over the limit of {3} {4:###,##0.00} or overdeposit is disabled.", CurrentTransaction.Transaction.id.ToString().ToUpper(), CurrentTransaction?.CurrencyCode?.ToUpper(), CurrentTransaction?.DroppedDisplayAmount, CurrentTransaction?.TransactionType?.default_account_currency?.ToUpper(), CurrentTransaction.TransactionLimits?.overdeposit_amount);
                                    CurrentTransaction.isPosting = true;
                                    CurrentTransaction.hasPosted = true;
                                    PostTransactionResponse response = Task.Run(() => PostToCoreBankingAsync(SessionID.Value, CurrentTransaction.Transaction)).Result;
                                    CurrentTransaction.Transaction.cb_date = new DateTime?(response.TransactionDateTime);
                                    CurrentTransaction.Transaction.cb_tx_number = response.TransactionID;
                                    CurrentTransaction.Transaction.cb_tx_status = response.PostResponseCode;
                                    CurrentTransaction.Transaction.cb_status_detail = response.PostResponseMessage;
                                    EscrowJam escrowJam = CurrentTransaction.Transaction.EscrowJams.FirstOrDefault();
                                    if (escrowJam != null)
                                        escrowJam.posted_amount = CurrentTransaction.Transaction.tx_amount.GetValueOrDefault();
                                    if (response.IsSuccess)
                                    {
                                        Log.InfoFormat(GetType().Name, "Attempt", "Posting", "Post SUCCESS with txID = {0}", response?.TransactionID);
                                        try
                                        {
                                            AlertManager.SendAlert(new AlertTransactionCustomerAlert(CurrentTransaction, CurrentSession.Device, DateTime.Now));
                                        }
                                        catch (Exception ex)
                                        {
                                            Log.Error("AppTransaction", "Send Customer SMS", nameof(EndTransaction), "Could not send customer SMS with error: {0}", new object[1] { ex.MessageString() });
                                        }
                                        CurrentTransaction.EndTransaction(result, ErrorMessage);
                                        Log.InfoFormat(GetType().Name, "Restart", "Clear Cache", "Done Post SUCCESS with txID = {0}", response?.TransactionID);

                                    }
                                    else
                                        CurrentTransaction.EndTransaction(ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE, response.ServerErrorMessage);
                                }
                                else
                                    Log.WarningFormat(GetType().Name, "PostingInProgress", "Posting", "Transaction [{0}] has already posted", CurrentTransaction?.Transaction?.id);
                            }
                            else
                                Log.WarningFormat(GetType().Name, "PostingInProgress", "Posting", "Transaction [{0}] is already posting", CurrentTransaction?.Transaction?.id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat(GetType().Name, 91, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString(), "Transaction posting failed>>ERROR: {0}", ex.MessageString());
                        CurrentTransaction?.EndTransaction(ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString());
                    }
                }
                else
                {
                    Log.Warning(GetType().Name, "Skip Post on Zero Count", "Posting", "Posting skipped on account of zero amount counted");
                    CurrentTransaction.EndTransaction(result, ErrorMessage);
                }
            }
            else
            {
                CurrentTransaction.EndTransaction(result, ErrorMessage);
            }

            if (CurrentTransaction == null)
                return;
            CurrentSession.Transaction =  null;


            //ResetDevice();
            //InitialiseApp();
            //CloseDialog(true);
        }

        internal void CancelCount()
        {
            Log.Info(GetType().Name, nameof(CancelCount), "Device Management", nameof(CancelCount));
            DeviceTransactionEnd();
        }

        internal void ResetDevice()
        {
            Log.Info(GetType().Name, nameof(ResetDevice), "Device Management", nameof(ResetDevice));
            DeviceManager.ResetDevice(false);
        }

        internal void StartCIT(string sealNumber)
        {
            Log.InfoFormat(GetType().Name, nameof(StartCIT), "Device Management", "Seal Number = {0}, Bag Number = {1}", sealNumber, lastCIT.new_bag_number);
            CurrentApplicationState = ApplicationState.CIT_START;
            if (DebugNoDevice)
            {
                CITResult e = new CITResult
                {
                    level = ErrorLevel.SUCCESS,
                    extendedResult = "",
                    resultCode = 0,
                    data = new CITResultBody()
                    {
                        BagNumber = lastCIT?.new_bag_number,
                        Currency = "KES",
                        CurrencyCount = 1,
                        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        DeviceSerialNumber = "wfsdfsd",
                        Name = "Harrold",
                        TotalValue = 3000L,
                        TransactionCount = 3,
                        denomination = new Denomination()
                        {
                            denominationItems = new List<DenominationItem>()
            {
              new DenominationItem()
              {
                Currency = "KES",
                denominationValue = 1000,
                count = 3L,
                type = DenominationItemType.NOTE
              }
            }
                        }
                    }
                };
                DeviceManager_CITResultEvent(this, e);
            }
            else
                _deviceManager.StartCIT(sealNumber);
        }

        internal void EndCIT()
        {
            using (new DepositorDBContext())
            {
                Log.InfoFormat(GetType().Name, nameof(EndCIT), "Device Management", "Bag Number = {0}", lastCIT.new_bag_number);
                _deviceManager.EndCIT(lastCIT.new_bag_number);
            }
        }

        internal void LockDevice(bool lockedByDevice, ApplicationErrorConst error, string errorMessage)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Device device = ApplicationModel.GetDevice(DBContext);
                Log.WarningFormat(GetType().Name, nameof(LockDevice), "Device Lock", "LockedByDevice = {0}: Error: {1}>>{2}", lockedByDevice ? "TRUE" : (object)"FALSE", error.ToString(), errorMessage);
                device.enabled = false;
                Log.Debug(GetType().Name, nameof(LockDevice), "Device Lock", "AlertManager.SendAlert(new AlertDeviceLocked(errorMessage, device, DateTime.Now));");
                AlertManager.SendAlert(new AlertDeviceLocked(errorMessage, device, DateTime.Now));
                Log.Debug(GetType().Name, nameof(LockDevice), "Device Lock", "DBContext.DeviceLocks.Add(new DeviceLock");
                DBContext.DeviceLocks.Add(new DeviceLock()
                {
                    id = Guid.NewGuid(),
                    device_id = device.id,
                    locked = true,
                    locking_user = CurrentUser?.id,
                    lock_date = DateTime.Now,
                    locked_by_device = lockedByDevice,
                    web_locking_user =  null
                });
                SaveToDatabase(DBContext);
                SetCashSwiftDeviceState(CashSwiftDeviceState.DEVICE_LOCK);
            }
        }

        internal void UnLockDevice(bool lockedByDevice, string lockMessage = null)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Device device = ApplicationModel.GetDevice(DBContext);
                Log.Info(GetType().Name, nameof(UnLockDevice), "Device Lock", lockMessage);
                device.enabled = true;
                AlertManager.SendAlert(new AlertDeviceUnLocked(lockMessage, device, DateTime.Now));
                DBContext.DeviceLocks.Add(new DeviceLock()
                {
                    id = Guid.NewGuid(),
                    device_id = device.id,
                    locked = false,
                    locking_user = CurrentUser?.id,
                    lock_date = DateTime.Now,
                    locked_by_device = lockedByDevice,
                    web_locking_user =  null
                });
                SaveToDatabase(DBContext);
                if (!ApplicationStatus.CashSwiftDeviceState.HasFlag(CashSwiftDeviceState.DEVICE_LOCK))
                    return;
                UnSetCashSwiftDeviceState(CashSwiftDeviceState.DEVICE_LOCK);
            }
        }

        internal void LogoffUsers(bool forced = false)
        {
            try
            {
                using (DepositorDBContext DBContext = new DepositorDBContext())
                {
                    if (CurrentUser != null)
                    {
                        DeviceLogin deviceLogin = DBContext.DeviceLogins.Where(x => x.User == CurrentUser.id && x.Success ==  true).OrderByDescending(x => x.LoginDate).FirstOrDefault();
                        if (deviceLogin != null)
                        {
                            deviceLogin.LogoutDate = new DateTime?(DateTime.Now);
                            deviceLogin.ForcedLogout = new bool?(forced);
                        }
                    }
                    if (ValidatingUser != null)
                    {
                        DeviceLogin deviceLogin = DBContext.DeviceLogins.Where(x => x.User == ValidatingUser.id && x.Success ==  true).OrderByDescending(x => x.LoginDate).FirstOrDefault();
                        if (deviceLogin != null)
                        {
                            deviceLogin.LogoutDate = new DateTime?(DateTime.Now);
                            deviceLogin.ForcedLogout = new bool?(forced);
                        }
                    }
                    SaveToDatabase(DBContext);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                CurrentUser =  null;
                ValidatingUser =  null;
            }
        }

        internal void LockUser(
          ApplicationUser user,
          bool lockedByDevice,
          ApplicationErrorConst error,
          string errorMessage)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.WarningFormat(GetType().Name, nameof(LockUser), "User Locked by device", "Error {0} {1} {2}", error, error.ToString(), errorMessage);
                ApplicationUser applicationUser = DBContext.ApplicationUsers.First(x => x.id == user.id);
                applicationUser.depositor_enabled = new bool?(false);
                applicationUser.IsActive = new bool?(false);
                AlertManager.SendAlert(new AlertUserLocked(user, ApplicationModel.GetDevice(DBContext), errorMessage, DateTime.Now));
                DBContext.UserLocks.Add(new UserLock()
                {
                    id = Guid.NewGuid(),
                    ApplicationUserLoginDetail = user.ApplicationUserLoginDetail,
                    LockType = new int?(0),
                    InitiatingUser = CurrentUser?.id,
                    LogDate = new DateTime?(DateTime.Now),
                    WebPortalInitiated = new bool?(false)
                });
                SaveToDatabase(DBContext);
            }
        }

        internal void UnLockUser(ApplicationUser user, bool lockedByUser, string lockMessage = null)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Log.Info(GetType().Name, nameof(UnLockUser), "User Lock", lockMessage);
                ApplicationUser applicationUser = DBContext.ApplicationUsers.First(x => x.id == user.id);
                applicationUser.depositor_enabled = new bool?(true);
                applicationUser.IsActive = new bool?(true);
                AlertManager.SendAlert(new AlertUserUnLocked(user, ApplicationModel.GetDevice(DBContext), lockMessage, DateTime.Now));
                DBContext.UserLocks.Add(new UserLock()
                {
                    id = Guid.NewGuid(),
                    ApplicationUserLoginDetail = user.ApplicationUserLoginDetail,
                    LockType = new int?(1),
                    InitiatingUser = CurrentUser?.id,
                    LogDate = new DateTime?(DateTime.Now),
                    WebPortalInitiated = new bool?(false)
                });
                SaveToDatabase(DBContext);
            }
        }

        internal bool UserPermissionAllowed(
          ApplicationUser currentUser,
          string activityString,
          bool isAuthorising = false)
        {
            using (new DepositorDBContext())
            {
                try
                {
                    return GetUserPermission(currentUser, activityString, isAuthorising) != null;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        internal Permission GetUserPermission(
          ApplicationUser user,
          string activity,
          bool isAuthenticating = false)
        {
            if (user == null)
                return null;
            if (string.IsNullOrWhiteSpace(activity))
                return null;
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                Activity Activity = depositorDbContext.Activities.FirstOrDefault(x => x.name.Equals(activity, StringComparison.InvariantCultureIgnoreCase));
                if (Activity == null)
                    return null;
                return depositorDbContext.Permissions.FirstOrDefault(x => x.role_id == user.role_id && x.activity_id == Activity.id && (isAuthenticating ? x.standalone_can_Authenticate : x.standalone_allowed));
            }
        }

        internal void TimeoutSession(string screen, double timeout, string message = null)
        {
            Log.InfoFormat(GetType().Name, nameof(TimeoutSession), "Session", "Screen {0} has timed out after {1:0.###} seconds with message {2}", screen, timeout, message);
            AlertManager.SendAlert(new AlertTransactionTimedout(CurrentTransaction, ApplicationModel.GetDevice(new DepositorDBContext()), DateTime.Now));
            if (CurrentSession != null)
                CurrentSession.CountingEnded = true;
            EndTransaction(ApplicationErrorConst.WARN_DEPOSIT_TIMEOUT, "Timeout on screen " + screen);
            EndSession(false, 1014, ApplicationErrorConst.WARN_DEPOSIT_TIMEOUT, string.Format("Screen {0} has timed out after {1:0.###} miliseconds with message {2}", screen, timeout, message));
            ResetDevice();
        }

        internal void StartCountingProcess()
        {
            if (CurrentApplicationState != ApplicationState.COUNT_STARTED)
            {
                Log.Info(GetType().Name, nameof(StartCountingProcess), "Device Management", "BeginCount");
                CurrentApplicationState = ApplicationState.COUNT_STARTED;
                ReferencesAccepted();
                long transactionLimitCents = (long)((bool)CurrentTransaction?.TransactionType?.TransactionLimitList?.Get_prevent_overdeposit(CurrentTransaction.Currency) ? CurrentTransaction?.TransactionType?.TransactionLimitList?.Get_overdeposit_amount(CurrentTransaction.Currency) : long.MaxValue);
                AppTransaction currentTransaction = CurrentTransaction;
                long transactionValueCents = currentTransaction != null ? currentTransaction.TransactionValue : 0L;
                DeviceTransactionStart(transactionLimitCents, transactionValueCents);
            }
            else
                Log.Warning(GetType().Name, nameof(StartCountingProcess), "InvalidOperation", "Invalid state change requested");
        }

        public void PrintReceipt(Transaction transaction, bool reprint = false, DepositorDBContext txDBContext = null)
        {
            if (DebugDisablePrinter)
                return;
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                txDBContext = txDBContext ?? depositorDbContext;
                Log.InfoFormat(GetType().Name, nameof(PrintReceipt), "Commands", "Transaction code = {0}, Reprint = {1}", transaction.id, reprint);
                Printer.PrintTransaction(transaction, txDBContext, reprint);
            }
        }

        public void PrintCITReceipt(CIT cit, DepositorDBContext DBContext, bool reprint = false)
        {
            if (DebugDisablePrinter)
                return;
            Log.InfoFormat(GetType().Name, nameof(PrintCITReceipt), "Commands", "CIT code = {0}, Reprint = {1}", cit.id, reprint);
            Printer.PrintCIT(cit, DBContext, reprint);
        }

        public async Task<PostTransactionResponse> PostToCoreBankingAsync(
          Guid requestID,
          Transaction transaction)
        {
            ApplicationViewModel applicationViewModel = this;
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                int num1;
                if (applicationViewModel.DebugNoCoreBanking)
                {
                    DepositorLogger log = Log;
                    string name = applicationViewModel.GetType().Name;
                    object[] objArray = new object[4]
                    {
             requestID,
             transaction.tx_account_number,
             transaction.tx_currency,
            null
                    };
                    long? txAmount = transaction.tx_amount;
                    long num2 = 100;
                    objArray[3] =  txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num2) : new long?();
                    log.InfoFormat(name, "PostToCoreBanking", "Commands", "DebugPosting: RequestID = {0}, AccountNumber = {1}, Currency = {2}, Amount = {3:N2}", objArray);
                    PostTransactionResponse coreBankingAsync = new PostTransactionResponse
                    {
                        MessageID = Guid.NewGuid().ToString().ToUpper(),
                        RequestID = Guid.NewGuid().ToString().ToUpper(),
                        PostResponseCode = 200.ToString() ?? "",
                        PostResponseMessage = "Posted",
                        MessageDateTime = DateTime.Now,
                        IsSuccess = true,
                        TransactionDateTime = DateTime.Now,
                        TransactionID = Guid.NewGuid().ToString().ToUpper()
                    };
                    return coreBankingAsync;
                }
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "Posting to live core banking", "Integation", "posting transaction {0}", transaction.ToString());
                    TransactionTypeListItem transactionTypeListItem = DBContext.TransactionTypeListItems.FirstOrDefault(x => x.id == transaction.tx_type.Value);
                    ApplicationModel applicationModel = applicationViewModel.ApplicationModel;
                    string str1;
                    if (applicationModel == null)
                    {
                        str1 =  null;
                    }
                    else
                    {
                        ICollection<DeviceSuspenseAccount> suspenseAccounts = applicationModel.GetDevice(DBContext).DeviceSuspenseAccounts;
                        str1 = suspenseAccounts != null ? suspenseAccounts.FirstOrDefault(x => x.enabled && string.Equals(x.currency_code, CurrentTransaction?.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))?.account_number : null;
                    }
                    string str2 = str1;
                    DepositorLogger log = Log;
                    string name = applicationViewModel.GetType().Name;
                    object[] objArray = new object[5]
                    {
             requestID,
             transaction.tx_account_number,
             transaction.tx_currency,
            null,
            null
                    };
                    long? txAmount = transaction.tx_amount;
                    long num3 = 100;
                    objArray[3] =  txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num3) : new long?();
                    objArray[4] =  str2;
                    log.InfoFormat(name, "PostToCoreBanking", "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", objArray);
                    Device device = applicationViewModel.CurrentSession.Device;
                    IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.app_id, device.app_key, null);
                    PostTransactionRequest request = new PostTransactionRequest
                    {
                        SystemCode_Cred = DeviceConfiguration.SYSTEM_CODE,
                        BankID = DeviceConfiguration.BANK_ID,
                        SystemCode_FT = DeviceConfiguration.SYSTEM_CODE,
                        AppID = device.app_id,
                        AppName = device.machine_name,
                        MessageDateTime = DateTime.Now,
                        MessageID = Guid.NewGuid().ToString(),
                        Language = applicationViewModel.CurrentLanguage,
                        DeviceID = device.id,
                        SessionID = applicationViewModel.SessionID.Value.ToString(),
                        FundsSource = transaction.funds_source,
                        RefAccountName = transaction.cb_ref_account_name,
                        RefAccountNumber = transaction.tx_ref_account,
                        DeviceReferenceNumber = string.Format("{0:#}", transaction.tx_random_number),
                        DepositorIDNumber = transaction.tx_id_number,
                        DepositorName = transaction.tx_depositor_name,
                        DepositorPhone = transaction.tx_phone,
                        TransactionType = transactionTypeListItem?.cb_tx_type,
                        TransactionTypeID = transactionTypeListItem.id,
                        Transaction = new CashSwift.API.Messaging.Integration.Transactions.PostTransactionData
                        {
                            TransactionID = transaction.id,
                            DebitAccount = new CashSwift.API.Messaging.Integration.Transactions.PostBankAccount()
                            {
                                AccountNumber = transaction.tx_suspense_account,
                                Currency = transaction.tx_currency.ToUpper()
                            },
                            CreditAccount = new CashSwift.API.Messaging.Integration.Transactions.PostBankAccount()
                            {
                                AccountName = transaction.cb_account_name,
                                AccountNumber = transaction.tx_account_number,
                                Currency = transaction.tx_currency.ToUpper()
                            },
                            Amount =  transaction.tx_amount.Value / 100.0M,
                            DateTime = transaction.tx_end_date.Value,
                            DeviceID = transaction.Device.id,
                            DeviceNumber = transaction.Device.device_number,
                            Narration = transaction.tx_narration
                        }
                    };

                    PostTransactionResponse coreBankingAsync = await (integrationServiceClient.PostTransactionAsync(request));
                    applicationViewModel.CheckIntegrationResponseMessageDateTime(coreBankingAsync.MessageDateTime);
                    return coreBankingAsync;
                }
                catch (Exception ex)
                {
                    string ErrorDetail = string.Format("Post failed with error: {0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                    Log.Error(applicationViewModel.GetType().Name, 91, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString(), ErrorDetail);
                    PostTransactionResponse coreBankingAsync = new PostTransactionResponse
                    {
                        MessageDateTime = DateTime.Now,
                        PostResponseCode = "-1",
                        PostResponseMessage = ErrorDetail,
                        RequestID = requestID.ToString().ToUpperInvariant(),
                        ServerErrorCode = "-1",
                        ServerErrorMessage = ErrorDetail,
                        IsSuccess = false
                    };
                    return coreBankingAsync;
                }
            }
        }
        public async Task<FundsTransferResponseDto> PostCITTransactionToFinacleCoreBankingAsync(Guid requestID, CITTransaction CITTransaction)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                if (DebugNoCoreBanking)
                {
                    Log.InfoFormat(GetType().Name, "PostCITTransactionToFinacleCoreBanking", "Commands", "DebugPosting: RequestID = {0}, AccountNumber = {1}, Currency = {2}, Amount = {3:N2}", requestID, CITTransaction.account_number, CITTransaction.currency, CITTransaction.amount / 100L);
                    FundsTransferResponseDto coreBankingAsync = new FundsTransferResponseDto
                    {
                        MessageID = Guid.NewGuid().ToString().ToUpper(),
                        RequestID = Guid.NewGuid().ToString().ToUpper(),
                        PostResponseCode = 200.ToString() ?? "",
                        PostResponseMessage = "Posted",
                        MessageDateTime = DateTime.Now,
                        IsSuccess = true
                    };
                    return coreBankingAsync;
                }


                Device device = GetDevice(DBContext);
                try
                {
                    Log.InfoFormat(GetType().Name, "Posting to live core banking", "Integation", "posting CITTransaction {0}", CITTransaction.ToString());
                    Log.InfoFormat(GetType().Name, "PostCITTransactionToFinacleCoreBanking", "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", requestID, CITTransaction.account_number, CITTransaction.currency, CITTransaction.amount / 100L, CITTransaction.suspense_account);
                    //IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.app_id, device.app_key, null);
                    var integrationServiceClient = new FinacleIntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, null);
                    Guid guid = CITTransaction.id;
                    PostCITTransactionRequest request = new PostCITTransactionRequest
                    {
                        SessionID = CITTransaction.id.ToString(),
                        AppID = device.app_id,
                        AppName = device.machine_name,
                        MessageDateTime = DateTime.Now,
                        MessageID = Guid.NewGuid().ToString(),
                        Language = CurrentLanguage,
                        DeviceID = device.id,
                        Transaction = new CashSwift.API.Messaging.Integration.Transactions.PostTransactionData()
                        {
                            TransactionID = CITTransaction.id,
                            Amount = CITTransaction.amount / 100.0M,
                            DateTime = CITTransaction.datetime,
                            DeviceID = device.id,
                            DeviceNumber = device.device_number,
                            Narration = CITTransaction.narration,
                            DebitAccount = new CashSwift.API.Messaging.Integration.Transactions.PostBankAccount()
                            {
                                AccountNumber = CITTransaction.account_number,
                                Currency = CITTransaction.currency.ToUpper()
                            },
                            CreditAccount = new CashSwift.API.Messaging.Integration.Transactions.PostBankAccount()
                            {
                                AccountNumber = CITTransaction.suspense_account,
                                Currency = CITTransaction.currency.ToUpper()
                            }
                        }
                    };
                    FundsTransferRequestDto fundsTransferRequest = new FundsTransferRequestDto
                    {
                        SystemCode_Cred = DeviceConfiguration.SYSTEM_CODE,
                        BankID = DeviceConfiguration.BANK_ID,
                        SystemCode_FT = DeviceConfiguration.SYSTEM_CODE,
                        TransactionType = "T",
                        TransactionSubType = "BI",
                        CDM_Number=device.machine_name,
                        TransactionReference_Dr = CITTransaction.id.ToString(),
                        TransactionItemKey_Dr = $"{CITTransaction.id}_1",
                        AccountNumber_Dr = CITTransaction.account_number,
                        TransactionAmount_Dr = CITTransaction.amount / 100.0M,
                        TransactionCurrency_Dr = CITTransaction.currency.ToUpper(),
                        Narrative_Dr = CITTransaction.narration,
                        TransactionReference_Cr = CITTransaction.id.ToString(),
                        TransactionItemKey_Cr = $"{CITTransaction.id}_2",
                        AccountNumber_Cr = CITTransaction.suspense_account,
                        TransactionAmount_Cr = CITTransaction.amount / 100.0M,
                        TransactionCurrency_Cr = CITTransaction.currency.ToUpper(),
                        Narrative_Cr = CITTransaction.narration,
                        AppID = device.app_id,
                        AppName = device.machine_name,
                        DeviceID = device.id,
                        SessionID = SessionID.Value.ToString()
                    };
                    var response = await integrationServiceClient.FundsTransferAsync(fundsTransferRequest);
                    CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                    return response;
                }
                catch (Exception ex)
                {
                    string errorText = string.Format("Post failed with error: {0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                    Log.Error(GetType().Name, 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), errorText);
                    FundsTransferResponseDto coreBankingAsync = new FundsTransferResponseDto();
                    Guid guid = SessionID.Value;
                    coreBankingAsync.SessionID = guid.ToString();
                    coreBankingAsync.AppID = device.app_id;
                    coreBankingAsync.AppName = device.machine_name;
                    coreBankingAsync.MessageDateTime = DateTime.Now;
                    guid = Guid.NewGuid();
                    coreBankingAsync.MessageID = guid.ToString();
                    coreBankingAsync.PostResponseCode = "-1";
                    coreBankingAsync.PostResponseMessage = errorText;
                    coreBankingAsync.RequestID = requestID.ToString().ToUpperInvariant();
                    coreBankingAsync.ServerErrorCode = "-1";
                    coreBankingAsync.ServerErrorMessage = errorText;
                    coreBankingAsync.IsSuccess = false;
                    coreBankingAsync.PublicErrorCode = "500";
                    coreBankingAsync.PublicErrorMessage = "System error. Contact administrator";
                    return coreBankingAsync;
                }
            }
        }

        internal void SetLanguage(string language = null)
        {
            if (language == null)
                language = CurrentSession?.Language;
            try
            {
                if (string.IsNullOrEmpty(language))
                    return;
                IEnumerable<ResourceDictionary> source1 = Application.Current.Resources.MergedDictionaries.Where(x => x.Source != null && x.Source.OriginalString.Contains("Lang_")).ToList();
                List<ResourceDictionary> resourceDictionaryList;
                if (source1 == null)
                {
                    resourceDictionaryList =  null;
                }
                else
                {
                    IEnumerable<ResourceDictionary> source2 = source1.Where(y =>
                    {
                        if (y == null)
                            return false;
                        Uri source3 = y.Source;
                        int? nullable1;
                        if ((object)source3 == null)
                        {
                            nullable1 = new int?();
                        }
                        else
                        {
                            string originalString = source3.OriginalString;
                            if (originalString == null)
                            {
                                nullable1 = new int?();
                            }
                            else
                            {
                                IEnumerable<char> source4 = originalString.Where(z => z == '.');
                                nullable1 = source4 != null ? new int?(source4.Count()) : new int?();
                            }
                        }
                        int? nullable2 = nullable1;
                        int num = 1;
                        return nullable2.GetValueOrDefault() == num & nullable2.HasValue;
                    });
                    resourceDictionaryList = source2 != null ? source2.ToList() : null;
                }
                List<ResourceDictionary> source5 = resourceDictionaryList;
                foreach (ResourceDictionary resourceDictionary1 in source5)
                {
                    string requestedCulture = string.Format("{0}.{1}.xaml", resourceDictionary1?.Source?.OriginalString.Replace(".xaml", ""), language);
                    ResourceDictionary resourceDictionary2 = Application.Current.Resources.MergedDictionaries.FirstOrDefault(d => string.Equals(d?.Source?.OriginalString, requestedCulture, StringComparison.InvariantCultureIgnoreCase));
                    if (resourceDictionary2 == null)
                    {
                        requestedCulture = resourceDictionary1?.Source?.OriginalString;
                        resourceDictionary2 = source5.FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
                    }
                    if (resourceDictionary2 != null)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary2);
                        Application.Current.Resources.MergedDictionaries.Add(resourceDictionary2);
                    }
                }
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                CurrentLanguage = language;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal void SetLanguage(Language value)
        {
            if (CurrentSession == null)
                return;
            CurrentSession.Language = value.code;
            SetLanguage(value.code);
        }

        internal static string GetDeviceNumber()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
                return GetDevice(DBContext)?.device_number;
        }

        internal static Device GetDevice(DepositorDBContext DBContext)
        {
            try
            {
                Device device = DBContext.Devices.FirstOrDefault(x => x.machine_name==Environment.MachineName);
                if (device != null)
                    return device;
                Log.Fatal(nameof(ApplicationViewModel), 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not get device info from database, terminating");
                throw new InvalidOperationException(string.Format("Device with machine name = {0} does not exists in the local database.", Environment.MachineName));
            }
            catch (Exception ex)
            {
                Log.FatalFormat(nameof(ApplicationViewModel), 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "{0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                throw;
            }
        }

        public static void SaveToDatabase(DepositorDBContext DBContext)
        {
            try
            {
                DBContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string ErrorDetail = string.Format("{0}>>{1}>>{2}>stack>{3}>Validation Errors: ", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message, ex.StackTrace);
                foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                {
                    ErrorDetail += ">validation error>";
                    foreach (DbValidationError validationError in (IEnumerable<DbValidationError>)entityValidationError.ValidationErrors)
                        ErrorDetail = ErrorDetail + ">Property=>" + validationError.PropertyName + ", ErrorMessage=>" + validationError.ErrorMessage;
                }
                Console.WriteLine(ErrorDetail);
                Log.Error("ApplicationViewModel.SaveToDatabase()", 3, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString("G"), ErrorDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
                Log.ErrorFormat(nameof(ApplicationViewModel), 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Error Saving to Database: {0}", string.Format("{0}>>{1}>>{2}>>{3}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message, ex.InnerException?.InnerException?.InnerException?.Message));
            }
        }

        internal bool InSessionAndTransaction(string callingCode, bool enforceDeviceState = true)
        {
            if (CurrentSession != null)
            {
                if (CurrentTransaction != null)
                    return true;
                if (enforceDeviceState && DeviceManager.CurrentState != 0)
                {
                    Log.WarningFormat(GetType().Name, nameof(InSessionAndTransaction), "InvalidOperation", "Not in Transaction at {0}. DeviceManager.CurrentState = {1}", callingCode, DeviceManager.CurrentState);
                    DeviceManager.ResetDevice();
                }
                return false;
            }
            if (DeviceManager.CurrentState != 0)
            {
                Log.WarningFormat(GetType().Name, nameof(InSessionAndTransaction), "InvalidOperation", "Not in Session at {0}. DeviceManager.CurrentState = {1}", callingCode, DeviceManager.CurrentState);
                DeviceManager.ResetDevice();
            }
            return false;
        }

        internal void ShowController()
        {
            Log.InfoFormat(GetType().Name + ".ShowController()", "Show Controller", "Diagnostics", "Show controller command has been issued by user {0}", CurrentUser?.username);
            DeviceManager.ShowDeviceController();
        }

        internal void ShutdownPC(ShutdownCommand command, string reason, uint time = 0)
        {
            time = time.Clamp(0U, 600U);
            string str = "";
            switch (command)
            {
                case ShutdownCommand.SHUTDOWN:
                    str = "-s";
                    break;
                case ShutdownCommand.RESTART:
                    str = "-r";
                    break;
                case ShutdownCommand.LOGOFF:
                    str = "-l";
                    break;
            }
            string arguments = string.Format("{0} -f -t {1}  -d p:0:0 -c \"{2}\"", str, time, reason.Substring(0, reason.Length.Clamp(0, 512)));
            Log.InfoFormat(GetType().Name + ".ShutdownPC()", "Running shutdown command", "Diagnostics", "shutdown.exe {0}", arguments);
            Process.Start("shutdown.exe", arguments);
        }

        private void CheckIntegrationResponseMessageDateTime(DateTime MessageDateTime)
        {
            DateTime dateTime1 = DateTime.Now.AddSeconds(DeviceConfiguration.MESSAGEKEEPALIVETIME);
            DateTime dateTime2 = DateTime.Now.AddSeconds(-DeviceConfiguration.MESSAGEKEEPALIVETIME);
            if (!(MessageDateTime < dateTime1) || !(MessageDateTime > dateTime2))
                throw new Exception(string.Format("Invalid MessageDateTime: value {0:yyyy-MM-dd HH:mm:ss.fff} is NOT between {1:yyyy-MM-dd HH:mm:ss.fff} and {2:yyyy-MM-dd HH:mm:ss.fff}", MessageDateTime, dateTime2, dateTime1));
        }
    }
}
