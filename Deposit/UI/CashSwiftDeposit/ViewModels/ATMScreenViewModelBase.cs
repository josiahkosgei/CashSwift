﻿using Caliburn.Micro;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftDeposit.ViewModels
{
    public class ATMScreenViewModelBase : Screen, IATMScreen
    {
        public ApplicationViewModel ApplicationViewModel;
        public bool isInitialised;
        protected PermissionRequiredResult _permissionRequiredResult;
        public int CurrentPageIndex;
        internal IList<ATMSelectionItem<object>> Screens = new List<ATMSelectionItem<object>>();
        private ATMSelectionItem<object> selected;
        private ICashSwiftWindowConductor _conductor;

        public PermissionRequiredResult PermissionRequiredResult => _permissionRequiredResult;

        private int pageSize { get; set; } = 6;

        public IList<ATMSelectionItem<object>> VisibleOptions => Screens.Skip(CurrentPageIndex * pageSize).Take(pageSize).ToList();

        public ATMSelectionItem<object> SelectedVisibleOption
        {
            get => selected;
            set
            {
                selected = value;
                NotifyOfPropertyChange(nameof(SelectedVisibleOption));
                Conductor.ShowDialog(value.Value);
                selected = null;
            }
        }

        public string ScreenTitle { get; set; }

        public ICashSwiftWindowConductor Conductor => _conductor;

        protected object CallingObject { get; set; }

        protected object NextObject { get; set; }

        protected string ErrorText { get; set; }

        public string CancelButton_Caption { get; set; }

        public string BackButton_Caption { get; set; }

        public string GetPreviousPageButton_Caption { get; set; }

        public string GetNextPageButton_Caption { get; set; }

        public ATMScreenViewModelBase(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          ICashSwiftWindowConductor conductor,
          object callingObject)
        {
            ScreenTitle = screenTitle;
            ApplicationViewModel = applicationViewModel ?? throw new ArgumentNullException("ApplicationViewModel was null during a call to ATMScreenViewModelBase");
            _conductor = conductor;
            CallingObject = callingObject;
            CancelButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.CancelButton_Caption", "sys_CancelButton_Caption", "Cancel");
            BackButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.BackButton_Caption", "sys_BackButton_Caption", "Back");
            GetPreviousPageButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetPreviousPageButton_Caption", "sys_GetPreviousPageButton_Caption", "Prev");
            GetNextPageButton_Caption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetNextPageButton_Caption", "sys_GetNextPageButton_Caption", "More");
        }

        protected override void OnViewAttached(object view, object context)
        {
            if (!GetType().Name.Equals("MenuBackendATMViewModel") && ApplicationViewModel.CurrentUser == null)
            {
                ApplicationViewModel.Log.Error(GetType().Name, 43, ApplicationErrorConst.ERROR_NO_USER_LOGGED_IN.ToString(), "User must be logged in");
                ApplicationViewModel.CloseDialog(true);
            }
            base.OnViewAttached(view, context);
        }

        public void GetFirstPage()
        {
            CurrentPageIndex = 0;
            GetPage(CurrentPageIndex);
        }

        public void GetLastPage()
        {
            CurrentPageIndex = Screens.Count - 1;
            GetPage(CurrentPageIndex);
        }

        public bool CanGetNextPage => Screens.Count - (CurrentPageIndex * pageSize + pageSize) > 0;

        public void GetNextPage()
        {
            CurrentPageIndex = (CurrentPageIndex + 1).Clamp(0, Screens.Count - 1);
            GetPage(CurrentPageIndex);
        }

        public bool CanGetPreviousPage => CurrentPageIndex > 0;

        public void GetPreviousPage()
        {
            CurrentPageIndex = (CurrentPageIndex - 1).Clamp(0, Screens.Count - 1);
            GetPage(CurrentPageIndex);
        }

        public void GetPage(int pageID)
        {
            CurrentPageIndex = pageID.Clamp(0, Screens.Count - 1);
            NotifyOfPropertyChange("VisibleOptions");
            NotifyOfPropertyChange("CanGetNextPage");
            NotifyOfPropertyChange("CanGetPreviousPage");
        }

        public void NavigateBack()
        {
            if (CallingObject == null)
                Conductor.CloseDialog();
            else
                Conductor.ShowDialog(CallingObject);
        }
    }
}
