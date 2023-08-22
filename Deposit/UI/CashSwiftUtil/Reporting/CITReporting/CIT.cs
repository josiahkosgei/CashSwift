
// Type: CashSwiftUtil.Reporting.CITReporting.CIT
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using CashSwiftUtil.Reporting.MSExcel;
using System;

namespace CashSwiftUtil.Reporting.CITReporting
{
    public class CIT
    {
        public string device { get; set; }

        public DateTime cit_date { get; set; }

        public DateTime? cit_complete_date { get; set; }

        public string InitiatingUser { get; set; }

        public string AuthorisingUser { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string OldBagNumber { get; set; }

        public string NewBagNumber { get; set; }

        public string SealNumber { get; set; }

        public bool Complete { get; set; }

        public string Error { get; set; }

        public string ErrorMessage { get; set; }

        [EpplusIgnore]
        public Guid id { get; set; }

        [EpplusIgnore]
        public Guid device_id { get; set; }
    }
}
