namespace CashSwift.Library.Standard.Logging
{
    public interface ICashSwiftWebLogger
    {
        void Debug(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Error(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Fatal(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Info(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Trace(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Warning(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);
    }
}
