
// Type: CashSwiftUtil.DataTypes.Auth.APIUser
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System;

namespace CashSwiftUtil.DataTypes.Auth
{
    public class APIUser
    {
        public Guid id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public Guid AppId { get; set; }

        public string AppKey { get; set; }
    }
}
