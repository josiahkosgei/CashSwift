using CashSwift.Library.Standard.Statuses;
using System;

namespace CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging
{
    public class UptimeComponentStateInstance
    {
        public Guid id { get; set; }

        public Guid device { get; set; }

        public DateTime created { get; set; }

        public DateTime start_date { get; set; }

        public DateTime? end_date { get; set; }

        public CashSwiftDeviceState component_state { get; set; }
    }
}
