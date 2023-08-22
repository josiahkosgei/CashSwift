
// Type: CashSwiftUtil.Logging.ICashSwiftLogger
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System;

namespace CashSwiftUtil.Logging
{
    public interface ICashSwiftLogger
    {
        void TraceFormat(UtilLogEntry logEntry);

        void TraceFormat(string format, params object[] args);

        void DebugFormat(UtilLogEntry logEntry);

        void DebugFormat(string format, params object[] args);

        void InfoFormat(UtilLogEntry logEntry);

        void InfoFormat(string format, params object[] args);

        void WarnFormat(UtilLogEntry logEntry);

        void WarnFormat(string format, params object[] args);

        void ErrorFormat(UtilLogEntry logEntry);

        void ErrorFormat(Exception exception);

        void FatalFormat(UtilLogEntry logEntry);

        void FatalFormat(string format, params object[] args);

        bool IsTraceEnabled { get; }

        bool IsDebugEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarnEnabled { get; }
    }
}
