using CashSwift.API.CDM.Messaging.Reporting.Uptime;
using CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging;
using CashSwift.API.CDM.UptimeReportLibrary;
using CashSwift.Library.Standard.Logging;
using CashSwift.Library.Standard.Utilities;
using System;
using System.Threading.Tasks;

namespace CashSwift.API.CDM.ReportingService.Controllers
{
    public class UptimeReportController : IUptimeReportController
    {
        public UptimeReportController(
          IUptimeReportConfiguration uptimeReportConfiguration,
          ICashSwiftAPILogger log,
          IUptimeReportGenerator uptimeReportGenerator)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            UptimeReportConfiguration = uptimeReportConfiguration;
            UptimeReportGenerator = uptimeReportGenerator ?? throw new ArgumentNullException(nameof(uptimeReportGenerator));
        }

        private IUptimeReportConfiguration UptimeReportConfiguration { get; set; }

        public ICashSwiftAPILogger Log { get; set; }

        public IUptimeReportGenerator UptimeReportGenerator { get; set; }

        public async Task<UptimeReportResponse> GetUptimeReportAsync(
          UptimeReportRequest request)
        {
            UptimeReportResponse uptimeReportResponse1 = new UptimeReportResponse();
            UptimeReportResponse uptimeReportAsync;
            try
            {
                uptimeReportAsync = await UptimeReportGenerator?.GenerateUptimeReportAsync(request);
            }
            catch (Exception ex)
            {
                UptimeReportResponse uptimeReportResponse2 = new UptimeReportResponse();
                uptimeReportResponse2.AppID = request.AppID;
                uptimeReportResponse2.AppName = request.AppName;
                uptimeReportResponse2.RequestID = request.MessageID;
                uptimeReportResponse2.SessionID = request.SessionID;
                uptimeReportResponse2.MessageDateTime = DateTime.Now;
                uptimeReportResponse2.MessageID = Guid.NewGuid().ToString();
                uptimeReportResponse2.IsSuccess = false;
                uptimeReportResponse2.PublicErrorCode = "500";
                uptimeReportResponse2.PublicErrorMessage = "Server error. Contact Administrator.";
                uptimeReportResponse2.ServerErrorCode = string.Format("{0:#}", 500);
                uptimeReportResponse2.ServerErrorMessage = ex.MessageString();
                uptimeReportAsync = uptimeReportResponse2;
            }
            return uptimeReportAsync;
        }
    }
}
