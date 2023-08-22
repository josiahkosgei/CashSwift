
// Type: CashSwiftUtil.Logging.UtilLogEntry
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System;

namespace CashSwiftUtil.Logging
{
    public class UtilLogEntry
    {
        public const string format = "\u0002{5}|{0:yyyy-MM-dd HH:mm:ss.ffff zzzz}|{1}|{3}|{4}|{2}\u0003";
        public string component;
        public string eventName;
        public string eventType;
        public string eventDetail;
        public UtilLoggingLevel level;
        public DateTime date = DateTime.Now;

        public UtilLogEntry(
          string Component,
          string EventName,
          string EventType,
          string EventDetail,
          UtilLoggingLevel Level)
        {
            level = Level;
            component = Component;
            eventName = EventName;
            eventType = EventType;
            eventDetail = EventDetail;
        }
    }
}
