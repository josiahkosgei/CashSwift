﻿using System;

namespace CashSwift.API.CDM.Reporting.Uptime.DataAccess.Models
{
    public class UptimeComponentState
    {
        public Guid id { get; set; }

        public Guid device { get; set; }

        public DateTime created { get; set; }

        public DateTime start_date { get; set; }

        public DateTime? end_date { get; set; }

        public int component_state { get; set; }
    }
}
