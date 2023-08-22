
// Type: CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports.UptimeReport
// Assembly: CashSwift.API.CDM.Messaging.Reporting.Uptime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 83FFEB48-B9D4-447C-8A31-3FCEEA43D28E
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.CDM.Messaging.Reporting.Uptime.dll

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports
{
    public class UptimeReport
    {
        public DeviceSummary DeviceSummary { get; set; }

        public UptimeReportSummary UptimeReportSummary { get; set; }

        public UptimeModeModel[] ModeData { get; set; }

        public ComponentModel[] ComponentData { get; set; }
    }
}
