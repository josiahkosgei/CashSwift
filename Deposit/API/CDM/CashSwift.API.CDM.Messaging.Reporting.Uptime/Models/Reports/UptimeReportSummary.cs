
// Type: CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports.UptimeReportSummary
// Assembly: CashSwift.API.CDM.Messaging.Reporting.Uptime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 83FFEB48-B9D4-447C-8A31-3FCEEA43D28E
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.CDM.Messaging.Reporting.Uptime.dll

using System;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports
{
    public class UptimeReportSummary
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Days => (EndDate - StartDate).TotalDays;

        public string DeviceName { get; set; }

        public string DeviceLocation { get; set; }

        public string DeviceNumber { get; set; }

        public UptimeSummary[] UptimeSummary { get; set; }
    }
}
