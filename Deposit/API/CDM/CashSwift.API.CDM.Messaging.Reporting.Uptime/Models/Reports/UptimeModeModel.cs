
// Type: CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports.UptimeModeModel
// Assembly: CashSwift.API.CDM.Messaging.Reporting.Uptime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 83FFEB48-B9D4-447C-8A31-3FCEEA43D28E
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.CDM.Messaging.Reporting.Uptime.dll

using CashSwift.Library.Standard.Statuses;
using System;
using System.ComponentModel;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports
{
    public class UptimeModeModel
    {
        [Description("Device Mode")]
        public UptimeModeType device_mode { get; set; }

        [Description("Axis")]
        public int axis => (int)device_mode;

        [Description("Start Date")]
        public DateTime start_date { get; set; }

        [Description("Duration")]
        public TimeSpan duration => end_date - start_date;

        [Description("End Date")]
        public DateTime end_date { get; set; }
    }
}
