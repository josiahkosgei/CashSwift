using Caliburn.Micro;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CashSwiftDeposit.ViewModels
{
    public class TransactionReportScreenViewModel : DepositorScreenViewModelBase
    {
        private const int txPageSize = 10;
        private int maxPage;
        private int _currentPage;
        private IQueryable<Transaction> txQuery;
        private IEnumerable<Transaction> _transactionList;
        private Transaction selectedTransaction;
        private IEnumerable<DenominationDetail> _denominationList;

        public TransactionReportScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          object callingObject,
          ICashSwiftWindowConductor conductor)
          : base(screenTitle, applicationViewModel, callingObject, conductor)
        {
            txQuery = DBContext.Transactions.Where(t => t.tx_end_date >= (DateTime?)txQueryStartDate && t.tx_end_date < (DateTime?)txQueryEndDate).OrderByDescending(t => t.tx_end_date);
            Activated += new EventHandler<ActivationEventArgs>(TransactionReportScreenViewModel_Activated);
        }

        private void TransactionReportScreenViewModel_Activated(object sender, ActivationEventArgs e) => PageFirst_Transaction();

        public int CurrentTxPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                NotifyOfPropertyChange(() => CanPageFirst_Transaction);
                NotifyOfPropertyChange(() => CanPageLast_Transaction);
                NotifyOfPropertyChange(() => CanPageNext_Transaction);
                NotifyOfPropertyChange(() => CanPagePrevious_Transaction);
                NotifyOfPropertyChange(() => PageNumberText);
                NotifyOfPropertyChange(() => CanEmailTransactionList);
                NotifyOfPropertyChange(() => CanPrintReceipt);
            }
        }

        public DateTime txQueryStartDate { get; set; } = DateTime.MinValue;

        public DateTime txQueryEndDate { get; set; } = DateTime.Now;

        public string txQueryType { get; set; }

        public string txQueryCurrency { get; set; }

        public IEnumerable<Transaction> Transactions
        {
            get => _transactionList;
            set
            {
                _transactionList = value;
                maxPage = (int)Math.Ceiling(txQuery.Count() / 10.0) - 1;
                NotifyOfPropertyChange(() => Transactions);
            }
        }

        public Transaction SelectedTransaction
        {
            get => selectedTransaction;
            set
            {
                selectedTransaction = value;
                NotifyOfPropertyChange(() => SelectedTransaction);
                NotifyOfPropertyChange(() => DenominationList);
                NotifyOfPropertyChange(() => CanPrintReceipt);
            }
        }

        public IEnumerable<DenominationDetail> DenominationList => SelectedTransaction?.DenominationDetails;

        public string PageNumberText => string.Format("Page {0} of {1}", CurrentTxPage + 1, maxPage + 1);

        public bool CanPageFirst_Transaction => CurrentTxPage > 0;

        public void PageFirst_Transaction()
        {
            CurrentTxPage = 0;
            Page_Transaction();
        }

        public bool CanPagePrevious_Transaction => CurrentTxPage > 0;

        public void PagePrevious_Transaction()
        {
            if (CurrentTxPage <= 0)
            {
                PageFirst_Transaction();
            }
            else
            {
                --CurrentTxPage;
                Page_Transaction();
            }
        }

        public bool CanPageNext_Transaction => CurrentTxPage < maxPage;

        public void PageNext_Transaction()
        {
            if (CurrentTxPage >= maxPage)
            {
                PageLast_Transaction();
            }
            else
            {
                ++CurrentTxPage;
                Page_Transaction();
            }
        }

        public bool CanPageLast_Transaction => CurrentTxPage < maxPage;

        public void PageLast_Transaction()
        {
            CurrentTxPage = maxPage;
            Page_Transaction();
        }

        public void Page_Transaction() => Transactions = txQuery.Skip(CurrentTxPage * 10).Take(10).ToList();

        public bool CanEmailTransactionList => txQuery.Count() > 0;

        public void EmailTransactionList()
        {
            int num = (int)MessageBox.Show("Mail Sent");
        }

        public bool CanPrintReceipt => SelectedTransaction != null;


        public void PrintReceipt()
        {
            ApplicationViewModel.PrintReceipt(SelectedTransaction, true);
            int num = (int)MessageBox.Show("Sent to printer");
        }
    }
}
