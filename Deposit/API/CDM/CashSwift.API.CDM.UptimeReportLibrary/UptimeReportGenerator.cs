using CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging;
using CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Reports;
using CashSwift.API.CDM.Reporting.Uptime.DataAccess;
using CashSwift.API.CDM.Reporting.Uptime.DataAccess.Models;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftUtil.Reporting.MSExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CashSwift.API.CDM.UptimeReportLibrary
{
    public class UptimeReportGenerator : IUptimeReportGenerator
    {
        private IUptimeReportDataAccess DataAccess { get; set; }

        private IUptimeReportConfiguration Configuration { get; set; }

        public UptimeReportGenerator(
          IUptimeReportConfiguration config,
          IUptimeReportDataAccess dataAccess)
        {
            Configuration = config;
            DataAccess = dataAccess;
        }

        public async Task<UptimeReportResponse> GenerateUptimeReportAsync(
          UptimeReportRequest request)
        {
            UptimeReportResponse uptimeReportResponse1 = new UptimeReportResponse();
            UptimeReportResponse uptimeReportAsync1;
            try
            {
                Device Device = await DataAccess.GetDevice(request.Device) ?? throw new NullReferenceException(string.Format("Device '{0}' not found", request?.Device));
                DateTime fromDate = request.FromDate.Date;
                DateTime toDate = request.ToDate.Date;
                double totalDays = (toDate - fromDate).TotalDays;
                if (fromDate > toDate)
                    throw new ArgumentException(string.Format("fromDate of {0:yyyy-MM-dd HH:mm:ss.fff ZZ} is after toDate of {1:yyyy-MM-dd HH:mm:ss.fff ZZ}", fromDate, toDate));
                List<UptimeMode> uptimeModesByRange = await DataAccess.GetUptimeModesByRange(request.Device, request.FromDate, request.ToDate);
                List<UptimeModeModel> UptimeModes = new List<UptimeModeModel>(uptimeModesByRange.Count + 10);
                foreach (UptimeMode uptimeMode in uptimeModesByRange)
                {
                    uptimeMode.start_date = uptimeMode.start_date.Clamp(fromDate, toDate);
                    DateTime dateTime1 = (uptimeMode.end_date ?? toDate).Clamp(fromDate, toDate);
                    DateTime dateTime2;
                    for (DateTime dateTime3 = uptimeMode.start_date; dateTime3 < dateTime1; dateTime3 = dateTime2)
                    {
                        dateTime2 = dateTime3.Date.AddDays(1.0);
                        UptimeModes.Add(new UptimeModeModel()
                        {
                            device_mode = (UptimeModeType)uptimeMode.device_mode,
                            start_date = dateTime3,
                            end_date = dateTime1 < dateTime2 ? dateTime1 : dateTime2
                        });
                    }
                }
                IEnumerable<UptimeSummary> UptimeSummaries = UptimeModes.GroupBy(x => new
                {
                    DayOfYear = x.start_date.DayOfYear
                }).Select(g => new UptimeSummary()
                {
                    Start = new DateTime(fromDate.Year, 1, 1).AddDays(g.Key.DayOfYear - 1),
                    End = new DateTime(fromDate.Year, 1, 1).AddDays(g.Key.DayOfYear),
                    ACTIVE = TimeSpan.FromMilliseconds(g.Where(f => f.device_mode == UptimeModeType.ACTIVE).Sum(a => a.duration.TotalMilliseconds)),
                    ADMIN = TimeSpan.FromMilliseconds(g.Where(f => f.device_mode == UptimeModeType.ADMIN).Sum(a => a.duration.TotalMilliseconds)),
                    CIT = TimeSpan.FromMilliseconds(g.Where(f => f.device_mode == UptimeModeType.CIT).Sum(a => a.duration.TotalMilliseconds)),
                    DEVICE_LOCKED = TimeSpan.FromMilliseconds(g.Where(f => f.device_mode == UptimeModeType.DEVICE_LOCKED).Sum(a => a.duration.TotalMilliseconds)),
                    OUT_OF_ORDER = TimeSpan.FromMilliseconds(g.Where(f => f.device_mode == UptimeModeType.OUT_OF_ORDER).Sum(a => a.duration.TotalMilliseconds))
                });
                List<UptimeComponentState> componentsByRange = await DataAccess.GetDeviceComponentsByRange(request.Device, request.FromDate, request.ToDate);
                List<ComponentModel> source = new List<ComponentModel>(componentsByRange.Count + 10);
                foreach (UptimeComponentState uptimeComponentState in componentsByRange)
                {
                    uptimeComponentState.start_date = uptimeComponentState.start_date.Clamp(fromDate, toDate);
                    DateTime dateTime4 = (uptimeComponentState.end_date ?? toDate).Clamp(fromDate, toDate);
                    DateTime dateTime5;
                    for (DateTime dateTime6 = uptimeComponentState.start_date; dateTime6 < dateTime4; dateTime6 = dateTime5)
                    {
                        dateTime5 = dateTime6.Date.AddDays(1.0);
                        source.Add(new ComponentModel()
                        {
                            component_state = (CashSwiftDeviceState)uptimeComponentState.component_state,
                            start_date = dateTime6,
                            end_date = dateTime4 < dateTime5 ? dateTime4 : dateTime5
                        });
                    }
                }
                UptimeReport UptimeReport = new UptimeReport()
                {
                    DeviceSummary = new DeviceSummary()
                    {
                        Device_Location = Device.device_location,
                        Device_Name = Device.name,
                        Device_Number = Device.device_number
                    },
                    ModeData = UptimeModes.OrderBy(x => x.start_date).ToArray(),
                    ComponentData = source.OrderBy(x => x.start_date).ToArray(),
                    UptimeReportSummary = new UptimeReportSummary()
                    {
                        StartDate = fromDate,
                        EndDate = toDate,
                        DeviceName = Device.name,
                        DeviceLocation = Device.device_location,
                        DeviceNumber = Device.device_number,
                        UptimeSummary = UptimeSummaries.ToArray()
                    }
                };
                string path = string.Format(request.UptimeReportPathFormat ?? Configuration.UptimeReportPathFormat, DateTime.Now, fromDate, toDate);
                string templatePath = request.UptimeReportTempatePath ?? Configuration.UptimeReportTempatePath;
                UptimeReportResponse uptimeReportAsync2 = new UptimeReportResponse();
                uptimeReportAsync2.ReportBytes = ExcelManager.GenerateUptimeReportExcelAttachment(UptimeReport, templatePath, path);
                uptimeReportAsync2.FileName = Path.GetFileName(path);
                uptimeReportAsync2.SessionID = request.SessionID;
                uptimeReportAsync2.MessageID = Guid.NewGuid().ToString().ToUpper();
                uptimeReportAsync2.AppID = request.AppID;
                uptimeReportAsync2.AppName = request.AppName;
                uptimeReportAsync2.RequestID = request.MessageID;
                uptimeReportAsync2.MessageDateTime = DateTime.Now;
                uptimeReportAsync2.IsSuccess = true;
                uptimeReportAsync2.PublicErrorCode = null;
                uptimeReportAsync2.PublicErrorMessage = null;
                uptimeReportAsync2.ServerErrorCode = null;
                uptimeReportAsync2.ServerErrorMessage = null;
                return uptimeReportAsync2;
            }
            catch (Exception ex)
            {
                UptimeReportResponse uptimeReportResponse2 = new UptimeReportResponse();
                uptimeReportResponse2.SessionID = request.SessionID;
                uptimeReportResponse2.MessageID = Guid.NewGuid().ToString().ToUpper();
                uptimeReportResponse2.AppID = request.AppID;
                uptimeReportResponse2.AppName = request.AppName;
                uptimeReportResponse2.RequestID = request.MessageID;
                uptimeReportResponse2.MessageDateTime = DateTime.Now;
                uptimeReportResponse2.IsSuccess = false;
                uptimeReportResponse2.PublicErrorCode = "500";
                uptimeReportResponse2.PublicErrorMessage = "Server error. Contact Administrator";
                uptimeReportResponse2.ServerErrorCode = "500";
                uptimeReportResponse2.ServerErrorMessage = ex.MessageString();
                uptimeReportAsync1 = uptimeReportResponse2;
            }
            return uptimeReportAsync1;
        }
    }
}
