using CashSwift.Library.Standard.Statuses;
using CashSwiftCashControlPortal.Module.Properties;
using CashSwiftUtil.Logging;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.DB.Exceptions;
using System;
using System.Reflection;
using System.Web;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public static class CustomErrorController
    {
        public static readonly UtilDepositorLogger Log = new UtilDepositorLogger(Assembly.GetExecutingAssembly().GetName().Version, "WebPortalLog");

        public static Exception HandleException(Exception ex)
        {
            if (Settings.Default.SHOW_ERRORS_IN_PORTAL)
            {
                CustomErrorController.Log.WarningFormat(nameof(CustomErrorController), nameof(HandleException), "Error", "Error: {0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
                return ex;
            }
            Exception exception;
            switch (ex)
            {
                case CashSwiftException _:
                    exception =  new CashSwiftException(HttpUtility.HtmlEncode(ex.Message), ex);
                    break;
                case SqlExecutionErrorException _:
                    exception = new Exception("System encountered a database error. Contact your administrator", ex);
                    break;
                case ValidationException _:
                label_6:
                    exception = new Exception(HttpUtility.HtmlEncode(ex.Message), ex);
                    break;
                default:
                    if ((ex.InnerException == null || !(ex.InnerException is ValidationException)) && !(ex is AuthenticationException) && !(ex is ChangePasswordException))
                    {
                        exception = !ex.Source.Contains("DevExpress.ExpressApp.Security") ? new Exception("System error. Contact your administrator", ex) : new Exception(HttpUtility.HtmlEncode(ex.Message), ex);
                        break;
                    }
                    goto label_6;
            }
            CustomErrorController.Log.WarningFormat(nameof(CustomErrorController), nameof(HandleException), "Error", "Error: {0}>>{1}>>{2}", exception?.Message, exception?.InnerException?.Message, exception?.InnerException?.InnerException?.Message);
            return exception;
        }
    }

}
