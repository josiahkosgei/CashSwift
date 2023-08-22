namespace CashSwift.Library.Standard.Logging
{
    public interface ICashSwiftLogger
    {
        void Debug(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Error(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Fatal(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Info(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Trace(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Warning(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);
    }
}
