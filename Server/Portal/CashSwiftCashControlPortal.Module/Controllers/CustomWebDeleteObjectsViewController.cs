
//Controllers.CustomWebDeleteObjectsViewController


using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web.SystemModule;
using System;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class CustomWebDeleteObjectsViewController : WebDeleteObjectsViewController
    {
        protected override void Delete(SimpleActionExecuteEventArgs args)
        {
            try
            {
                base.Delete(args);
            }
            catch (Exception ex)
            {
                throw CustomErrorController.HandleException(ex);
            }
        }
    }
}
