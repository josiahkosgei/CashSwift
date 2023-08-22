using System.Globalization;
using System.Threading;
using System;
using System.Windows;
using System.Configuration;

namespace CashSwiftDeposit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            NLog.LogManager.Shutdown();
        }
        public App()
        {
            try
            {
                //NLog.LogManager.Setup().SetupExtensions(s => s.RegisterAssembly("NLog.Config"));
                string appSetting1 = ConfigurationManager.AppSettings["Culture"];
                string appSetting2 = ConfigurationManager.AppSettings["UICulture"];
                if (string.IsNullOrEmpty(appSetting1))
                    return;
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(appSetting1);
                }
                catch (Exception ex)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(appSetting1);
                }
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(appSetting2);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
