
//Controllers.DeviceStatusViewController


using CashSwiftCashControlPortal.Module.BusinessObjects.Monitoring;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class DeviceStatusViewController : ObjectViewController<ListView, DeviceStatus>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            View.CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("IsVisibleByUserGroup([device_id.user_group])");
        }

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();
    }
}
