using CashSwift.Library.Standard.Statuses;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels.RearScreen;
using System.Windows;

namespace CashSwiftDeposit.ViewModels
{
    internal class AdminDeviceSummaryViewModel : FormViewModelBase
    {
        private Device Device { get; }

        public AdminDeviceSummaryViewModel(
          ApplicationViewModel applicationViewModel,
          ICashSwiftWindowConductor conductor,
          object callingObject,
          bool isNewEntry)
          : base(applicationViewModel, conductor, callingObject, isNewEntry)
        {
            Device = applicationViewModel?.ApplicationModel?.GetDevice(DBContext);
            if (Device == null)
                ApplicationViewModel.Log.Error(GetType().Name, 106, ApplicationErrorConst.ERROR_NULL_REFERENCE_EXCEPTION.ToString(), "Device is null when constructing AdminDeviceSummaryViewModel");
            if (!(Application.Current.FindResource("DeviceSummaryScreenTitle") is string str))
                str = "Device Summary";
            ScreenTitle = str;
            ActivateItemAsync(new FormListViewModel(this));
        }
    }
}
