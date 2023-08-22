
// Type: CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging.UptimeModeInstance
// Assembly: CashSwift.API.CDM.Messaging.Reporting.Uptime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 83FFEB48-B9D4-447C-8A31-3FCEEA43D28E
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.CDM.Messaging.Reporting.Uptime.dll

using CashSwift.Library.Standard.Statuses;
using System;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging
{
    public class UptimeModeInstance
    {
        public Guid id { get; set; }

        public Guid device { get; set; }

        public DateTime created { get; set; }

        public DateTime start_date { get; set; }

        public DateTime? end_date { get; set; }

        public UptimeModeType device_mode { get; set; }
    }
}
