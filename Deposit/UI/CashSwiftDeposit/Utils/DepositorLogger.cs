using CashSwift.Library.Standard.Logging;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels;
using CashSwiftUtil.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace CashSwiftDeposit.Utils
{
    public class DepositorLogger : CashSwiftLogger
    {
        private CashSwiftLogger _innerLogger;

        private ApplicationViewModel ApplicationViewModel { get; }

        public DepositorLogger(ApplicationViewModel applicationViewModel, string LoggerName = "CashSwiftDepositLog")
          : base(Assembly.GetCallingAssembly().GetName().Version.ToString(), LoggerName, null)
        {
            if (string.IsNullOrWhiteSpace(LoggerName))
                LoggerName = "CashSwiftDepositLog";
            ApplicationViewModel = applicationViewModel;
            _innerLogger = new CashSwiftLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), LoggerName, null);
        }

        public void Trace(string Component, string EventName, string EventType, string EventDetail) => Log(Component, EventName, EventType, UtilLoggingLevel.TRACE, EventDetail);

        public void TraceFormat(
          string Component,
          string EventName,
          string EventType,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects)
        {
            LogFormat(Component, EventName, EventType, UtilLoggingLevel.TRACE, EventDetailFormat, EventDetailFormatObjects);
        }

        public void Debug(string Component, string EventName, string EventType, string EventDetail) => Log(Component, EventName, EventType, UtilLoggingLevel.DEBUG, EventDetail);

        public void DebugFormat(
          string Component,
          string EventName,
          string EventType,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects)
        {
            LogFormat(Component, EventName, EventType, UtilLoggingLevel.DEBUG, EventDetailFormat, EventDetailFormatObjects);
        }

        public void Info(string Component, string EventName, string EventType, string EventDetail) => Log(Component, EventName, EventType, UtilLoggingLevel.INFO, EventDetail);

        public void InfoFormat(
          string Component,
          string EventName,
          string EventType,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects)
        {
            LogFormat(Component, EventName, EventType, UtilLoggingLevel.INFO, EventDetailFormat, EventDetailFormatObjects);
        }

        public void Warning(string Component, string EventName, string EventType, string EventDetail) => Log(Component, EventName, EventType, UtilLoggingLevel.WARN, EventDetail);

        public void WarningFormat(
          string Component,
          string EventName,
          string EventType,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects)
        {
            LogFormat(Component, EventName, EventType, UtilLoggingLevel.WARN, EventDetailFormat, EventDetailFormatObjects);
        }

        public void Error(string Component, int Code, string ErrorName, string ErrorDetail) => Log(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.ERROR, ErrorDetail);

        public void ErrorFormat(
          string Component,
          int Code,
          string ErrorName,
          string ErrorMessageFormat,
          params object[] ErrorMessageFormatObjects)
        {
            LogFormat(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.ERROR, ErrorMessageFormat, ErrorMessageFormatObjects);
        }

        public void Fatal(string Component, int Code, string ErrorName, string ErrorDetail) => Log(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.FATAL, ErrorDetail);

        public void FatalFormat(string Component, int Code, string ErrorName, string ErrorMessageFormat, params object[] ErrorMessageFormatObjects)
        {
            LogFormat(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.FATAL, ErrorMessageFormat, ErrorMessageFormatObjects);
            string.Format(ErrorMessageFormat, ErrorMessageFormatObjects);
        }

        private void Log(string Component, string EventName, string EventType, UtilLoggingLevel Level, string EventDetail)
        {
            LogFormat(Component, EventName, EventType, Level, "{0}", EventDetail);
        }

        public void LogFormat(string Component, string EventName, string EventType, UtilLoggingLevel Level, string EventDetailFormat, params object[] EventDetailFormatObjects)
        {
            try
            {
                switch (Level)
                {
                    case UtilLoggingLevel.TRACE:
                        _innerLogger.Trace(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.DEBUG:
                        _innerLogger.Debug(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.INFO:
                        _innerLogger.Info(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.WARN:
                        _innerLogger.Warning(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.ERROR:
                        _innerLogger.Error(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.FATAL:
                        _innerLogger.Fatal(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            string str = string.Format(EventDetailFormat, EventDetailFormatObjects);
            if (string.IsNullOrEmpty(str) || Level < (UtilLoggingLevel)ApplicationViewModel.DeviceConfiguration.LOGGING_LEVEL)
                return;
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                try
                {
                    ApplicationLog applicationLog = new ApplicationLog();
                    applicationLog.id = GuidExt.UuidCreateSequential();
                    applicationLog.log_date = DateTime.Now;
                    applicationLog.component = Component;
                    applicationLog.device_id = DBContext.Devices.FirstOrDefault(f => f.machine_name == Environment.MachineName).id;
                    applicationLog.session_id = (ApplicationViewModel?.SessionID);
                    applicationLog.event_detail = str;
                    applicationLog.event_name = EventName;
                    applicationLog.event_type = EventType;
                    applicationLog.log_level = (int)Level;
                    applicationLog.machine_name = Environment.MachineName;
                    ApplicationLog entity = applicationLog;
                    DBContext.ApplicationLogs.Add(entity);
                    ApplicationViewModel.SaveToDatabase(DBContext);
                }
                catch (Exception ex)
                {
                    _innerLogger.Warning(GetType().Name, "Logging Failed", "LogEvent", string.Format("{0}>>{1}>>{2}>stack>{3}", ex.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message, ex.StackTrace), Array.Empty<object>());
                }
            }
        }
    }
}
