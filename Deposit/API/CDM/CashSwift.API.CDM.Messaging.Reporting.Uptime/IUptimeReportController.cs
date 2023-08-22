using CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging;
using CashSwift.Library.Standard.Logging;
using System.Threading.Tasks;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime
{
    public interface IUptimeReportController
    {
        ICashSwiftAPILogger Log { get; set; }

        Task<UptimeReportResponse> GetUptimeReportAsync(
          UptimeReportRequest request);
    }
}
