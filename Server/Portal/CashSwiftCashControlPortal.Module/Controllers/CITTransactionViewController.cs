
//Controllers.CITTransactionViewController


using CashSwiftCashControlPortal.Module.BusinessObjects.CITs;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class CITTransactionViewController : ObjectViewController<ListView, CITTransaction>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            View.CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("IsVisibleByUserGroup([cit_id.device_id.user_group])");
        }

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();
    }
}
