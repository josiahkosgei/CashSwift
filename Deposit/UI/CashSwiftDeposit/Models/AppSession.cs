using Caliburn.Micro;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Utils.AlertClasses;
using CashSwiftDeposit.ViewModels;
using System;
using System.ComponentModel;
using System.Globalization;

namespace CashSwiftDeposit.Models
{
    public class AppSession : PropertyChangedBase
    {
        private AppTransaction _transaction;
        private ApplicationViewModel _applicationViewModel;

        public AppSession(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
            Device = ApplicationViewModel.ApplicationModel.GetDevice(DBContext);
            DepositorSession = new DepositorSession()
            {
                id = GuidExt.UuidCreateSequential(),
                device_id = Device.id,
                session_start = DateTime.Now,
                session_end = new DateTime?(),
                language_code = "en-gb"
            };
            SessionID = DepositorSession.id;
            TermsAccepted = false;
            SessionStart = DateTime.Now;
            SessionComplete = false;
            PropertyChanged += new PropertyChangedEventHandler(OnPropertyChangedEvent);
            DBContext.DepositorSessions.Add(DepositorSession);
            ApplicationViewModel.SaveToDatabase(DBContext);
            ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(SessionStart), "Session", "Session {0} started", SessionID.ToString().ToUpper());
        }

        public Guid SessionID
        {
            get => DepositorSession.id;
            set
            {
                DepositorSession.id = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionID Set", "Tx Property Changed", "SessionID set to {0}", SessionID);
                NotifyOfPropertyChange(nameof(SessionID));
            }
        }

        public DateTime SessionStart
        {
            get => DepositorSession.session_start;
            set
            {
                DepositorSession.session_start = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionStart Set", "Tx Property Changed", "SessionStart set to {0}", SessionStart);
                NotifyOfPropertyChange(nameof(SessionStart));
            }
        }

        public DateTime SessionEnd
        {
            get => DepositorSession.session_end.Value;
            set
            {
                DepositorSession.session_end = new DateTime?(value);
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionEnd Set", "Tx Property Changed", "SessionEnd set to {0}", SessionEnd);
                NotifyOfPropertyChange(nameof(SessionEnd));
            }
        }

        public AppTransaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                NotifyOfPropertyChange(nameof(Transaction));
            }
        }

        public CultureInfo Culture { get; set; }

        public CultureInfo UICulture { get; set; }

        public string Language
        {
            get => DepositorSession.language_code;
            set
            {
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "Language Changed", "Tx Property Changed", "Language changed from {0} to {1}", DepositorSession.language_code, value);
                DepositorSession.language_code = value;
                NotifyOfPropertyChange(() => Language);
                Culture = new CultureInfo(Language);
                NotifyOfPropertyChange(() => Culture);
                UICulture = new CultureInfo(Language);
                NotifyOfPropertyChange(() => UICulture);
            }
        }

        public bool SessionComplete
        {
            get => DepositorSession.complete;
            set
            {
                DepositorSession.complete = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionComplete Set", "Tx Property Changed", "SessionComplete set to {0}", SessionComplete);
                NotifyOfPropertyChange(nameof(SessionComplete));
            }
        }

        public bool SessionCompleteSuccess
        {
            get => DepositorSession.complete_success;
            set
            {
                DepositorSession.complete_success = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionCompleteSuccess Set", "Tx Property Changed", "SessionCompleteSuccess set to {0}", SessionCompleteSuccess);
                NotifyOfPropertyChange(nameof(SessionCompleteSuccess));
            }
        }

        public int SessionErrorCode
        {
            get => DepositorSession.error_code.Value;
            set
            {
                DepositorSession.error_code = new int?(value);
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionErrorCode Set", "Tx Property Changed", "SessionErrorCode set to {0}", SessionErrorCode);
                NotifyOfPropertyChange(nameof(SessionErrorCode));
            }
        }

        public string SessionErrorMessage
        {
            get => DepositorSession.error_message;
            set
            {
                DepositorSession.error_message = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionErrorMessage Set", "Tx Property Changed", "SessionErrorMessage set to {0}", SessionErrorMessage);
                NotifyOfPropertyChange(nameof(SessionErrorMessage));
            }
        }

        public bool TermsAccepted
        {
            get => DepositorSession.terms_accepted;
            set
            {
                DepositorSession.terms_accepted = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "TermsAccepted Set", "Tx Property Changed", "TermsAccepted set to {0}", TermsAccepted);
                NotifyOfPropertyChange(nameof(TermsAccepted));
            }
        }

        public bool AccountVerified
        {
            get => DepositorSession.account_verified;
            set
            {
                DepositorSession.account_verified = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "AccountVerified Set", "Tx Property Changed", "AccountVerified set to {0}", AccountVerified);
                NotifyOfPropertyChange(nameof(AccountVerified));
            }
        }

        public bool ReferenceAccountVerified
        {
            get => DepositorSession.reference_account_verified;
            set
            {
                DepositorSession.reference_account_verified = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "ReferenceAccountVerified Set", "Tx Property Changed", "ReferenceAccountVerified set to {0}", ReferenceAccountVerified);
                NotifyOfPropertyChange(nameof(ReferenceAccountVerified));
            }
        }

        internal bool HasCounted { get; set; } = false;

        internal bool CountingStarted { get; set; } = false;

        internal bool CountingEnded { get; set; } = false;

        public ApplicationViewModel ApplicationViewModel => _applicationViewModel;

        public DepositorDBContext DBContext { get; set; } = new DepositorDBContext();

        public Device Device { get; }

        public DepositorSession DepositorSession { get; set; }

        public override bool Equals(object obj)
        {
            Guid? nullable = obj is AppSession appSession ? new Guid?(appSession.SessionID) : new Guid?();
            Guid sessionId = SessionID;
            return nullable.HasValue && (!nullable.HasValue || nullable.GetValueOrDefault() == sessionId);
        }

        public override int GetHashCode() => SessionID.GetHashCode();

        internal void CreateTransaction(TransactionTypeListItem transactionType)
        {
            Transaction = new AppTransaction(this, transactionType, transactionType.default_account_currency);
            Transaction.TransactionLimitReachedEvent += new EventHandler<EventArgs>(Transaction_TransactionLimitReachedEvent);
            ApplicationViewModel.AlertManager.SendAlert(new AlertTransactionStarted(Transaction, Device, DateTime.Now));
        }

        private void Transaction_TransactionLimitReachedEvent(object sender, EventArgs e) => OnTransactionLimitReachedEvent(this, e);

        internal void LogSessionError(ApplicationErrorConst result, string errormessage)
        {
            SessionErrorCode = (int)result;
            SessionErrorMessage = errormessage;
        }

        internal void EndSession(
          bool success,
          int errorCode,
          ApplicationErrorConst result,
          string errorMessage)
        {
            ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(EndSession), "Session", "Session Result = {0}, transaction result = {1},errorcode = {2}, errormessage={3}", success, result, errorCode, errorMessage);
            SessionComplete = true;
            SessionCompleteSuccess = success;
            if (result != 0)
                LogSessionError(result, errorMessage);
            SessionEnd = DateTime.Now;
            if (Transaction != null && !Transaction.Completed)
                Transaction.EndTransaction(result, errorMessage);
            ApplicationViewModel.SaveToDatabase(DBContext);
            DBContext.Dispose();
        }

        private void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e) => SaveToDatabase();

        public event EventHandler<EventArgs> TransactionLimitReachedEvent;

        private void OnTransactionLimitReachedEvent(object sender, EventArgs e)
        {
            if (TransactionLimitReachedEvent == null)
                return;
            TransactionLimitReachedEvent(this, e);
        }

        internal void SaveToDatabase() => ApplicationViewModel.SaveToDatabase(DBContext);
    }
}
