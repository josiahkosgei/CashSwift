using CashSwift.Library.Standard.Statuses;
using System;
using System.ComponentModel;

namespace DeviceManager
{
    public interface IDeviceManager
    {
        bool EscrowBillPresent { get; set; }

        DeviceManagerMode DeviceManagerMode { get; set; }

        bool Enabled { get; set; }

        ControllerState ControllerState { get; set; }

        bool HasEscrow { get; set; }

        IControllerStatus CurrentDeviceStatus { get; set; }

        DeviceManagerState CurrentState { get; set; }

        bool CanEndCount { get; }

        bool CanEscrowDrop { get; }

        bool CanCount { get; }

        bool CanTransactionEnd { get; }

        bool CanPauseCount { get; }

        bool CanEscrowReject { get; }

        long DroppedAmountCents { get; }

        double DroppedAmountMajorCurrency { get; }

        DeviceTransaction CurrentTransaction { get; set; }

        Version DeviceManagerVersion { get; }

        bool CanClearNoteJam { get; }

        event EventHandler<DeviceStatusChangedEventArgs> RaiseDeviceStatusChangedEvent;

        event EventHandler<ControllerStateChangedEventArgs> RaiseControllerStateChangedEvent;

        event EventHandler<DeviceStateChangedEventArgs> RaiseDeviceStateChangedEvent;

        event EventHandler<CountChangedEventArgs> RaiseCountChangedEvent;

        event EventHandler<ConnectionEventArgs> RaiseConnectionEvent;

        event EventHandler<CITResult> CITResultEvent;

        event EventHandler<EventArgs> DoorOpenEvent;

        event EventHandler<EventArgs> DoorClosedEvent;

        event EventHandler<EventArgs> BagRemovedEvent;

        event EventHandler<EventArgs> BagPresentEvent;

        event EventHandler<EventArgs> BagOpenedEvent;

        event EventHandler<EventArgs> BagClosedEvent;

        event EventHandler<ControllerStatus> BagFullAlertEvent;

        event EventHandler<ControllerStatus> BagFullWarningEvent;

        event EventHandler<EventArgs> BagReplacedEvent;

        event EventHandler<EventArgs> DeviceLockedEvent;

        event EventHandler<EventArgs> DeviceUnlockedEvent;

        event PropertyChangedEventHandler PropertyChanged;

        event EventHandler<EventArgs> NotifyCurrentTransactionStatusChangedEvent;

        event EventHandler<DeviceStatusChangedEventArgs> StatusReportEvent;

        event EventHandler<DeviceTransactionResult> TransactionStatusEvent;

        event EventHandler<DeviceTransactionResult> TransactionEndEvent;

        event EventHandler<DeviceTransactionResult> DropResultEvent;

        event EventHandler<DeviceTransactionResult> CashInStartedEvent;

        event EventHandler<DeviceTransactionResult> CountEndEvent;

        event EventHandler<DeviceTransactionResult> CountStartedEvent;

        event EventHandler<DeviceTransactionResult> CountPauseEvent;

        event EventHandler<DeviceTransactionResult> EscrowDropEvent;

        event EventHandler<DeviceTransactionResult> EscrowRejectEvent;

        event EventHandler<DeviceTransactionResult> EscrowOperationCompleteEvent;

        event EventHandler<EventArgs> EscrowJamStartEvent;

        event EventHandler<EventArgs> EscrowJamClearWaitEvent;

        event EventHandler<EventArgs> EscrowJamEndRequestEvent;

        event EventHandler<EventArgs> EscrowJamEndEvent;

        event EventHandler<StringResult> ConnectionEvent;

        event EventHandler<DeviceTransaction> TransactionStartedEvent;

        event EventHandler<EventArgs> NoteJamStartEvent;

        event EventHandler<EventArgs> NoteJamClearWaitEvent;

        event EventHandler<EventArgs> NoteJamEndRequestEvent;

        event EventHandler<EventArgs> NoteJamEndEvent;

        void CountNotes();

        void CountCoins();

        void CountBoth();

        void ResetDevice(bool openEscrow = false);

        void SetCurrency(string currency);

        void Connect();

        void Disconnect();

        void CashInStart();

        void Initialise();

        void ShowDeviceController();

        void EscrowReject();

        void TransactionEnd();

        void StartCIT(string sealNumber);

        void EndCIT(string bagnumber);

        void TransactionStart(
          string currency,
          string accountNumber,
          string sessionID,
          string transactionID,
          long transactionLimitCents = 9223372036854775807,
          long transactionValueCents = 0);

        void Count();

        void PauseCount();

        void EscrowDrop();

        void OnEscrowDropEvent(object sender, DeviceTransactionResult e);

        void OnEscrowRejectEvent(object sender, DeviceTransactionResult e);

        void OnEscrowOperationCompleteEvent(object sender, DeviceTransactionResult e);

        void OnEscrowJamStartEvent(object sender, EventArgs e);

        void OnEscrowJamClearWaitEvent(object sender, EventArgs e);

        void OnEscrowJamEndRequestEvent(object sender, EventArgs e);

        void OnEscrowJamEndEvent(object sender, EventArgs e);

        void EndEscrowJam();

        void ClearEscrowJam();

        void ClearNotesinEscrowWithDrop();

        void ClearNoteJam();

        void OnNoteJamStartEvent(object sender, EventArgs e);

        void OnNoteJamClearWaitEvent(object sender, EventArgs e);

        void OnNoteJamEndRequestEvent(object sender, EventArgs e);

        void OnNoteJamEndEvent(object sender, EventArgs e);
    }
}
