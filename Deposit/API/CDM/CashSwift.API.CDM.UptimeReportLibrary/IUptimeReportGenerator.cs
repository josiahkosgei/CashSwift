
using CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging;
using System.Threading.Tasks;

namespace CashSwift.API.CDM.UptimeReportLibrary
{
    public interface IUptimeReportGenerator
    {
        Task<UptimeReportResponse> GenerateUptimeReportAsync(
          UptimeReportRequest request);
    }
}
