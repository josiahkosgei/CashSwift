
// Type: CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports.UptimeSummary
// Assembly: CashSwift.API.CDM.Messaging.Reporting.Uptime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 83FFEB48-B9D4-447C-8A31-3FCEEA43D28E
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.CDM.Messaging.Reporting.Uptime.dll

using System;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports
{
    public class UptimeSummary
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double Uptime
        {
            get
            {
                TimeSpan timeSpan = ACTIVE + ADMIN + CIT;
                double totalSeconds1 = timeSpan.TotalSeconds;
                timeSpan = TOTAL;
                double totalSeconds2 = timeSpan.TotalSeconds;
                return totalSeconds1 / totalSeconds2;
            }
        }

        public TimeSpan ACTIVE { get; set; }

        public TimeSpan ADMIN { get; set; }

        public TimeSpan CIT { get; set; }

        public TimeSpan OUT_OF_ORDER { get; set; }

        public TimeSpan DEVICE_LOCKED { get; set; }

        public TimeSpan TOTAL => ACTIVE + ADMIN + OUT_OF_ORDER + CIT + DEVICE_LOCKED;
    }
}
