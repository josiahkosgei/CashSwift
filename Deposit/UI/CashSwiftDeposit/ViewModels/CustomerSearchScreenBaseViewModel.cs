﻿using CashSwift.API.Messaging.CDM.GUIControl.AccountsLists;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDeposit.Models;
using CashSwiftUtil.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CashSwiftDeposit.ViewModels
{
    public abstract class CustomerSearchScreenBaseViewModel : DepositorCustomerScreenBaseViewModel
    {
        private bool searchButtonIsVisible = true;
        private string searchButtonCaption;
        private bool cancelSearchButtonIsVisible;
        private string cancelSearchButtonCaption;
        private bool screenFooterIsVisible = true;
        private bool screenHeaderIsVisible = true;
        private string previousQuery;
        private bool keyboardGridIsVisible;
        private int PageNumber;
        protected int PageSize = 10;
        private ATMSelectionItem<object> selected;

        public CustomerSearchScreenBaseViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            SetFormMode(CustomerSearchFormMode.NORMAL);
            SearchButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(nameof(SearchButtonCaption), "sys_SearchButtonCaption", "Search");
            CancelSearchButtonCaption = ApplicationViewModel.CashSwiftTranslationService?.TranslateSystemText(nameof(CancelSearchButtonCaption), "sys_CancelSearchButtonCaption", "Cancel");
            PropertyChanged += new PropertyChangedEventHandler(CustomerSearchScreenBaseViewModel_PropertyChanged);
        }

        public ObservableCollection<ATMSelectionItem<object>> FullList { get; set; }

        public ObservableCollection<ATMSelectionItem<object>> FilteredList { get; set; }

        public ATMSelectionItem<object> SelectedFilteredList
        {
            get => selected;
            set
            {
                PrintErrorText("Processing, please wait...");
                selected = value;
                NotifyOfPropertyChange(nameof(SelectedFilteredList));
                ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.WorkerReportsProgress = false;
                backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
                backgroundWorker.RunWorkerAsync();
            }
        }

        public bool SearchButtonIsVisible
        {
            get => searchButtonIsVisible;
            set
            {
                searchButtonIsVisible = value;
                NotifyOfPropertyChange(() => SearchButtonIsVisible);
            }
        }

        public string SearchButtonCaption
        {
            get => searchButtonCaption;
            set
            {
                searchButtonCaption = value;
                NotifyOfPropertyChange(() => SearchButtonCaption);
            }
        }

        public bool CancelSearchButtonIsVisible
        {
            get => cancelSearchButtonIsVisible;
            set
            {
                cancelSearchButtonIsVisible = value;
                NotifyOfPropertyChange(() => CancelSearchButtonIsVisible);
            }
        }

        public string CancelSearchButtonCaption
        {
            get => cancelSearchButtonCaption;
            set
            {
                cancelSearchButtonCaption = value;
                NotifyOfPropertyChange(() => CancelSearchButtonCaption);
            }
        }

        public bool ScreenFooterIsVisible
        {
            get => screenFooterIsVisible;
            set
            {
                screenFooterIsVisible = value;
                NotifyOfPropertyChange(() => ScreenFooterIsVisible);
            }
        }

        public CustomerSearchFormMode FormMode { get; set; }

        public bool ScreenHeaderIsVisible
        {
            get => screenHeaderIsVisible;
            set
            {
                screenHeaderIsVisible = value;
                NotifyOfPropertyChange(() => ScreenHeaderIsVisible);
            }
        }

        public bool KeyboardGridIsVisible
        {
            get => keyboardGridIsVisible;
            set
            {
                keyboardGridIsVisible = value;
                NotifyOfPropertyChange(() => KeyboardGridIsVisible);
            }
        }

        protected void ScrollList()
        {
            List<ATMSelectionItem<object>> list = CreateList(Task.Run(() => ApplicationViewModel.GetAccountListAsync(ApplicationViewModel.CurrentTransaction.TransactionType, ApplicationViewModel.CurrentTransaction.Currency.code, PageNumber, PageSize))?.Result);
            if (FullList == null)
            {
                FullList = new ObservableCollection<ATMSelectionItem<object>>(list);
            }
            else
            {
                foreach (ATMSelectionItem<object> atmSelectionItem in list)
                    FullList.Add(atmSelectionItem);
            }
        }

        protected List<ATMSelectionItem<object>> CreateList(
          AccountsListResponse AccountList)
        {
            try
            {
                return AccountList != null ? AccountList.Accounts.Select(x => new ATMSelectionItem<object>(x.Icon == null ? ImageManipuation.GetBitmapFromFile("{AppDir}\\NoImg.png") : ImageManipuation.GetBitmapFromBytes(x.Icon), x.account_number + " | " + x.account_name, x)).ToList() : null;
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(CustomerSearchScreenBaseViewModel), "Error", nameof(CreateList), ex.MessageString(), Array.Empty<object>());
                return null;
            }
        }

        public void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset < scrollViewer.ScrollableHeight)
                return;
            ++PageNumber;
            ScrollList();
        }

        public void PerformSearch()
        {
            FilteredList = string.IsNullOrWhiteSpace(CustomerInput) || CustomerInput.Length <= 2 ? FullList : new ObservableCollection<ATMSelectionItem<object>>(CreateList(Task.Run(() => ApplicationViewModel.SearchAccountListAsync(CustomerInput, ApplicationViewModel.CurrentTransaction.TransactionType, ApplicationViewModel.CurrentTransaction.CurrencyCode)).Result));
            NotifyOfPropertyChange(() => FilteredList);
        }

        public void SearchButton()
        {
            switch (FormMode)
            {
                case CustomerSearchFormMode.NORMAL:
                    previousQuery = CustomerInput;
                    SetFormMode(CustomerSearchFormMode.SEARCH);
                    break;
                case CustomerSearchFormMode.SEARCH:
                    PerformSearch();
                    SetFormMode(CustomerSearchFormMode.NORMAL);
                    break;
            }
        }

        public void CancelSearchButton()
        {
            CustomerInput = previousQuery;
            SetFormMode(CustomerSearchFormMode.NORMAL);
        }

        public void CustomerInputGotFocus()
        {
            if (FormMode == CustomerSearchFormMode.SEARCH)
                return;
            SearchButton();
        }

        private void SetFormMode(CustomerSearchFormMode mode)
        {
            FormMode = mode;
            switch (FormMode)
            {
                case CustomerSearchFormMode.NORMAL:
                    CancelSearchButtonIsVisible = false;
                    KeyboardGridIsVisible = false;
                    ScreenHeaderIsVisible = true;
                    ScreenFooterIsVisible = true;
                    break;
                case CustomerSearchFormMode.SEARCH:
                    CancelSearchButtonIsVisible = true;
                    KeyboardGridIsVisible = true;
                    ScreenHeaderIsVisible = false;
                    ScreenFooterIsVisible = false;
                    break;
            }
        }

        private void CustomerSearchScreenBaseViewModel_PropertyChanged(
          object sender,
          PropertyChangedEventArgs e)
        {
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e) => PerformSelection();

        public abstract void PerformSelection();
    }
}
