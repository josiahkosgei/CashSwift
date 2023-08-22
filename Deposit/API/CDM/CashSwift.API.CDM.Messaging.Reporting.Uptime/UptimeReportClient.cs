
using CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging;
using CashSwift.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime
{
    public class UptimeReportClient : CDM_APIClient, IUptimeReportController
    {
        public ICashSwiftAPILogger Log { get; set; }

        public UptimeReportClient(
          string apiBaseAddress,
          Guid AppID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashSwiftAPILogger(nameof(UptimeReportClient), configuration), apiBaseAddress, AppID, appKey, configuration)
        {
        }

        public async Task<UptimeReportResponse> GetUptimeReportAsync(
          UptimeReportRequest request)
        {
            return await SendAsync<UptimeReportResponse>("api/UptimeReport/Generate", request);
        }
    }
}
