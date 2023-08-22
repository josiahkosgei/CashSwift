using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CashSwiftDeposit.ViewModels
{
    internal class CITFormViewModel : FormViewModelBase
    {
        private string _bagNumber;
        private string _sealNumber;

        protected string NewBagNumber
        {
            get => _bagNumber;
            set
            {
                _bagNumber = value;
                NotifyOfPropertyChange("Bagnumber");
            }
        }

        protected string SealNumber
        {
            get => _sealNumber;
            set
            {
                _sealNumber = value;
                NotifyOfPropertyChange(nameof(SealNumber));
            }
        }

        private CIT CIT { get; set; }

        private CITDenomination CITDenominations { get; set; }

        private DateTime thisCITToDate { get; set; }

        private DateTime thisCITFromDate { get; set; }

        private Device Device { get; }

        public CITFormViewModel(
          ApplicationViewModel applicationViewModel,
          ICashSwiftWindowConductor conductor,
          object callingObject,
          bool isNewEntry)
          : base(applicationViewModel, conductor, callingObject, isNewEntry)
        {
            Device = applicationViewModel.ApplicationModel.GetDevice(DBContext);
            ScreenTitle = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(GetType().Name + ".Constructor ScreenTitle", "sys_CITFormScreenTitle", "CIT Operation");
            NextCaption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(GetType().Name + ".Constructor NextCaption", "sys_StartCITCommand_Caption", "Start CIT");
            Fields.Add(new FormListItem()
            {
                DataLabel = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_CIT_NewBagNumber_Caption", "sys_CIT_NewBagNumber_Caption", "New Bag Number"),
                Validate = new Func<string, string>(ValidateBagNumber),
                ValidatedText = NewBagNumber,
                DataTextBoxLabel = NewBagNumber,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_CIT_SealNumber_Caption", "sys_CIT_SealNumber_Caption", "Seal Number"),
                Validate = new Func<string, string>(ValidateSealNumber),
                ValidatedText = SealNumber,
                DataTextBoxLabel = SealNumber,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            ActivateItemAsync(new FormListViewModel(this));
        }

        public string ValidateBagNumber(string newBagNumber)
        {
            if (string.IsNullOrWhiteSpace(newBagNumber))
                return "Please enter a Bag Number";
            NewBagNumber = newBagNumber;
            return null;
        }

        public string ValidateSealNumber(string newSealNumber)
        {
            if (DBContext.CITs.Where(x => x.seal_number == newSealNumber).ToList().Count > 0)
                return "Seal Number must be unique and unused.";
            SealNumber = newSealNumber;
            return null;
        }

        public override string SaveForm()
        {
            int num = FormValidation();
            if (num > 0)
                return string.Format("Form validation failed with {0:0} errors. Kindly correct them and try saving again", num);
            CIT cit1 = createCIT();
            if (cit1 != null)
            {
                CIT cit2 = DBContext.CITs.Add(cit1);
                try
                {
                    ApplicationViewModel.SaveToDatabase(DBContext);
                    ApplicationViewModel.PrintCITReceipt(cit2, DBContext);
                    ApplicationViewModel.StartCIT(SealNumber);
                    ApplicationViewModel.AdminMode = false;
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.Error(GetType().Name, nameof(CITFormViewModel), nameof(SaveForm), "Error saving CIT to database: {0}", new object[1]
                    {
             ex.MessageString()
                    });
                    return ex.Message=="CIT Suspense Account Not Setup" ? "CIT Suspense Account Not Setup" : "Error occurred. Contact administrator.";
                }
            }
            return null;
        }

        public override int FormValidation()
        {
            int num = 0;
            foreach (FormListItem field in Fields)
            {
                Func<string, string> validate = field.Validate;
                string str = validate != null ? validate((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE ? field.DataTextBoxLabel : field.ValidatedText) : null;
                if (str != null)
                {
                    field.ErrorMessageTextBlock = str;
                    ++num;
                }
            }
            return num;
        }

        private CIT createCIT()
        {
            try
            {
                ApplicationViewModel.HandleIncompleteSession();
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.WarningFormat("CITFormViewModel.createCIT()", "Error closing incomplete sessions", "System", "{0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
            }
            CIT lastCit = ApplicationViewModel.lastCIT;
            int num;
            if (lastCit == null)
            {
                num = 1;
            }
            else
            {
                DateTime toDate = lastCit.toDate;
                num = 0;
            }
            thisCITFromDate = num != 0 ? DateTime.MinValue : lastCit.toDate;
            thisCITToDate = DateTime.Now;
            CIT = new CIT()
            {
                id = GuidExt.UuidCreateSequential(),
                cit_date = DateTime.Now,
                start_user = ApplicationViewModel.CurrentUser.id,
                auth_user = new Guid?(ApplicationViewModel.ValidatingUser.id),
                fromDate = new DateTime?(thisCITFromDate),
                toDate = thisCITToDate,
                device_id = Device.id,
                old_bag_number = lastCit?.new_bag_number,
                new_bag_number = NewBagNumber,
                seal_number = SealNumber,
                cit_error = 0,
                complete = false
            };
            DbSet<Transaction> transactions = DBContext.Transactions;
            Expression<Func<Transaction, bool>> predicate = x => x.cit_id == new Guid?() && x.device_id == Device.id && (DateTime?)x.tx_start_date >= CIT.fromDate && x.tx_start_date <= CIT.toDate;
            foreach (Transaction transaction in transactions.Where(predicate).ToList())
                transaction.CIT = CIT;

            /*
             select t.tx_currency,denom, sum([count]) as [count], sum(subtotal) as subtotal from DenominationDetail
	INNER JOIN [Transaction] as t on t.id = tx_id
	where ([datetime] between @startDate AND @endDate) AND t.device_id = (SELECT id FROM ThisDevice as td)
	group by t.tx_currency,denom
             */
            var denominationByDatesResults = DBContext.DenominationDetails.Where(x => x.datetime >= thisCITFromDate && x.datetime <= thisCITToDate)
                .Include(i => i.tx)
                .GroupBy(x => new { x.tx.tx_currency, x.denom })
                .Select(cl => new GetCITDenominationByDates_Result
                {
                    subtotal = cl.Sum(c => c.subtotal),
                    count = cl.Sum(c => c.count),
                    tx_currency= cl.FirstOrDefault().tx.tx_currency,
                    denom =cl.FirstOrDefault().denom,

                }).ToList();
            // var denominationByDatesResults= DBContext.GetCITDenominationByDates(new DateTime?(thisCITFromDate), new DateTime?(thisCITToDate)).OrderBy(x => x.denom).ToList();
            foreach (var denominationByDatesResult in denominationByDatesResults)
            {
                ICollection<CITDenomination> citDenominations = CIT.CITDenominations;
                CITDenomination citDenomination = new CITDenomination();
                citDenomination.id = GuidExt.UuidCreateSequential();
                citDenomination.currency_id = denominationByDatesResult.tx_currency;
                citDenomination.datetime = new DateTime?(thisCITToDate);
                citDenomination.denom = denominationByDatesResult.denom;
                long? nullable = denominationByDatesResult.count;
                citDenomination.count = nullable.Value;
                nullable = denominationByDatesResult.subtotal;
                citDenomination.subtotal = nullable.Value;
                citDenominations.Add(citDenomination);
            }
            CreateCITTransactions(CIT);
            return CIT;
        }

        private void CreateCITTransactions(CIT cit)
        {
            try
            {
                ApplicationViewModel.Log.DebugFormat("ApplicationViewModel", "Generate CITTransaction", nameof(CreateCITTransactions), "Generating CITTransactions for CIT id={0}", cit.id);
                List<CITTransaction> citTransactionList = new List<CITTransaction>(5);
                foreach (IGrouping<string, CITDenomination> grouping in cit.CITDenominations.GroupBy(denom => denom.currency_id))
                {
                    IGrouping<string, CITDenomination> currency = grouping;
                    long num = currency.Sum(x => x.subtotal);
                    if (num > 0L)
                    {
                        DeviceCITSuspenseAccount citSuspenseAccount = DBContext.DeviceCITSuspenseAccounts.FirstOrDefault(x => x.device_id == cit.device_id && x.currency_code.Equals(currency.Key, StringComparison.OrdinalIgnoreCase) && x.enabled == true);

                        if (ApplicationViewModel.DeviceConfiguration.CIT_ALLOW_POST && citSuspenseAccount == null)
                            throw new NullReferenceException(string.Format("No valid CITSuspenseAccount found for currency {0}", currency));
                        cit.CITTransactions.Add(new CITTransaction()
                        {
                            id = Guid.NewGuid(),
                            cit_id = cit.id,
                            account_number = citSuspenseAccount?.account_number ?? "",
                            suspense_account = (DBContext.DeviceSuspenseAccounts.FirstOrDefault(x => x.device_id == cit.device_id && x.currency_code.Equals(currency.Key, StringComparison.OrdinalIgnoreCase) && x.enabled == true) ?? throw new NullReferenceException(string.Format("No valid DeviceSuspenseAccount found for currency {0}", currency))).account_number,
                            datetime = DateTime.Now,
                            amount = num,
                            currency = currency.Key,
                            narration = string.Format("CIT {0} on {1:yyyyMMddTHHmmss}", Device.device_number, cit.cit_date)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.ErrorFormat("ApplicationViewModel.CreateCITTransactions", 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), "Error posting CIT {0}: {1}", cit.id, ex.MessageString());
                throw;
            }
        }
    }
}
