using CashAccSysDeviceManager;
using CashSwift.Library.Standard.Statuses;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("D0E649AB-3379-4BED-8A93-9FDF29FF6AA0")]
    public class CountScreenViewModel : DepositorCustomerScreenBaseViewModel
    {
        private CountScreenState currentCountScreenState;
        private bool sTATE_CASHIN_START_IsVisible;
        private bool sTATE_COUNTING_IsVisible;
        private bool sTATE_CASHIN_PAUSE_IsVisible;
        private bool sTATE_ESCROW_REJECT_IsVisible;
        private bool sTATE_ESCROW_ACCEPTED_IsVisible;
        private string StateInstESCROW_ACCEPTED = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("CountScreenViewModel.CurrentCountScreenState", "sys_CountState_Instructions_ACCEPTED", "1. Click DEPOSIT for counted notes to be received.\r\n2. Press ADD MORE MONEY to continue counting.");
        private string _DropConfirmationText;
        private string _DropConfirmationTitle;
        private string _CountBeginButtonCaption;
        private string _PauseCountButton_Caption;
        private string _CountButton_Caption;
        private string _EscrowDropButton_Caption;
        private string _EscrowRejectButton_Caption;
        private string _CountScreenTotal_Caption;
        private bool _lastCanNext;
        private bool _lastCanCancel;
        private bool _lastCanCount;
        private bool _lastCanEscrowDrop;
        private bool _lastCanEscrowReject;
        private string screenStateInstructions;
        private string counTableDenominationCaption;
        private string counTableCountCaption;
        private string counTableSubTotalCaption;
        private string countTableTransactionSummaryCaption;
        private string countTableTotalAmountCaption;
        private bool sTATE_INIT_IsVisible;
        private string instructionTitleCaption;

        public bool DoNotShowDropConfirmationCheckbox { get; set; }

        public object TimeoutLock { get; set; } = new object();

        private bool TimeoutMode { get; set; }
        private string image { get; set; }

        private bool TimeoutModeComplete { get; set; }
        public List<DenominationItem> _denominationItems;
        public List<DenominationItem> TransactionDenominationItems
        {
            get => _denominationItems;
            set
            {
                _denominationItems = value;
                NotifyOfPropertyChange(nameof(TransactionDenominationItems));
            }
        }
        public bool TransactionSummaryIsVisible
        {
            get
            {
                TransactionDenominationItems = CurrentTransaction?.CountedDenomination?.denominationItems ?? CurrentTransaction?.TotalDenomination?.denominationItems;
                return !StateInitIsVisible && TransactionDenominationItems?.Count > 0;
            }
        }

        public string ScreenStateInstructions
        {
            get => screenStateInstructions;
            set
            {
                screenStateInstructions = value;
                NotifyOfPropertyChange(nameof(ScreenStateInstructions));
            }
        }

        public bool StateInitIsVisible
        {
            get => sTATE_INIT_IsVisible;
            set
            {
                sTATE_INIT_IsVisible = value;
                NotifyOfPropertyChange(() => StateInitIsVisible);
                NotifyOfPropertyChange(() => TransactionSummaryIsVisible);
            }
        }

        public bool STATE_CASHIN_START_IsVisible
        {
            get => sTATE_CASHIN_START_IsVisible;
            set
            {
                sTATE_CASHIN_START_IsVisible = value;
                NotifyOfPropertyChange(nameof(STATE_CASHIN_START_IsVisible));
            }
        }

        public bool STATE_COUNTING_IsVisible
        {
            get => sTATE_COUNTING_IsVisible;
            set
            {
                sTATE_COUNTING_IsVisible = value;
                NotifyOfPropertyChange(nameof(STATE_COUNTING_IsVisible));
            }
        }

        public bool STATE_CASHIN_PAUSE_IsVisible
        {
            get => sTATE_CASHIN_PAUSE_IsVisible;
            set
            {
                sTATE_CASHIN_PAUSE_IsVisible = value;
                NotifyOfPropertyChange(nameof(STATE_CASHIN_PAUSE_IsVisible));
            }
        }

        public bool STATE_ESCROW_REJECT_IsVisible
        {
            get => sTATE_ESCROW_REJECT_IsVisible;
            set
            {
                sTATE_ESCROW_REJECT_IsVisible = value;
                NotifyOfPropertyChange(nameof(STATE_ESCROW_REJECT_IsVisible));
            }
        }

        public bool STATE_ESCROW_ACCEPTED_IsVisible
        {
            get => sTATE_ESCROW_ACCEPTED_IsVisible;
            set
            {
                sTATE_ESCROW_ACCEPTED_IsVisible = value;
                NotifyOfPropertyChange(nameof(STATE_ESCROW_ACCEPTED_IsVisible));
            }
        }

        private void ApplicationViewModel_DeviceStatusChangedEvent(
          object sender,
          DeviceStatusChangedEventArgs e)
        {
        }

        private void ApplicationViewModel_TransactionStatusEvent(
          object sender,
          DeviceTransactionResult e)
        {
        }

        private CountScreenState CurrentCountScreenState
        {
            get => currentCountScreenState;
            set
            {
                if (currentCountScreenState == value)
                    return;
                currentCountScreenState = value;
                NotifyOfPropertyChange(() => CurrentCountScreenState);
                StateInitIsVisible = false;
                STATE_CASHIN_START_IsVisible = false;
                STATE_COUNTING_IsVisible = false;
                STATE_CASHIN_PAUSE_IsVisible = false;
                STATE_ESCROW_REJECT_IsVisible = false;
                STATE_ESCROW_ACCEPTED_IsVisible = false;
                switch (value)
                {
                    case CountScreenState.NONE:
                    case CountScreenState.INIT:
                        StateInitIsVisible = true;
                        if (!HasStartedCounting)
                        {
                            ScreenStateInstructions = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Instructions_CASHIN", "Place cash on the counter and then\r\npress START COUNT");
                            ScreenTitle = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Title_CASHIN", "CASHIN");
                            break;
                        }
                        break;
                    case CountScreenState.IDLE:
                    case CountScreenState.CASHIN_START:
                        STATE_CASHIN_START_IsVisible = true;
                        break;
                    case CountScreenState.COUNTING:
                        STATE_COUNTING_IsVisible = true;
                        ScreenStateInstructions = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Instructions_COUNTING", "1. Press COUNT PAUSE when the notes have finished counting to proceed.");
                        ScreenTitle = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Title_COUNTING", "COUNTING");
                        break;
                    case CountScreenState.CASHIN_PAUSE:
                        STATE_CASHIN_PAUSE_IsVisible = true;
                        ScreenStateInstructions = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Instructions_PAUSE", "1. Click ACCEPT for all the counted notes to be accepted.\r\nThis operation cannot be reversed.\r\n2. Click REJECT to take back the counted notes");
                        ScreenTitle = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Title_PAUSE", "PAUSE");
                        break;
                    case CountScreenState.ESCROW_REJECTED:
                        STATE_ESCROW_REJECT_IsVisible = true;
                        ScreenStateInstructions = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Instructions_REJECTED", "1. Please remove the counted notes from the\r\nopening on the counter.\r\n2. Press ADD MORE MONEY to start count again.\r\n3. Press CANCEL to close.");
                        ScreenTitle = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Title_REJECTED", "REJECTED");
                        break;
                    case CountScreenState.ESCROW_ACCEPTED:
                        STATE_ESCROW_ACCEPTED_IsVisible = true;
                        ScreenStateInstructions = StateInstESCROW_ACCEPTED;
                        ScreenTitle = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(string.Format("{0}.{1}.{2}", nameof(CountScreenViewModel), nameof(CurrentCountScreenState), value), "sys_CountState_Title_ACCEPTED", "ACCEPTED");
                        CountBeginButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountBeginButtonCaption", "sys_CountBeginButtonCaption", "Add More Money");
                        break;
                }
            }
        }

        public AppTransaction CurrentTransaction { get; set; }

        public bool AutoDropChecked { get; set; }

        public bool CanAutoDropChecked
        {
            get
            {
                DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
                return deviceConfiguration == null || deviceConfiguration.AUTODROP_CHANGE_ALLOWED;
            }
        }

        public bool AutoCountChecked { get; set; }

        public bool CanAutoCountChecked
        {
            get
            {
                DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
                return deviceConfiguration == null || deviceConfiguration.AUTOCOUNT_CHANGE_ALLOWED;
            }
        }

        public bool DropConfirmationExpanderIsVisible => CanEscrowDrop || CanEscrowReject;

        public string InstructionTitleCaption
        {
            get => instructionTitleCaption;
            set
            {
                instructionTitleCaption = value;
                NotifyOfPropertyChange(nameof(InstructionTitleCaption));
            }
        }

        public string CountTableTotalAmountCaption
        {
            get => countTableTotalAmountCaption;
            set
            {
                countTableTotalAmountCaption = value;
                NotifyOfPropertyChange(nameof(CountTableTotalAmountCaption));
            }
        }

        public string CountTableTransactionSummaryCaption
        {
            get => countTableTransactionSummaryCaption;
            set
            {
                countTableTransactionSummaryCaption = value;
                NotifyOfPropertyChange(nameof(CountTableTransactionSummaryCaption));
            }
        }

        public string CountTableDenominationCaption
        {
            get => counTableDenominationCaption;
            set
            {
                counTableDenominationCaption = value;
                NotifyOfPropertyChange(nameof(CountTableDenominationCaption));
            }
        }

        public string CountTableCountCaption
        {
            get => counTableCountCaption;
            set
            {
                counTableCountCaption = value;
                NotifyOfPropertyChange(nameof(CountTableCountCaption));
            }
        }

        public string CountTableSubTotalCaption
        {
            get => counTableSubTotalCaption;
            set
            {
                counTableSubTotalCaption = value;
                NotifyOfPropertyChange(nameof(CountTableSubTotalCaption));
            }
        }

        public string InlineDropConfirmationText => DropConfirmationText;

        public string DropConfirmationText
        {
            get => _DropConfirmationText;
            set
            {
                _DropConfirmationText = value;
                NotifyOfPropertyChange(() => DropConfirmationText);
                NotifyOfPropertyChange(() => InlineDropConfirmationText);
            }
        }

        public string DropConfirmationTitle
        {
            get => _DropConfirmationTitle;
            set
            {
                _DropConfirmationTitle = value;
                NotifyOfPropertyChange(() => DropConfirmationTitle);
            }
        }

        public string CountBeginButtonCaption
        {
            get => _CountBeginButtonCaption;
            set
            {
                _CountBeginButtonCaption = value;
                NotifyOfPropertyChange(() => CountBeginButtonCaption);
            }
        }

        public string PauseCountButtonCaption
        {
            get => _PauseCountButton_Caption;
            set
            {
                _PauseCountButton_Caption = value;
                NotifyOfPropertyChange(() => PauseCountButtonCaption);
            }
        }

        public string CountButtonCaption
        {
            get => _CountButton_Caption;
            set
            {
                _CountButton_Caption = value;
                NotifyOfPropertyChange(() => CountButtonCaption);
            }
        }

        public string EscrowDropButtonCaption
        {
            get => _EscrowDropButton_Caption;
            set
            {
                _EscrowDropButton_Caption = value;
                NotifyOfPropertyChange(() => EscrowDropButtonCaption);
            }
        }

        public string EscrowRejectButtonCaption
        {
            get => _EscrowRejectButton_Caption;
            set
            {
                _EscrowRejectButton_Caption = value;
                NotifyOfPropertyChange(() => EscrowRejectButtonCaption);
            }
        }

        public string CountScreenTotalCaption
        {
            get => _CountScreenTotal_Caption;
            set
            {
                _CountScreenTotal_Caption = value;
                NotifyOfPropertyChange(() => CountScreenTotalCaption);
            }
        }
        public string DisplayedImage
        {
            get { return @"{AppDir}/Resources/CountState_Inst_INIT.png"; }
        }
        public CountScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            //StateInitIsVisible = true;
            //NotifyOfPropertyChange(() => StateInitIsVisible);

            //CurrentCountScreenState = CountScreenState.INIT;
            CurrentTransaction = applicationViewModel.CurrentTransaction;
            ApplicationViewModel.CashInStartedEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CashInStartedEvent);
            ApplicationViewModel.CountStartedEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountStartedEvent);
            ApplicationViewModel.CountPauseEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountPauseEvent);
            ApplicationViewModel.EscrowDropEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_EscrowDropEvent);
            ApplicationViewModel.EscrowRejectEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_EscrowRejectEvent);
            ApplicationViewModel.EscrowOperationCompleteEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_EscrowOperationCompleteEvent);
            ApplicationViewModel.CountEndEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountEndEvent);
            ApplicationViewModel.TransactionStatusEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_TransactionStatusEvent);
            ApplicationViewModel.NotifyCurrentTransactionStatusChangedEvent += new EventHandler<EventArgs>(ApplicationViewModel_NotifyCurrentTransactionStatusChanged);
            ApplicationViewModel.DeviceManager.PropertyChanged += new PropertyChangedEventHandler(DeviceManager_PropertyChanged);
            if (ApplicationViewModel.DeviceManager != null)
                ApplicationViewModel.DeviceManager.CashAccSysSerialFix.PropertyChanged += new PropertyChangedEventHandler(CashAccSysSerialFix_PropertyChanged);
            DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
            AutoDropChecked = deviceConfiguration != null && deviceConfiguration.AUTODROP_CHECKED;
            AutoCountChecked = deviceConfiguration != null && deviceConfiguration.AUTOCOUNT_CHECKED;
            InstructionTitleCaption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("CustomerAllReferenceScreenViewModel", "sys_ScreenInstructionsLabelCaption", "Instructions");
            InitialiseScreen();
        }

        private void ApplicationViewModel_EscrowOperationCompleteEvent(
          object sender,
          DeviceTransactionResult e)
        {
            CurrentCountScreenState = CountScreenState.CASHIN_START;

            NotifyOfPropertyChange(() => TransactionSummaryIsVisible);
        }

        private void ApplicationViewModel_EscrowRejectEvent(object sender, DeviceTransactionResult e) => CurrentCountScreenState = CountScreenState.ESCROW_REJECTED;

        private void ApplicationViewModel_EscrowDropEvent(object sender, DeviceTransactionResult e) => CurrentCountScreenState = CountScreenState.ESCROW_ACCEPTED;

        private void ApplicationViewModel_CountStartedEvent(object sender, DeviceTransactionResult e)
        {
            if (ApplicationViewModel.DeviceManager != null)
                CurrentCountScreenState = CountScreenState.CASHIN_PAUSE;
            else
                CurrentCountScreenState = CountScreenState.COUNTING;
        }

        private void ApplicationViewModel_CashInStartedEvent(object sender, DeviceTransactionResult e) => CurrentCountScreenState = CountScreenState.CASHIN_START;

        private void CashAccSysSerialFix_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine($"PropertyChangedEventArgs {e.PropertyName}");
        }

        private void HandleTimeoutOperation()
        {
            if (!TimeoutMode || TimeoutModeComplete || ApplicationViewModel.DeviceManager == null)
                return;
            if (ApplicationViewModel.DeviceManager.CashAccSysSerialFix.DE50Operation == DE50OperatingState.Storing_start_request)
            {
                ApplicationViewModel.Log.Info(nameof(CountScreenViewModel), "Processing", "DoCancelTransactionOnTimeout", "cash in the escrow detected. Dropping notes...");
                EscrowDrop();
            }
            else if (ApplicationViewModel.DeviceManager.ControllerState == ControllerState.DROP)
                EscrowReject();
            else if (ApplicationViewModel.CanTransactionEnd && ApplicationViewModel.DeviceManager.ControllerState == ControllerState.IDLE && (ApplicationViewModel.DeviceManager.CashAccSysSerialFix.DE50Operation == DE50OperatingState.Waiting || ApplicationViewModel.DeviceManager.CashAccSysSerialFix.DE50Operation == DE50OperatingState.Counting_start_request))
            {
                ApplicationViewModel.Log.Info(nameof(CountScreenViewModel), "Processing", "DoCancelTransactionOnTimeout", "no cash in escrow, end the transaction");
                base.DoCancelTransactionOnTimeout();
                TimeoutModeComplete = true;
            }
        }

        private void DeviceManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ApplicationViewModel.DeviceManager == null || !e.PropertyName.Equals("ClearHopperRequest"))
                return;
            string str = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("CountScreenViewModel.DeviceManager_PropertyChanged", "sys_ClearHopperRequest_Caption", "Kindly remove notes from hopper.", ApplicationViewModel.CurrentLanguage);
            if (ApplicationViewModel.DeviceManager.ClearHopperRequest)
            {
                string errorText = ErrorText;
                ErrorText = ((errorText != null ? (errorText.Length > 0 ? 1 : 0) : 0) != 0 ? "\r\n" : "") + str;
                ApplicationViewModel.Log.Warning(nameof(CountScreenViewModel), "Notes in hopper", "Clear Notes", str);
            }
            else
                ErrorText = ErrorText.Replace(str, "");
        }

        private void InitialiseScreen()
        {
            CountBeginButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountBeginButtonCaption", "sys_CountBeginButtonCaption", "Start Count");
            DropConfirmationTitle = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("DropConfirmationTitle", "sys_DropConfirmation_Title", "Drop Disclaimer");
            PauseCountButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("PauseCountButtonCaption", "sys_PauseCountButton_Caption", "Count Complete");
            EscrowDropButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("EscrowDropButtonCaption", "sys_EscrowDropButton_Caption", "Drop");
            EscrowRejectButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("EscrowRejectButtonCaption", "sys_EscrowRejectButton_Caption", "Reject");
            CountButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountButtonCaption", "sys_CountButton_Caption", "Count");
            CountScreenTotalCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountScreenTotalCaption", "sys_CountScreenTotal_Caption", "Total");
            DropConfirmationText = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("DropConfirmationText", "sys_DropConfirmation_Text", "By pressing " + EscrowDropButtonCaption + " you are accepting that the count displayed is correct else press " + EscrowRejectButtonCaption);
            CountTableTotalAmountCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountTableTotalAmountCaption", "sys_CountTable_TotalAmountCaption", "Total Amount");
            CountTableTransactionSummaryCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountTableDenominationCaption", "sys_CountTable_TransactionSummaryCaption", "Transaction Summary");
            CountTableDenominationCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountTableDenominationCaption", "sys_CountTable_Denomination_Caption", "Denomination");
            CountTableCountCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountTableCountCaption", "sys_CountTable_Count_Caption", "Count");
            CountTableSubTotalCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText("CountTableSubTotalCaption", "sys_CountTable_SubTotal_Caption", "Amount");
        }

        private void ApplicationViewModel_NotifyCurrentTransactionStatusChanged(
          object sender,
          EventArgs e)
        {
            NotifyCurrentTransactionStatusChanged();
            HandleCurrentScreenStateTransition();
        }

        private void HandleCurrentScreenStateTransition()
        {
        }

        private void NotifyCurrentTransactionStatusChanged()
        {
            int num = (int)IsCountWithinTheLimits();
            NotifyOfPropertyChange(() => NextCaption);
            NotifyOfPropertyChange(() => CancelCaption);
            NotifyOfPropertyChange(() => CurrentTransaction);
            NotifyOfPropertyChange(() => CanBeginCount);
            NotifyOfPropertyChange(() => CanCount);
            NotifyOfPropertyChange(() => CanPauseCount);
            NotifyOfPropertyChange(() => CanEscrowDrop);
            NotifyOfPropertyChange(() => CanEscrowReject);
            NotifyOfPropertyChange(() => CanNext);
            NotifyOfPropertyChange(() => CanCancel);
            NotifyOfPropertyChange(() => DropDisclaimerIsVisible);
            NotifyOfPropertyChange(() => DropConfirmationExpanderIsVisible);
            NotifyOfPropertyChange(() => TransactionSummaryIsVisible);
            HandleTimeoutOperation();
        }

        private void ApplicationViewModel_CountPauseEvent(object sender, DeviceTransactionResult e)
        {
            CurrentCountScreenState = CountScreenState.CASHIN_PAUSE;
            if (!AutoDropChecked)
                return;
            ApplicationViewModel.EscrowDrop();
        }

        private void ApplicationViewModel_CountEndEvent(object sender, DeviceTransactionResult e)
        {
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (ApplicationViewModel.DebugNoDevice)
                return;

            ApplicationViewModel.DeviceTransactionEnd();
        }

        public bool CanPauseCount => ApplicationViewModel.CanPauseCount;

        public void PauseCount()
        {
            if (CanPauseCount)
                ApplicationViewModel.PauseCount();
            else
                ApplicationViewModel.Log.Warning(nameof(CountScreenViewModel), "InvalidOperationException", nameof(PauseCount), "Illegal action: Cannot perform PauseCount when CanPauseCount is false");
        }

        public bool LastCanNext
        {
            get => _lastCanNext;
            set
            {
                if (_lastCanNext == value)
                    return;
                _lastCanNext = value;
                ResetIdleTimerOnUserInteraction();
            }
        }

        public new bool CanNext
        {
            get
            {
                int num1;
                if (!IsCountWithinTheLimits().HasFlag(CountLimitCheckResult.UNDERDEPOSIT))
                {
                    ApplicationViewModel applicationViewModel = ApplicationViewModel;
                    int num2;
                    if (applicationViewModel == null)
                    {
                        num2 = 0;
                    }
                    else
                    {
                        long? droppedAmountCents = applicationViewModel.CurrentTransaction?.DroppedAmountCents;
                        long num3 = 0;
                        num2 = droppedAmountCents.GetValueOrDefault() > num3 & droppedAmountCents.HasValue ? 1 : 0;
                    }
                    if (num2 != 0 && CanCancel)
                    {
                        num1 = !TimeoutMode ? 1 : 0;
                        goto label_7;
                    }
                }
                num1 = 0;
            label_7:
                bool canNext = num1 != 0;
                if (ApplicationViewModel.DeviceManager != null)
                    canNext = canNext && !ApplicationViewModel.DeviceManager.ClearHopperRequest;
                return canNext;
            }
        }

        public void Next()
        {
            ClearErrorText();
            PrintErrorText("processing, please wait...");
            ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
            BackgroundWorker backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = false
            };
            backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
            backgroundWorker.RunWorkerAsync();

            backgroundWorker.RunWorkerCompleted += Worker_Completed_App_Restart;
        }

        void Worker_Completed_App_Restart(object sender, RunWorkerCompletedEventArgs e)
        {
            var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
            //Process.Start(currentExecutablePath);
            //Application.Current.Shutdown();
        }
        public bool LastCanCancel
        {
            get => _lastCanCancel;
            set
            {
                if (_lastCanCancel == value)
                    return;
                _lastCanCancel = value;
                ResetIdleTimerOnUserInteraction();
            }
        }

        public new bool CanCancel
        {
            get
            {
                bool canCancel;
                if (ApplicationViewModel.DeviceManager.HasEscrow)
                {
                    canCancel = AutoCountChecked ? CurrentTransaction.CountedAmountCents == 0L && CanEscrowDrop : ApplicationViewModel.CanTransactionEnd && !TimeoutMode;
                    if (ApplicationViewModel.DeviceManager != null)
                        canCancel = canCancel && !ApplicationViewModel.DeviceManager.ClearHopperRequest;
                }
                else
                    canCancel = ApplicationViewModel.CanTransactionEnd;
                LastCanCancel = canCancel;
                return canCancel;
            }
        }

        public void Cancel()
        {
            if (MessageBox.Show("Cancel Deposit?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                return;
            ApplicationViewModel.Log.Warning(GetType().Name, nameof(Cancel), "Command", "Customer has cancelled the transaction");
            Next();
        }

        public bool LastCanCount
        {
            get => _lastCanCount;
            set
            {
                if (_lastCanCount == value)
                    return;
                _lastCanCount = value;
                ResetIdleTimerOnUserInteraction();
            }
        }

        private bool HasStartedCounting { get; set; } = false;

        public bool CanBeginCount => CanCount && !HasStartedCounting;

        public void BeginCount()
        {
            Count();
            if (HasStartedCounting)
                return;
            HasStartedCounting = true;
        }

        public bool CanCount
        {
            get
            {
                CountLimitCheckResult limitCheckResult = IsCountWithinTheLimits() & ~CountLimitCheckResult.OVERDEPOSIT;
                bool canCount = !AutoCountChecked && ApplicationViewModel.CanCount && !TimeoutMode && limitCheckResult == CountLimitCheckResult.OK;
                LastCanCount = canCount;
                return canCount;
            }
        }

        public void Count()
        {
            if (!CanCount)
                return;
            ApplicationViewModel.Count();
        }

        public bool LastCanEscrowDrop
        {
            get => _lastCanEscrowDrop;
            set
            {
                if (_lastCanEscrowDrop == value)
                    return;
                _lastCanEscrowDrop = value;
                ResetIdleTimerOnUserInteraction();
            }
        }

        public bool DropDisclaimerIsVisible => CanEscrowDrop;

        public bool CanEscrowDrop
        {
            get
            {
                CountLimitCheckResult limitCheckResult = IsCountWithinTheLimits() & ~CountLimitCheckResult.OVERDEPOSIT;
                bool canEscrowDrop = !AutoDropChecked && ApplicationViewModel.CanEscrowDrop && !TimeoutMode && limitCheckResult == CountLimitCheckResult.OK && !limitCheckResult.HasFlag(CountLimitCheckResult.UNDERDEPOSIT);
                LastCanEscrowDrop = canEscrowDrop;
                return canEscrowDrop;
            }
        }


        public void EscrowDrop() => this.ApplicationViewModel.EscrowDrop();



        public bool LastCanEscrowReject
        {
            get => _lastCanEscrowReject;
            set
            {
                if (_lastCanEscrowReject == value)
                    return;
                _lastCanEscrowReject = value;
                ResetIdleTimerOnUserInteraction();
            }
        }

        public bool CanEscrowReject
        {
            get
            {
                bool canEscrowReject = ApplicationViewModel.CanEscrowReject && !TimeoutMode;
                LastCanEscrowReject = canEscrowReject;
                return canEscrowReject;
            }
        }


        public void EscrowReject() => this.ApplicationViewModel.EscrowReject();

        public CountLimitCheckResult IsCountWithinTheLimits()
        {
            CountLimitCheckResult limitCheckResult = CountLimitCheckResult.OK;
            AppTransaction currentTransaction1 = CurrentTransaction;
            if (currentTransaction1 != null && currentTransaction1.IsUnderDeposit)
                limitCheckResult |= CountLimitCheckResult.UNDERDEPOSIT;
            long num;
            if (limitCheckResult.HasFlag(CountLimitCheckResult.UNDERDEPOSIT))
            {
                if (string.IsNullOrWhiteSpace(ErrorText))
                {
                    string s = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("CountScreenViewModel.IsCountWithinTheLimits", "sys_UnderDeposit_ErrorMessage", "Kindly deposit {transaction.currency} {transaction.Underdeposit_amount} or more to continue or cancel the transaction");
                    string format = ApplicationViewModel.DeviceConfiguration?.APPLICATION_MONEY_FORMAT ?? "#,##0.00";
                    AppTransaction currentTransaction2 = CurrentTransaction;
                    string str1;
                    if (currentTransaction2 == null)
                    {
                        str1 = null;
                    }
                    else
                    {
                        TransactionLimitListItem transactionLimits = currentTransaction2.TransactionLimits;
                        if (transactionLimits == null)
                        {
                            str1 = null;
                        }
                        else
                        {
                            num = transactionLimits.underdeposit_amount;
                            str1 = num.ToString(format);
                        }
                    }
                    string newValue = str1;
                    string str2 = CustomerInputScreenReplace(s)?.Replace("{transaction.Underdeposit_amount}", newValue)?.Replace("{transaction.currency}", CurrentTransaction?.CurrencyCode);
                    PrintErrorText(string.Format("[{0}] ", 1019) + str2);
                }
            }
            else
            {
                string errorText = ErrorText;
                if (errorText != null && errorText.Contains(string.Format("[{0}]", 1019)))
                    ClearErrorText();
            }
            AppTransaction currentTransaction3 = CurrentTransaction;
            if (currentTransaction3 != null && currentTransaction3.IsOverCount)
                limitCheckResult |= CountLimitCheckResult.OVERCOUNT;
            if (limitCheckResult.HasFlag(CountLimitCheckResult.OVERCOUNT))
            {
                if (string.IsNullOrWhiteSpace(ErrorText))
                {
                    string s = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("CountScreenViewModel.IsCountWithinTheLimits", "sys_OverCount_ErrorMessage", "WARNING: deposits greater than {transaction.currency} {transaction.OverCount_amount} are not allowed.");
                    string newValue = CurrentTransaction?.TransactionLimits?.overcount_amount.ToString(ApplicationViewModel.DeviceConfiguration?.APPLICATION_MONEY_FORMAT ?? "#,##0.00");
                    string str = CustomerInputScreenReplace(s)?.Replace("{transaction.OverCount_amount}", newValue)?.Replace("{transaction.currency}", CurrentTransaction?.CurrencyCode);
                    PrintErrorText(string.Format("[{0}] ", 1017) + str);
                }
            }
            else
            {
                string errorText = ErrorText;
                if (errorText != null && errorText.Contains(string.Format("[{0}]", 1017)))
                    ClearErrorText();
            }
            AppTransaction currentTransaction4 = CurrentTransaction;
            if (currentTransaction4 != null && currentTransaction4.IsOverDeposit)
                limitCheckResult |= CountLimitCheckResult.OVERDEPOSIT;
            if (!limitCheckResult.HasFlag(CountLimitCheckResult.OVERCOUNT) && limitCheckResult.HasFlag(CountLimitCheckResult.OVERDEPOSIT))
            {
                if (string.IsNullOrWhiteSpace(ErrorText))
                {
                    string s = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("CountScreenViewModel.IsCountWithinTheLimits", "sys_OverDeposit_ErrorMessage", "WARNING: deposits greater than {transaction.currency} {transaction.Overdeposit_amount} will not be credited.");
                    string format = ApplicationViewModel.DeviceConfiguration?.APPLICATION_MONEY_FORMAT ?? "#,##0.00";
                    AppTransaction currentTransaction5 = CurrentTransaction;
                    string str3;
                    if (currentTransaction5 == null)
                    {
                        str3 = null;
                    }
                    else
                    {
                        TransactionLimitListItem transactionLimits = currentTransaction5.TransactionLimits;
                        if (transactionLimits == null)
                        {
                            str3 = null;
                        }
                        else
                        {
                            num = transactionLimits.overdeposit_amount;
                            str3 = num.ToString(format);
                        }
                    }
                    string newValue = str3;
                    string str4 = CustomerInputScreenReplace(s)?.Replace("{transaction.Overdeposit_amount}", newValue)?.Replace("{transaction.currency}", CurrentTransaction?.CurrencyCode);
                    PrintErrorText(string.Format("[{0}] ", 1016) + str4);
                }
            }
            else
            {
                string errorText = ErrorText;
                if (errorText != null && errorText.Contains(string.Format("[{0}]", 1016)))
                    ClearErrorText();
            }
            if (IsBagFullOverFlow)
                limitCheckResult |= CountLimitCheckResult.BAGOVERFLOW;
            return limitCheckResult;
        }

        public bool IsBagFullOverFlow
        {
            get
            {
                long num1 = ApplicationViewModel?.DeviceManager?.CurrentDeviceStatus?.Bag?.NoteLevel ?? int.MaxValue;
                long num2 = ApplicationViewModel?.DeviceManager?.CurrentDeviceStatus?.Bag?.NoteCapacity ?? int.MinValue;
                DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
                long num3 = deviceConfiguration != null ? deviceConfiguration.BAGFULL_OVERFLOW_COUNT : 1000L;
                long num4 = num2 + num3;
                bool isBagFullOverFlow = num1 >= num4;
                if (isBagFullOverFlow)
                {
                    string errorText = ErrorText;
                    if (errorText != null && !errorText.Contains(string.Format("[{0}]", 1015)))
                    {
                        if (!(Application.Current.FindResource("Bagfull_Overflow_WarningMessage") is string s))
                            s = "DEFAULT: Counting Complete. Press {btn_next_caption} to complete the deposit or {btn_escrow_reject_caption} to Cancel.";
                        PrintErrorText(string.Format(string.Format("[{0}] {1}", 1015, CustomerInputScreenReplace(s))));
                    }
                }
                else
                {
                    string errorText = ErrorText;
                    if (errorText != null && errorText.Contains(string.Format("[{0}]", 1015)))
                        ClearErrorText();
                }
                return isBagFullOverFlow;
            }
        }

        protected override void DoCancelTransactionOnTimeout()
        {
            TimeoutMode = true;
            if (ApplicationViewModel.DeviceManager.ControllerState != ControllerState.DROP || ApplicationViewModel.DeviceManager.CashAccSysSerialFix.DE50Operation != DE50OperatingState.Waiting)
                return;
            EscrowReject();
        }

        public void AcceptDropConfirmation()
        {
        }

        public void RejectDropConfirmation()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            ApplicationViewModel.CashInStartedEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CashInStartedEvent);
            ApplicationViewModel.CountStartedEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountStartedEvent);
            ApplicationViewModel.CountPauseEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountPauseEvent);
            ApplicationViewModel.EscrowDropEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_EscrowDropEvent);
            ApplicationViewModel.EscrowRejectEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_EscrowRejectEvent);
            ApplicationViewModel.EscrowOperationCompleteEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_EscrowOperationCompleteEvent);
            ApplicationViewModel.CountEndEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountEndEvent);
            ApplicationViewModel.NotifyCurrentTransactionStatusChangedEvent -= new EventHandler<EventArgs>(ApplicationViewModel_NotifyCurrentTransactionStatusChanged);
            ApplicationViewModel.DeviceManager.PropertyChanged -= new PropertyChangedEventHandler(DeviceManager_PropertyChanged);
            if (ApplicationViewModel.DeviceManager == null)
                return;
            ApplicationViewModel.DeviceManager.CashAccSysSerialFix.PropertyChanged -= new PropertyChangedEventHandler(CashAccSysSerialFix_PropertyChanged);
        }

        [Flags]
        public enum CountLimitCheckResult
        {
            ERROR = 0,
            OK = 1,
            UNDERDEPOSIT = 2,
            OVERDEPOSIT = 4,
            OVERCOUNT = 8,
            BAGOVERFLOW = 16,
        }
    }
}
