
// Type: CashSwiftUtil.Reporting.FailedPostsReporting.FailedPostReport
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System;
using System.Collections.Generic;

namespace CashSwiftUtil.Reporting.FailedPostsReporting
{
    public class FailedPostReport
    {
        public List<FailedTransaction> FailedTransactions { get; set; }

        public DateTime RunDate { get; set; }
    }
}
