// ViewModels.LanguageListScreenViewModel


using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("16A5EB6B-4D4F-4652-ADC2-0351F1FBDFA1")]
    public class LanguageListScreenViewModel : CustomerListScreenBaseViewModel
    {
        public LanguageListScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            ApplicationViewModel applicationViewModel1 = ApplicationViewModel;
            List<ATMSelectionItem<object>> atmSelectionItemList;
            if (applicationViewModel1 == null)
            {
                atmSelectionItemList = null;
            }
            else
            {
                List<Language> languagesAvailable = applicationViewModel1.LanguagesAvailable;
                if (languagesAvailable == null)
                {
                    atmSelectionItemList = null;
                }
                else
                {
                    IEnumerable<ATMSelectionItem<object>> source = languagesAvailable.Select(x => new ATMSelectionItem<object>(x.flag, x.name, x));
                    atmSelectionItemList = source != null ? source.ToList() : null;
                }
            }
            FullList = atmSelectionItemList;
            GetFirstPage();
        }

        public void Cancel() => ApplicationViewModel.CancelSessionOnUserInput();

        public override void PerformSelection()
        {
            ApplicationViewModel?.SetLanguage(SelectedFilteredList.Value as Language);
            ApplicationViewModel.NavigateNextScreen();
        }
    }
}
