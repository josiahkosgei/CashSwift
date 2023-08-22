using CashSwift.Library.Standard.Security;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

namespace CashSwiftDeposit.Models
{
    public class ApplicationModel
    {
        private List<GUIScreen> _dbGUIScreens;
        private List<Currency> _dbcurrencyList;
        private PasswordPolicyItems _passwordPolicy;
        public List<Language> _dblanguageList;
        public List<TransactionTypeListItem> _txTypeList;
        private ApplicationViewModel _applicationViewModel;

        public List<GUIScreen> dbGUIScreens => _dbGUIScreens;

        public List<Currency> CurrencyList => _dbcurrencyList;

        public PasswordPolicyItems PasswordPolicy => _passwordPolicy;

        public List<Language> LanguageList => _dblanguageList;

        public string SplashVideoPath => getSplashVideo();

        public List<TransactionTypeListItem> TransactionList => _txTypeList;

        public ApplicationViewModel ApplicationViewModel => _applicationViewModel;

        public ApplicationModel(ApplicationViewModel ApplicationViewModel) => _applicationViewModel = ApplicationViewModel;

        public bool TestConnection()
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                ApplicationViewModel.Log.Debug(GetType().Name, "Database Connectivity Test", "Initialisation", "Testing Database Connection....");
                DbConnection connection = depositorDbContext.Database.Connection;
                for (int index = 0; index < 10; ++index)
                {
                    ApplicationViewModel.Log.DebugFormat(GetType().Name, "DB Connection Attempt", "Initialisation", "Attempt {0}", index + 1);
                    try
                    {
                        connection.Open();
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, "DB Connect SUCCESS", "Initialisation", "Database Connection OK after {0} tries", index + 1);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ApplicationViewModel.Log.ErrorFormat(GetType().Name, 88, ApplicationErrorConst.ERROR_DATABASE_OFFLINE.ToString(), "Database Connection FAILED: {0}", ex.MessageString());
                    }
                }
                return false;
            }
        }

        internal void InitialiseApplicationModel()
        {
            using (new DepositorDBContext())
            {
                _passwordPolicy = GetPasswordPolicy();
                GenerateTransactionTypeList();
                GenerateCurrencyList();
                GenerateLanguageList();
                GenerateScreenList();
            }
        }

        internal string GetDeviceNumber()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
                return GetDevice(DBContext)?.device_number;
        }
        internal Device GetDevice(DepositorDBContext DBContext)
        {
            try
            {
                Device device = DBContext.Devices.FirstOrDefault(x => x.machine_name == Environment.MachineName);
                if (device != null)
                    return device;
                ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not get device info from database, terminating");
                throw new InvalidOperationException(string.Format("Device with machine name = {0} does not exists in the local database.", Environment.MachineName));
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), ex.MessageString());
                throw;
            }
        }

        internal PasswordPolicyItems GetPasswordPolicy()
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                PasswordPolicy passwordPolicy = depositorDbContext.PasswordPolicies.FirstOrDefault();
                return new PasswordPolicyItems()
                {
                    Lower_Case_length = passwordPolicy.min_lowercase,
                    Minimum_Length = passwordPolicy.min_length,
                    Special_length = passwordPolicy.min_special,
                    Numeric_length = passwordPolicy.min_digits,
                    Upper_Case_length = passwordPolicy.min_uppercase,
                    HistorySize = passwordPolicy.history_size
                };
            }
        }

        internal string getSplashVideo() => "resources/bank.mp4";

        public void GenerateScreenList()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateScreenList), "Initialisation", "Generating Screens List");
                
                var device = DBContext.Devices
                    .Where(x => x.machine_name == Environment.MachineName)
                    .Include(x => x.GUIScreenList.GuiScreenListScreens.Select(d=>d.GUIScreen.GUIScreenType))
                    .Include(x => x.GUIScreenList.GuiScreenListScreens.Select(d=>d.GUIScreen.GUIScreenText))

                    .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.disclaimerNavigation.TransactionTexttermsNavigations))

                    .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.account_name_captionNavigation))

                    .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.account_number_captionNavigation))

                    .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.account_number_captionNavigation))

                    .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.alias_account_name_captionNavigation))



                    .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.depositor_name_captionNavigation))

                    .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.full_instructionsNavigation))


                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.FundsSource_captionNavigation))


                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.id_number_captionNavigation))


                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.listItem_captionNavigation))


                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.narration_captionNavigation))


                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.phone_number_captionNavigation))

                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.receipt_templateNavigation))


                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.reference_account_name_captionNavigation))

                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.reference_account_number_captionNavigation))


                   .Include(x => x.GUIScreenList.TransactionTypeListItems
                         .Select(d => d.TransactionText.termsNavigation))
                   .FirstOrDefault();

                _dbGUIScreens = device.GUIScreenList.GuiScreenListScreens.OrderBy(x => x.screen_order).Select(x => x.GUIScreen).Where(x => x.enabled).ToList();
                if (_dbGUIScreens != null)
                {
                    List<GUIScreen> dbGuiScreens = _dbGUIScreens;

                    if ((dbGuiScreens != null ? (dbGuiScreens.Count == 0 ? 1 : 0) : 0) == 0)
                        return;
                }
                ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not generate screen list from database");
                throw new Exception("Could not generate screen list from database");
            }
        }

        public void GenerateLanguageList()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateLanguageList), "Initialisation", "Generating Language List");
                _dblanguageList = GetDevice(DBContext).LanguageList.LanguageListLanguages.OrderBy(x => x.language_order).Select(x => x.Language).Where(x => x.enabled).ToList();
                if (_dblanguageList != null)
                {
                    List<Language> dblanguageList = _dblanguageList;

                    if ((dblanguageList != null ? (dblanguageList.Count == 0 ? 1 : 0) : 0) == 0)
                        return;
                }
                ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not generate language list from database");
                throw new Exception("Could not generate language list from database");
            }
        }

        public void GenerateCurrencyList()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateCurrencyList), "Initialisation", "Generating Currency List");
                _dbcurrencyList = GetDevice(DBContext).CurrencyList.CurrencyListCurrencies.OrderBy(x => x.currency_order).Select(x => x.Currency).Skip(1).ToList();
                if (_dbcurrencyList != null)
                {
                    List<Currency> dbcurrencyList = _dbcurrencyList;

                    if ((dbcurrencyList != null ? (dbcurrencyList.Count == 0 ? 1 : 0) : 0) == 0)
                        return;
                }
                ApplicationViewModel.Log.Info(GetType().Name, nameof(GenerateCurrencyList), "Initialisation", "No extra currencies available. Using signle currency.");
            }
        }

        public void GenerateTransactionTypeList()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                try
                {
                    ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateTransactionTypeList), "Initialisation", "Generating Transaction Type List");
                    _txTypeList = GetDevice(DBContext).TransactionTypeList.TransactionTypeListTransactionTypeListItems.OrderBy(x => x.list_order).Select(x => x.TransactionTypeListItem).Where(x => (bool)x.enabled).ToList();
                    if (_txTypeList != null)
                    {
                        List<TransactionTypeListItem> txTypeList = _txTypeList;

                        if ((txTypeList != null ? (txTypeList.Count == 0 ? 1 : 0) : 0) == 0)
                            return;
                    }
                    ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not generate transactiontype list from database");
                    throw new Exception("Could not generate transactiontype list from database");
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not generate transactiontype list from database");
                    throw new Exception("Could not generate transactiontype list from database: " + ex?.Message, ex);
                }
            }
        }

        public List<GUIScreen> GetTransactionTypeScreenList(
          TransactionTypeListItem transactionChosen)
        {
            return transactionChosen.GUIScreenList.GuiScreenListScreens.Where(x => x.enabled).OrderBy(x => x.screen_order).Select(x => x.GUIScreen).Where(x => x.enabled).ToList();
        }

        public event EventHandler<EventArgs> DatabaseStorageErrorEvent;

        private void OnDatabaseStorageErrorEvent(object sender, EventArgs e)
        {
            if (DatabaseStorageErrorEvent == null)
                return;
            DatabaseStorageErrorEvent(this, e);
        }
    }
}
