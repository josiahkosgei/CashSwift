
//Controllers.CustomWebModificationsController


using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web.SystemModule;
using System;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class CustomWebModificationsController : WebModificationsController
    {
        protected override void Save(SimpleActionExecuteEventArgs args)
        {
            try
            {
                base.Save(args);
            }
            catch (Exception ex)
            {
                throw CustomErrorController.HandleException(ex);
            }
        }

        protected override void SaveAndClose(SimpleActionExecuteEventArgs args)
        {
            try
            {
                base.SaveAndClose(args);
            }
            catch (Exception ex)
            {
                throw CustomErrorController.HandleException(ex);
            }
        }

        protected override void SaveAndNew(SingleChoiceActionExecuteEventArgs args)
        {
            try
            {
                base.SaveAndNew(args);
            }
            catch (Exception ex)
            {
                throw CustomErrorController.HandleException(ex);
            }
        }
    }
}
