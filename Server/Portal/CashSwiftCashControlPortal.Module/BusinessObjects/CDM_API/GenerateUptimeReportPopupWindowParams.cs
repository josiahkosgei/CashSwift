using DevExpress.ExpressApp.DC;
using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.CDM_API
{
    [DomainComponent]
    public class GenerateUptimeReportPopupWindowParams
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UptimeReportTempatePath { get; set; }

        public string UptimeReportPathFormat { get; set; }

        public string ReportSavePath { get; set; }

        public string CDM_URL { get; set; }

        public GenerateUptimeReportPopupWindowParams() : base()
        {
            StartDate = DateTime.Now.AddDays(-1.0);
            EndDate = DateTime.Now.Date;
            UptimeReportPathFormat = "c:\\Deposit\\Reports\\UptimeReport\\{0:yyyyMMddTHHmmss}_Uptime_{1:yyyyMMdd}_{2:yyyyMMdd}.xlsx";
            ReportSavePath = "c:\\Server\\Reports\\UptimeReport\\{0}\\{1}";
        }
    }
}
