
//Controllers.DeviceVisibilityViewController


using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class DeviceVisibilityViewController : ObjectViewController<ListView, Device>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            View.CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("IsVisibleByUserGroup([user_group])");
        }

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();
    }
}
