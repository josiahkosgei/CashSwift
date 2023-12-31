﻿using System;

namespace CashSwift.API.CDM.Reporting.Uptime.DataAccess.Models
{
    public class UptimeMode
    {
        public Guid id { get; set; }

        public Guid device { get; set; }

        public DateTime created { get; set; }

        public DateTime start_date { get; set; }

        public DateTime? end_date { get; set; }

        public int device_mode { get; set; }
    }
}
