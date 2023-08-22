using Caliburn.Micro;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CashSwiftDeposit.ViewModels
{
    public class CITReportScreenViewModel : DepositorScreenViewModelBase
    {
        private new DepositorDBContext DBContext = new DepositorDBContext();
        private const int txPageSize = 10;
        private int maxPage;
        private int _currentPage;
        private IQueryable<CIT> txQuery;
        private IEnumerable<CIT> _citTransactionList;
        private CIT selectedCITTransaction;
        private IEnumerable<CITDenomination> _citDenominationList;

        public CITReportScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          object callingObject,
              ICashSwiftWindowConductor conductor)
          : base(screenTitle, applicationViewModel, callingObject, conductor)
        {
            txQuery = DBContext.CITs.Where(t => t.fromDate >= (DateTime?)txQueryStartDate && t.toDate < txQueryEndDate).OrderByDescending(t => t.toDate);
            Activated += new EventHandler<ActivationEventArgs>(CITReportScreenViewModel_Activated);
        }

        private void CITReportScreenViewModel_Activated(object sender, ActivationEventArgs e) => PageFirst_Transaction();

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
                NotifyOfPropertyChange(() => CanEmailCITTransactionList);
                NotifyOfPropertyChange(() => CanPrintCITReceipt);
            }
        }

        public DateTime txQueryStartDate { get; set; } = DateTime.MinValue;

        public DateTime txQueryEndDate { get; set; } = DateTime.Now;

        public string txQueryType { get; set; }

        public string txQueryCurrency { get; set; }

        public IEnumerable<CIT> CITTransactions
        {
            get => _citTransactionList;
            set
            {
                _citTransactionList = value;
                maxPage = (int)Math.Ceiling(txQuery.Count() / 10.0) - 1;
                NotifyOfPropertyChange(() => CITTransactions);
            }
        }

        public CIT SelectedCITTransaction
        {
            get => selectedCITTransaction;
            set
            {
                selectedCITTransaction = value;
                NotifyOfPropertyChange(() => SelectedCITTransaction);
                NotifyOfPropertyChange(() => CITDenominationList);
                NotifyOfPropertyChange(() => CanPrintCITReceipt);
            }
        }

        public IEnumerable<CITDenomination> CITDenominationList
        {
            get
            {
                CIT selectedCitTransaction = SelectedCITTransaction;
                return selectedCitTransaction == null ? null : (IEnumerable<CITDenomination>)selectedCitTransaction.CITDenominations.Select(x => new CITDenomination()
                {
                    cit_id = x.cit_id,
                    denom = x.denom / 100,
                    count = x.count,
                    subtotal = x.subtotal / 100L,
                    Currency = x.Currency,
                    currency_id = x.currency_id,
                    datetime = x.datetime,
                    CIT = x.CIT,
                    id = x.id
                }).OrderBy(x => x.currency_id);
            }
        }

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

        public void Page_Transaction() => CITTransactions = txQuery.Skip(CurrentTxPage * 10).Take(10).ToList();

        public bool CanEmailCITTransactionList => txQuery.Count() > 0;

        public void EmailCITTransactionList()
        {
            int num = (int)MessageBox.Show("Mail Sent");
        }

        public bool CanPrintCITReceipt => SelectedCITTransaction != null;

        public void PrintCITReceipt()
        {
            ApplicationViewModel.PrintCITReceipt(SelectedCITTransaction, DBContext, true);
            int num = (int)MessageBox.Show("Printing Complete");
        }
    }
}
