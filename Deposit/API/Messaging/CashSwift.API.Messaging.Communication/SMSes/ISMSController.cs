﻿
// Type: CashSwift.API.Messaging.Communication.SMSes.ISMSController
// Assembly: CashSwift.API.Messaging.Communication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F45642AD-C4B4-4961-9A77-6FFE525EEEC0
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.Messaging.Communication.dll

using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Communication.SMSes
{
    public interface ISMSController
    {
        Task<SMSResponse> SendSMSAsync(SMSRequest request);
    }
}
