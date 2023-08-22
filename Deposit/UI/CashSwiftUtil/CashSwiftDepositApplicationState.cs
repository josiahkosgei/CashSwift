
// Type: CashSwiftUtil.CashSwiftDepositApplicationState
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using Caliburn.Micro;

namespace CashSwiftUtil
{
    public class CashSwiftDepositApplicationState : PropertyChangedBase
    {
        public bool HasPrinterError { get; set; }

        public bool HasDatabaseError { get; set; }

        public bool HasFileSystemError { get; set; }

        public bool HasDeviceError { get; set; }

        public bool HasServerError { get; set; }
    }
}
