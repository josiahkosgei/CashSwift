using Caliburn.Micro;
using CashSwift.Library.Standard.Logging;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.Properties;
using CashSwiftDeposit.Utils;
using CashSwiftDeposit.Utils.AlertClasses;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace CashSwiftDeposit.ViewModels
{
    public class StartupViewModel : Conductor<object>
    {
        private DispatcherTimer StartupTimer = new DispatcherTimer(DispatcherPriority.Send);
        private DispatcherTimer OutOfOrderTimer = new DispatcherTimer(DispatcherPriority.Send);

        public CashSwiftLogger Log { get; set; }

        public AlertManager AlertManager { get; set; }

        private ApplicationViewModel ApplicationViewModel { get; set; }

        public StartupViewModel()
        {
            Log = new CashSwiftLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), "CashSwiftDepositLog", null);
            using (new DepositorDBContext())
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CrashHandler);
                ApplicationViewModel.DeviceConfiguration = DeviceConfiguration.Initialise();
                ActivateItemAsync(new StartupImageViewModel());
                StartupTimer.Interval = TimeSpan.FromSeconds(10.0);
                StartupTimer.Tick += new EventHandler(StartupTimer_Tick);
                StartupTimer.IsEnabled = true;
                OutOfOrderTimer.Interval = TimeSpan.FromSeconds(11.0);
                OutOfOrderTimer.Tick += new EventHandler(OutOfOrderTimer_Tick);
                OutOfOrderTimer.IsEnabled = true;
                WebAPI_StartOWINHost();
            }
        }

        public void WebAPI_StartOWINHost()
        {
            try
            {
                string str = Settings.Default.OWIN_BASE_ADDRESS ?? "http://localhost:9000/";
                Log.Info(nameof(StartupViewModel), "OWIN Start", nameof(WebAPI_StartOWINHost), "Starting server at {0}", new object[1] { str });
                DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
                if (deviceConfiguration != null && deviceConfiguration.ALLOW_WEB_SERVER)
                    WebApp.Start<Startup>(str);
                TestCashSwiftOWIN(str);
            }
            catch (Exception ex)
            {
                Log.Error(nameof(StartupViewModel), "OWIN Start", nameof(WebAPI_StartOWINHost), "Error starting OWIN server: {0}", new object[1] { ex.MessageString() });
            }
        }

        private static void TestCashSwiftOWIN(string baseAddress)
        {
        }

        private void StartupTimer_Tick(object sender, EventArgs e)
        {
            StartupTimer.Stop();
            StartupTimer = null;
            ChangeActiveItemAsync(new OutOfOrderFatalScreenViewModel(), true);
        }
        private void OutOfOrderTimer_Tick(object sender, EventArgs e)
        {
            OutOfOrderTimer.Stop();
            OutOfOrderTimer = null;
            using (DepositorDBContext depositorDBContext = new DepositorDBContext())
            {
                try
                {
                    Device device = GetDevice(depositorDBContext);
                    try
                    {
                        DeviceConfiguration deviceConfiguration = DeviceConfiguration.Initialise();
                        AlertManager = new AlertManager(new DepositorLogger(null), deviceConfiguration.API_COMMSERV_URI, device.app_id, device.app_key, device.machine_name);
                        ApplicationViewModel = new ApplicationViewModel(this);
                        ChangeActiveItemAsync(ApplicationViewModel, closePrevious: true);
                    }
                    catch (Exception ex)
                    {
                        ActivateItemAsync(new OutOfOrderFatalScreenViewModel());
                        AlertManager?.SendAlert(new AlertDeviceStartupFailed(ex?.Message, device, DateTime.Now));
                        CrashHandler(this, new UnhandledExceptionEventArgs(ex, isTerminating: false));
                        DeviceStatus deviceStatus = depositorDBContext?.DeviceStatus.FirstOrDefault((DeviceStatus x) => x.device_id == device.id);
                        if (deviceStatus == null)
                        {
                            deviceStatus = CashSwiftDepositCommonClasses.GenerateDeviceStatus(device.id, depositorDBContext);
                            depositorDBContext.DeviceStatus.Add(deviceStatus);
                        }
                        deviceStatus.current_status |= 1024;
                        deviceStatus.modified = DateTime.Now;
                    }
                }
                catch (Exception exception)
                {
                    CrashHandler(this, new UnhandledExceptionEventArgs(exception, isTerminating: false));
                }
                SaveToDatabase(depositorDBContext);
            }
        }
        private void CrashHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            try
            {
                Console.WriteLine($"Crash Handler caught : {ex.MessageString()}");
                Log.Fatal("StartupViewModel", ApplicationErrorConst.ERROR_CRASH.ToString(), "CrashHandler", "Crash Handler caught : {0}", ex.MessageString());
                using (DepositorDBContext dBContext = new DepositorDBContext())
                {
                    Device device = GetDevice(dBContext);
                    SaveToDatabase(dBContext);
                    AlertManager?.SendAlert(new AlertApplicationCrash(device, ex.Message, DateTime.Now, ex.StackTrace));
                }
            }
            catch (Exception ex2)
            {
                try
                {
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\[" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.fff") + "] Crash.log", $"Crash Handler caught : {ex2.MessageString()}>>{ex.MessageString()}");
                }
                catch (Exception)
                {
                }
            }
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            Application.Current.Shutdown();
        }
        public void SaveToDatabase(DepositorDBContext DBContext)
        {
            try
            {
                ApplicationViewModel.SaveToDatabase(DBContext);
            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
                List<ICollection<DbValidationError>> dbValidationErrorsList;
                if (ex == null)
                {
                    dbValidationErrorsList = null;
                }
                else
                {
                    IEnumerable<DbEntityValidationResult> validationErrors = ex.EntityValidationErrors;
                    if (validationErrors == null)
                    {
                        dbValidationErrorsList = null;
                    }
                    else
                    {
                        IEnumerable<ICollection<DbValidationError>> source = validationErrors.Select(x => x.ValidationErrors);
                        dbValidationErrorsList = source != null ? source.ToList() : null;
                    }
                }
                string str = "";
                foreach (ICollection<DbValidationError> dbValidationErrors in dbValidationErrorsList)
                    str = str + "," + dbValidationErrors?.ToString();
                Log.Error(nameof(StartupViewModel), ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), nameof(SaveToDatabase), "Error Saving to Database: {0}>>{1}", new object[2]
                {
           ex.MessageString(),
           str
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
                Log.Error(nameof(StartupViewModel), ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), nameof(SaveToDatabase), "Error Saving to Database: {0}", new object[1]
                {
           ex.MessageString()
                });
            }
        }

        private Device GetDevice(DepositorDBContext DBContext)
        {
            return DBContext.Devices.FirstOrDefault(f => f.machine_name == Environment.MachineName) ?? throw new Exception("Device not set correctly in database. Device is null during start up.");
        }
    }
}
