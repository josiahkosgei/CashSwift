using CashSwift.API.Messaging;
using System;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging
{
    public class UptimeReportRequest : APIRequestBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Guid Device { get; set; }

        public string UptimeReportPathFormat { get; set; }

        public string UptimeReportTempatePath { get; set; }
    }
}
