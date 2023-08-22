
// Type: CashSwift.API.Messaging.Communication.SMSes.SMSMessage
// Assembly: CashSwift.API.Messaging.Communication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F45642AD-C4B4-4961-9A77-6FFE525EEEC0
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.Messaging.Communication.dll

using System.Collections.Generic;

namespace CashSwift.API.Messaging.Communication.SMSes
{
    public class SMSMessage
    {
        public SMSMessage()
        {
            ToContacts = new List<SMSContact>(1);
            FromContacts = new List<SMSContact>(1);
        }

        public List<SMSContact> ToContacts { get; set; }

        public List<SMSContact> FromContacts { get; set; }

        public string Subject { get; set; }

        public string MessageText { get; set; }
    }
}
