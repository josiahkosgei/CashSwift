using CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports;
using CashSwift.API.Messaging;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging
{
    public class UptimeReportResponse : APIResponseBase
    {
        public UptimeReport UptimeReport { get; set; }

        public byte[] ReportBytes { get; set; }

        public string FileName { get; set; }
    }
}
