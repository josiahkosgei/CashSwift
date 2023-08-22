using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Linq;

namespace CashSwift.Library.Standard.Logging
{
    public class CashSwiftLogger : ICashSwiftLogger
    {
        private ILogger _logger;

        private string DateTimeFormat { get; set; }

        public CashSwiftLogger(string version, string name, IConfiguration configuration)
        {
            _logger = LogManager.GetLogger(name);
            DateTimeFormat = configuration?["Logging.DateTimeFormat"] ?? "yyyy-MM-dd HH:mm:ss.fff";
        }

        public void Trace(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsTraceEnabled)
                return;
            _logger.Trace(string.Format("\u0002{0,-5}|{1}|{2}|{3}|{4}|{5}\u0003", LogLevel.Trace.ToString().ToUpper(), DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Debug(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsDebugEnabled)
                return;
            _logger.Debug(string.Format("\u0002{0,-5}|{1}|{2}|{3}|{4}|{5}\u0003", LogLevel.Debug.ToString().ToUpper(), DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Info(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsInfoEnabled)
                return;
            _logger.Info(string.Format("\u0002{0,-5}|{1}|{2}|{3}|{4}|{5}\u0003", LogLevel.Info.ToString().ToUpper(), DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Warning(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsWarnEnabled)
                return;
            _logger.Warn(string.Format("\u0002{0,-5}|{1}|{2}|{3}|{4}|{5}\u0003", LogLevel.Warn.ToString().ToUpper(), DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Error(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsErrorEnabled)
                return;
            _logger.Error(string.Format("\u0002{0,-5}|{1}|{2}|{3}|{4}|{5}\u0003", LogLevel.Error.ToString().ToUpper(), DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Fatal(
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsFatalEnabled)
                return;
            _logger.Fatal(string.Format("\u0002{0,-5}|{1}|{2}|{3}|{4}|{5}\u0003", LogLevel.Fatal.ToString().ToUpper(), DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }
    }
}
