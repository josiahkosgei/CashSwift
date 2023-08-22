
// Type: CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports.ComponentModelSummary
// Assembly: CashSwift.API.CDM.Messaging.Reporting.Uptime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 83FFEB48-B9D4-447C-8A31-3FCEEA43D28E
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.CDM.Messaging.Reporting.Uptime.dll

using System;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports
{
    public class ComponentModelSummary
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public TimeSpan ACTIVE { get; set; }

        public TimeSpan ADMIN { get; set; }

        public TimeSpan CIT { get; set; }

        public TimeSpan OUT_OF_ORDER { get; set; }

        public TimeSpan DEVICE_LOCKED { get; set; }
    }
}
