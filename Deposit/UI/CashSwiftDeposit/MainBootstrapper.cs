using Caliburn.Micro;
using CashSwiftDeposit.Properties;
using CashSwiftDeposit.Utils;
using CashSwiftDeposit.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CashSwiftDeposit
{
    internal class MainBootstrapper : BootstrapperBase
    {
        public MainBootstrapper()
          : base()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(UtilExtentionMethods.PasswordBoxHelper.BoundPasswordProperty, "Password", "PasswordChanged");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(assemblyFolder + "\\NLog.config");

            Dictionary<string, object> settings = new Dictionary<string, object>();
            settings.Add("WindowStartupLocation", WindowStartupLocation.CenterScreen);
            settings.Add("Title", "CashSwift Deposit");
            settings.Add("WindowState", WindowState.Maximized);
            settings.Add("WindowStyle", WindowStyle.None);
            settings.Add("Topmost", Settings.Default.GUI_ALWAYS_ON_TOP);
            if (!Settings.Default.GUI_SHOW_MOUSE_CURSOR)
                settings.Add("Cursor", Cursors.None);
            ViewLocator.NameTransformer.AddRule("^CashSwiftDeposit.ViewModels.*ATMScreenCommandViewModel", "CashSwiftDeposit.Views.WaitForProcessScreenView");
            ViewLocator.NameTransformer.AddRule("^CashSwiftDeposit.ViewModels.*InputScreenViewModel", "CashSwiftDeposit.Views.CustomerInputScreenView");
            ViewLocator.NameTransformer.AddRule("^CashSwiftDeposit.ViewModels.*ListScreenViewModel", "CashSwiftDeposit.Views.CustomerListScreenView");
            ViewLocator.NameTransformer.AddRule("^CashSwiftDeposit.ViewModels.*VerifyDetailsScreenViewModel", "CashSwiftDeposit.Views.CustomerVerifyDetailsScreenView");
            ViewLocator.NameTransformer.AddRule("^CashSwiftDeposit.ViewModels.*FormViewModel", "CashSwiftDeposit.Views.FormScreenView");
            ViewLocator.NameTransformer.AddRule("^CashSwiftDeposit.ViewModels.*ATMViewModel", "CashSwiftDeposit.Views.ATMScreenView");
            ViewLocator.NameTransformer.AddRule("^CashSwiftDeposit.ViewModels.*DialogueBoxViewModel", "CashSwiftDeposit.Views.DialogueBoxView");
            ViewLocator.NameTransformer.AddRule("^CashSwiftDeposit.ViewModels.*SearchScreenViewModel", "CashSwiftDeposit.Views.CustomerSearchScreenView");
            DisplayRootViewForAsync<StartupViewModel>(settings);
        }
    }
}
