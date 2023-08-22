
//Util.Logger


using CashSwift.Library.Standard.Logging;
using System.Reflection;

namespace CashSwiftCashControlPortal.Module.Util
{
    public class Logger
    {
        public static readonly ICashSwiftLogger Log = new CashSwiftLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), "WebPortalLog", null);
        public static readonly ICashSwiftLogger AuditLog = new CashSwiftLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), "PortalAuditLog", null);
    }
}
