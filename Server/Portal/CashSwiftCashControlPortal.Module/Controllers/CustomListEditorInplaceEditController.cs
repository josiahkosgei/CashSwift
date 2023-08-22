
//Controllers.CustomListEditorInplaceEditController


using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.SystemModule;
using System;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class CustomListEditorInplaceEditController : ListEditorInplaceEditController
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            RecordsNavigationController controller = Frame.GetController<RecordsNavigationController>();
            if (controller == null)
                return;
            controller.NextObjectAction.Active.SetItemValue("Block", false);
            controller.PreviousObjectAction.Active.SetItemValue("Block", false);
        }

        protected override void CommitChangesIfNeed()
        {
            try
            {
                base.CommitChangesIfNeed();
            }
            catch (Exception ex)
            {
                throw CustomErrorController.HandleException(ex);
            }
        }
    }
}
