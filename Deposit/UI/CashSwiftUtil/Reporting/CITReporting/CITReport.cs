
// Type: CashSwiftUtil.Reporting.CITReporting.CITReport
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System.Collections.Generic;

namespace CashSwiftUtil.Reporting.CITReporting
{
    public class CITReport
    {
        public CIT CIT { get; set; }

        public List<CITDenomination> CITDenominations { get; set; }

        public IList<Transaction> Transactions { get; set; }

        public IList<EscrowJam> EscrowJams { get; set; }
    }
}
