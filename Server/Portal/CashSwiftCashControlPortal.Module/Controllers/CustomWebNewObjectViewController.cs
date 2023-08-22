
//Controllers.CustomWebNewObjectViewController


using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web.SystemModule;
using System;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class CustomWebNewObjectViewController : WebNewObjectViewController
    {
        protected override void New(SingleChoiceActionExecuteEventArgs args)
        {
            try
            {
                base.New(args);
            }
            catch (Exception ex)
            {
                throw CustomErrorController.HandleException(ex);
            }
        }
    }
}
