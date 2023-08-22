using CashSwift.API.CDM.Reporting.Uptime.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashSwift.API.CDM.Reporting.Uptime.DataAccess
{
    public interface IUptimeReportDataAccess
    {
        string ConnectionString { get; set; }

        Task<Device> GetDevice(Guid id);

        Task<List<UptimeMode>> GetUptimeModesByRange(
          Guid device,
          DateTime fromDate,
          DateTime toDate);

        Task<List<UptimeComponentState>> GetDeviceComponentsByRange(
          Guid device,
          DateTime fromDate,
          DateTime toDate);
    }
}
