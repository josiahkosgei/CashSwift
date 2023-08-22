using Caliburn.Micro;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Utils;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace CashSwiftDeposit.Models
{
    public class DepositorPrinter : PropertyChangedBase
    {
        private bool logoTest;
        public static char Esc = '\u001B';
        private DepositorLogger Log;
        private PrinterState _state;
        private SerialPort _port;
        private DispatcherTimer dispTimer = new DispatcherTimer(DispatcherPriority.Send);
        private bool _CTSHolding;

        public PrinterState State => _state;

        private ApplicationViewModel ApplicationViewModel { get; }

        public DepositorPrinter(
          ApplicationViewModel applicationViewModel,
          DepositorLogger log,
          string portName,
          int baudRate = 9600,
          Parity parity = Parity.None,
          int databits = 8,
          StopBits stopBits = StopBits.One)
        {
            ApplicationViewModel = applicationViewModel;
            Log = log;
            Log?.Info(GetType().Name, "Port Listener Initialising", "Initialisation", "Initialising the port listener");
            Directory.CreateDirectory(ApplicationViewModel.DeviceConfiguration.RECEIPT_FOLDER);
            Port = new SerialPort(portName, baudRate, parity, databits, stopBits);
            dispTimer.Interval = TimeSpan.FromSeconds(5.0);
            dispTimer.Tick += new EventHandler(dispTimer_Tick);
            dispTimer.IsEnabled = true;
            Log?.Info(GetType().Name, "Port Listener Initialising Result", "Initialisation", "SUCCESS");
        }

        private SerialPort Port
        {
            get => _port;
            set => _port = value;
        }

        public object PrintCITReceiptLock { get; set; } = new object();

        public bool CTSHolding
        {
            get => _CTSHolding;
            set
            {
                if (value == _CTSHolding)
                    return;
                _CTSHolding = value;
                NotifyOfPropertyChange(nameof(CTSHolding));
                OnPrinterStateChangedEventEvent(this, new PrinterStateChangedEventArgs()
                {
                    state = new PrinterState() { HasError = value }
                });
            }
        }

        private void dispTimer_Tick(object sender, EventArgs e) => CTSHolding = GetCTSHolding();

        public void PrintCIT(CIT cit, DepositorDBContext DBContext, bool isCopy)
        {
            CITPrintout printout = new CITPrintout()
            {
                id = GuidExt.UuidCreateSequential(),
                print_guid = Guid.NewGuid(),
                is_copy = isCopy,
                datetime = DateTime.Now
            };
            string printoutFromCit;
            try
            {
                printoutFromCit = GeneratePrintoutFromCIT(cit, printout);
            }
            catch (IOException ex)
            {
                Log?.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "IOException generating CIT receipt: {0}", ex.MessageString());
                throw;
            }
            catch (NullReferenceException ex)
            {
                Log?.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "NullReferenceException generating CIT receipt: {0}", ex.MessageString());
                throw;
            }
            catch (Exception ex)
            {
                Log?.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Error generating CIT receipt: {0}", ex.MessageString());
                throw;
            }
            cit.CITPrintouts.Add(printout);
            try
            {
                ApplicationViewModel.SaveToDatabase(DBContext);
            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
                return;
            }
            int receiptOriginalCount = ApplicationViewModel.DeviceConfiguration.CIT_RECEIPT_ORIGINAL_COUNT;
            int num = receiptOriginalCount < 1 ? 1 : receiptOriginalCount;
            for (int index = 0; index < ApplicationViewModel.DeviceConfiguration.CIT_RECEIPT_ORIGINAL_COUNT; ++index)
                Print(printoutFromCit);
        }

        public void PrintTransaction(
          Transaction transaction,
          DepositorDBContext depositorDBContext,
          bool isCopy = false)
        {
            Printout printout = new Printout()
            {
                id = GuidExt.UuidCreateSequential(),
                print_guid = Guid.NewGuid(),
                is_copy = isCopy,
                datetime = DateTime.Now
            };
            string printoutFromTransaction;
            try
            {
                printoutFromTransaction = GeneratePrintoutFromTransaction(transaction, printout, depositorDBContext);
            }
            catch (IOException ex)
            {
                Log.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "IOException generating  receipt: {0}", ex.MessageString());
                return;
            }
            catch (NullReferenceException ex)
            {
                Log.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "NullReferenceException generating  receipt: {0}>>{1}>>{2}>stack>{3}", ex.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message, ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Error generating  receipt:  {0}>>{1}>>{2}>stack>{3}", ex.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message, ex.StackTrace);
                throw;
            }
            transaction.Printouts.Add(printout);
            ApplicationViewModel.SaveToDatabase(depositorDBContext);
            int receiptOriginalCount = ApplicationViewModel.DeviceConfiguration.RECEIPT_ORIGINAL_COUNT;
            int num = receiptOriginalCount < 1 ? 1 : receiptOriginalCount;
            for (int index = 0; index < num; ++index)
                Print(printoutFromTransaction);
        }

        public void Print(string path)
        {
            try
            {
                Port.Open();
                using (FileStream input = File.OpenRead(path))
                    Port.Write(new BinaryReader(input).ReadBytes((int)input.Length), 0, (int)input.Length);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 90, ApplicationErrorConst.ERROR_FILE_IO.ToString(), "Print(string path) Error generating  receipt:  {0}", ex.MessageString());
            }
            finally
            {
                if (Port.IsOpen)
                    Port.Close();
            }
        }

        private string GeneratePrintoutFromTransaction(
          Transaction transaction,
          Printout printout,
          DepositorDBContext depositorDBContext)
        {
            TransactionText transactionText = transaction?.TransactionTypeListItem?.TransactionText;
            if (transactionText == null)
                throw new NullReferenceException("GeneratePrintoutFromTransaction(): transactionText cannot be null");
            string str1 = "\r\n" + ApplicationViewModel.CashSwiftTranslationService?.TranslateUserText(GetType().Name + ".GeneratePrintoutFromTransaction receiptTemplate", transactionText.receipt_template, null).Replace("\r\n", "\n").Replace("\n", "\r\n");
            if (str1 == null)
                throw new NullReferenceException("GeneratePrintoutFromTransaction(): receipt_template cannot be null");
            Device device = depositorDBContext.Devices.FirstOrDefault(x => x.id == transaction.device_id);
            if (device == null)
                throw new NullReferenceException("GeneratePrintoutFromTransaction(): transactionDevice cannot be null");
            string str2 = str1.Replace("{device_name}", device.name).Replace("{device_machine_name}", device.machine_name).Replace("{device_device_number}", device.device_number).Replace("{receipt_bank_name}", ApplicationViewModel.DeviceConfiguration.RECEIPT_BANK_NAME);
            string str3 = (!printout.is_copy ? str2.Replace("{receipt_copy_text}" + Environment.NewLine, "").Replace("{receipt_copy_print_date}" + Environment.NewLine, "") : str2.Replace("{receipt_copy_text}", ApplicationViewModel.DeviceConfiguration.RECEIPT_COPY_TEXT).Replace("{receipt_copy_print_date}", "Printed on: " + DateTime.Now.ToString(ApplicationViewModel.DeviceConfiguration.RECEIPT_DATE_FORMAT))).Replace("{transactiontypelistitem_name}", transaction.TransactionTypeListItem.name);
            Transaction transaction1 = transaction;
            DateTime dateTime;
            string newValue1;
            if (transaction1 == null)
            {
                newValue1 = null;
            }
            else
            {
                dateTime = transaction1.tx_end_date.Value;
                newValue1 = dateTime.ToString(ApplicationViewModel.DeviceConfiguration.RECEIPT_DATE_FORMAT);
            }
            string str4 = str3.Replace("{tx_end_date}", newValue1);
            Transaction transaction2 = transaction;
            string newValue2;
            if (transaction2 == null)
            {
                newValue2 = null;
            }
            else
            {
                dateTime = transaction2.tx_start_date;
                newValue2 = dateTime.ToString(ApplicationViewModel.DeviceConfiguration.RECEIPT_DATE_FORMAT);
            }
            string str5 = str4.Replace("{tx_start_date}", newValue2);
            int val = transaction.tx_account_number.Length - ApplicationViewModel.DeviceConfiguration.RECEIPT_ACCOUNT_NUMBER_VISIBLE_DIGITS;
            string str6 = str5.Replace("{tx_account_number}", transaction.tx_account_number.Substring(val.Clamp(0, transaction.tx_account_number.Length - 1)).PadLeft(transaction.tx_account_number.Length, ApplicationViewModel.DeviceConfiguration.RECEIPT_ACCOUNT_NO_PAD_CHAR)).Replace("{cb_account_name}", transaction.cb_account_name);
            string str7 = transaction.tx_ref_account == null ? str6.Replace("{tx_ref_account}" + Environment.NewLine, "") : str6.Replace("{tx_ref_account}", string.Format("{0}: {1}", ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("DevicePrinter.PrintTransaction.reference_account_number_caption", transactionText?.reference_account_number_caption, "Reference Account"), transaction.tx_ref_account));
            string str8 = (transaction.cb_ref_account_name == null ? str7.Replace("{cb_ref_account_name}" + Environment.NewLine, "") : str7.Replace("{cb_ref_account_name}", string.Format("{0}: {1}", ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("DevicePrinter.PrintTransaction.reference_account_name_caption", transactionText?.reference_account_name_caption, "Reference Name"), transaction.cb_ref_account_name))).Replace("{branch_name}", device.Branch.name).Replace("{tx_random_number}", transaction.tx_random_number.Value.ToString() ?? "");
            string str9 = (transaction.cb_tx_number == null ? str8.Replace("{cb_tx_number}" + Environment.NewLine, "") : str8.Replace("{cb_tx_number}", transaction.cb_tx_number ?? "")).Replace("{tx_currency}", transaction.tx_currency.ToUpper());
            string str10 = transaction.tx_narration == null ? str9.Replace("{tx_narration}" + Environment.NewLine, "") : str9.Replace("{tx_narration}", string.Format("{0}: {1}", ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.narration_caption", transactionText?.narration_caption, "Narration"), transaction.tx_narration));
            string str11 = transaction.funds_source == null ? str10.Replace("{tx_funds_source}" + Environment.NewLine, "") : str10.Replace("{tx_funds_source}", string.Format("{0}: {1}", ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.funds_source_caption", transactionText?.FundsSource_caption, "Funds Source"), transaction.funds_source));
            string str12 = transaction.tx_depositor_name == null ? str11.Replace("{tx_depositor_name}" + Environment.NewLine, "") : str11.Replace("{tx_depositor_name}", string.Format("{0}: {1}", ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.depositor_name_caption", transactionText?.depositor_name_caption, "Depositor Name"), transaction.tx_depositor_name));
            string str13 = string.IsNullOrWhiteSpace(transaction.tx_id_number) ? str12.Replace("{tx_id_number}" + Environment.NewLine, "") : str12.Replace("{tx_id_number}", string.Format("{0}: {1}", ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.id_number_caption", transactionText?.id_number_caption, "ID Number"), transaction.tx_id_number));
            List<string> list = (transaction.tx_phone == null ? str13.Replace("{tx_phone}" + Environment.NewLine, "") : str13.Replace("{tx_phone}", string.Format("{0}: {1}", ApplicationViewModel.CashSwiftTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.phone_number_caption", transactionText?.phone_number_caption, "Phone"), transaction.tx_phone))).Replace("{denomination_breakdown}", GenerateDenominationBreakdown(transaction)).Replace("{Printout_UUID}", printout.print_guid.ToString()).Split(new string[1]
            {
        Environment.NewLine
            }, StringSplitOptions.None).ToList();
            object[] objArray = new object[4]
            {
         ApplicationViewModel.DeviceConfiguration.RECEIPT_FOLDER,
        null,
        null,
        null
            };
            dateTime = DateTime.Now;
            objArray[1] = dateTime.ToString("yyy-MM-dd HH.mm.ss.fff");
            objArray[2] = transaction.Printouts.Count;
            objArray[3] = string.Format("{0}_{1}_{2:0}", transaction?.tx_account_number, transaction?.tx_ref_account, transaction?.tx_amount);
            string path1 = string.Format("{0}\\[{1}]_receipt_{2}_{3}.txt", objArray);
            File.WriteAllText(path1 + "1", string.Join("\n", list.ToArray()));
            if (ApplicationViewModel.DeviceConfiguration.RECEIPT_INVERT_ORDER)
                list.Reverse();
            string str14 = string.Join("\r", list.ToArray());
            printout.print_content = string.Join(Environment.NewLine, list.ToArray());
            if (ApplicationViewModel.DeviceConfiguration.RECEIPT_INVERT_ORDER)
            {
                File.AppendAllText(path1, logoTest ? "\r" : str14);
                string path2 = AppDomain.CurrentDomain.BaseDirectory + ApplicationViewModel.DeviceConfiguration.RECEIPT_LOGO;
                FileIOExtentions.AppendAllBytes(path1, File.ReadAllBytes(path2));
            }
            else
            {
                string path3 = AppDomain.CurrentDomain.BaseDirectory + ApplicationViewModel.DeviceConfiguration.RECEIPT_LOGO;
                FileIOExtentions.AppendAllBytes(path1, File.ReadAllBytes(path3));
                File.AppendAllText(path1, logoTest ? "\r cutcyrcytc \r\r\r\r" : str14);
            }
            File.AppendAllText(path1, "\n\n\n\n");
            return path1;
        }

        private string GeneratePrintoutFromCIT(CIT cit, CITPrintout printout)
        {
            lock (PrintCITReceiptLock)
            {
                try
                {
                    List<string> stringList1 = new List<string>();
                    new DepositorDBContext().Devices.FirstOrDefault(x => x.id == cit.device_id);
                    stringList1.Add(MakeTitleText(ApplicationViewModel.DeviceConfiguration.RECEIPT_BANK_NAME));
                    if (printout.is_copy)
                        stringList1.Add("RECEIPT COPY");
                    stringList1.Add("========================");
                    stringList1.Add(ApplicationViewModel.DeviceConfiguration.RECEIPT_CIT_TITLE);
                    stringList1.Add("========================");
                    stringList1.Add("Printed on: ");
                    List<string> stringList2 = stringList1;
                    DateTime dateTime = DateTime.Now;
                    string str1 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    stringList2.Add(str1);
                    stringList1.Add("Start Date: ");
                    List<string> stringList3 = stringList1;
                    string str2;
                    if (!cit.fromDate.HasValue)
                    {
                        dateTime = DateTime.MinValue;
                        str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    }
                    else
                    {
                        dateTime = cit.fromDate.Value;
                        str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    }
                    stringList3.Add(str2);
                    stringList1.Add("End Date: ");
                    List<string> stringList4 = stringList1;
                    dateTime = cit.toDate;
                    string str3 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var usernameStrt=new DepositorDBContext().ApplicationUsers.Where(x => x.id==cit.start_user).FirstOrDefault();
                    var usernameAuth=new DepositorDBContext().ApplicationUsers.Where(x => x.id==cit.auth_user).FirstOrDefault();
                    stringList4.Add(str3);
                    stringList1.Add("User: ");
                    stringList1.Add(usernameStrt.username);
                    stringList1.Add("Authorising User: ");
                    stringList1.Add(usernameAuth.username);
                    stringList1.Add("Branch:");
                    stringList1.Add(cit.Device.Branch.name);
                    stringList1.Add("Device Name:");
                    stringList1.Add(cit.Device.name);
                    stringList1.Add("Device Location:");
                    stringList1.Add(cit.Device.device_location);
                    stringList1.Add("Bag Number:");
                    stringList1.Add(cit.old_bag_number);
                    stringList1.Add("Seal Number:");
                    stringList1.Add(cit.seal_number);
                    stringList1.Add("Total Transaction Count:");
                    stringList1.Add(cit.Transactions.Count().ToString() ?? "");
                    stringList1.Add("Total Note Count:");
                    stringList1.Add(cit.Transactions.Sum(x => x.DenominationDetails.Sum(y => y.count)).ToString() ?? "");
                    stringList1.Add("Total Currency Count:");
                    List<string> stringList5 = stringList1;
                    int num1 = cit.Transactions.Select(x => x.Currency).Distinct().Count();
                    string str4 = num1.ToString() ?? "";
                    stringList5.Add(str4);
                    stringList1.Add(DrawSingleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                    stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                    stringList1.Add(DrawSingleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                    foreach (var currency1 in cit.Transactions.Select(x => x.Currency).Distinct().ToList())
                    {
                        var currency = currency1;
                        List<CITDenomination> list = cit.CITDenominations.Where(x => x.currency_id == currency.code).ToList();
                        stringList1.Add("Currency:");
                        stringList1.Add(currency.code.ToUpper());
                        stringList1.Add("Transaction Count:");
                        List<string> stringList6 = stringList1;
                        num1 = cit.Transactions.Where(x => x.Currency == currency).Count();
                        string str5 = num1.ToString() ?? "";
                        stringList6.Add(str5);
                        stringList1.Add(DrawSingleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                        stringList1.Add(MakeTitleText(currency.code.ToUpper() + " Denominations"));
                        stringList1.Add(DrawSingleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                        foreach (CITDenomination citDenomination in (IEnumerable<CITDenomination>)list.OrderBy((x => x.denom)))
                        {
                            num1 = citDenomination.denom / 100;
                            string str6 = num1.ToString() ?? "";
                            long num2 = citDenomination.count;
                            string str7 = num2.ToString() ?? "";
                            num2 = citDenomination.denom * citDenomination.count / 100L;
                            string str8 = num2.ToString() ?? "";
                            int num3 = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 11;
                            stringList1.Add(string.Format(string.Format("{{0,-6}}{{1,5}}{{2,{0}}}", num3), str6, str7, str8));
                        }
                        stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                        stringList1.Add(string.Format(string.Format("{{0,-6}}" + Environment.NewLine + "{{1,{0}}}", ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 4), currency.code.ToUpper() + " TOTAL:", MakeTitleText(string.Format("{0:#,#0.##}", list.Sum((x => x.subtotal)) / 100.0M))));
                        stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                        try
                        {
                            ICollection<Transaction> transactions = cit.Transactions;
                            long? nullable1;
                            if (transactions == null)
                            {
                                nullable1 = new long?();
                            }
                            else
                            {
                                IEnumerable<Transaction> source = transactions.Where(x => x.tx_currency.Equals(currency.code, StringComparison.OrdinalIgnoreCase));
                                nullable1 = source != null ? new long?(source.Sum(y => y.EscrowJams.Sum(z => z.retreived_amount))) : new long?();
                            }
                            long? nullable2 = nullable1;
                            Decimal? nullable3 = nullable2.HasValue ? new Decimal?(nullable2.GetValueOrDefault() / 100M) : new Decimal?();
                            Decimal? nullable4 = nullable3;
                            Decimal num4 = 0M;
                            if (nullable4.GetValueOrDefault() > num4 & nullable4.HasValue)
                            {
                                stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                                stringList1.Add(string.Format(string.Format("{{0,-6}}" + Environment.NewLine + "{{1,{0}}}", ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 4), currency.code.ToUpper() + " Recovered:", MakeTitleText(string.Format("{0:#,#0.##}", nullable3))));
                                stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    stringList1.Add("\n");
                    stringList1.Add("\n");
                    stringList1.Add(cit.id.ToString().ToUpper());
                    stringList1.Add("\n");
                    string path1 = string.Format("{0}\\[{1}]_citreceipt_{2}.txt", ApplicationViewModel.DeviceConfiguration.RECEIPT_FOLDER, DateTime.Now.ToString("yyy-MM-dd HH.mm.ss.fff"), cit.id);
                    File.WriteAllText(path1 + "1", string.Join("\r", stringList1.ToArray()));
                    printout.print_content = string.Join(Environment.NewLine, stringList1.ToArray());
                    cit.CITPrintouts.Add(printout);
                    if (ApplicationViewModel.DeviceConfiguration.RECEIPT_INVERT_ORDER)
                        stringList1.Reverse();
                    string str9 = string.Join("\r", stringList1.ToArray());
                    if (ApplicationViewModel.DeviceConfiguration.RECEIPT_INVERT_ORDER)
                    {
                        File.AppendAllText(path1, logoTest ? "\r" : str9);
                        string path2 = AppDomain.CurrentDomain.BaseDirectory + ApplicationViewModel.DeviceConfiguration.RECEIPT_LOGO;
                        Log.Debug(GetType().Name, "GeneratePrintoutFromTransaction", "Printer", "Adding the logo from " + path2);
                        FileIOExtentions.AppendAllBytes(path1, File.ReadAllBytes(path2));
                    }
                    else
                    {
                        string path3 = AppDomain.CurrentDomain.BaseDirectory + ApplicationViewModel.DeviceConfiguration.RECEIPT_LOGO;
                        Log.Debug(GetType().Name, "GeneratePrintoutFromTransaction", "Printer", "Adding the logo from " + path3);
                        FileIOExtentions.AppendAllBytes(path1, File.ReadAllBytes(path3));
                        File.AppendAllText(path1, logoTest ? "\r" : str9);
                    }
                    File.AppendAllText(path1, "\n\n\n\n");
                    return path1;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private string DrawSingleLine(int length) => MakeTitleText("".PadLeft(length, '_'));

        private string MakeTitleText(string text) => text;

        private string DrawDoubleLine(int length) => MakeTitleText("".PadLeft(length, '='));

        private string MakeBold(string v) => string.Format("{0}E1{1}{2}E0", Esc, v, Esc);

        public bool GetCTSHolding()
        {
            bool ctsHolding = false;
            try
            {
                ctsHolding = Port.CtsHolding;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCTSHolding() Error: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
            }
            return ctsHolding;
        }

        public string GenerateDenominationBreakdown(Transaction transaction)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            foreach (DenominationDetail denominationDetail in (IEnumerable<DenominationDetail>)transaction.DenominationDetails)
            {
                string str1 = (denominationDetail.denom / 100).ToString() ?? "";
                string str2 = denominationDetail.count.ToString() ?? "";
                string str3 = (denominationDetail.denom * denominationDetail.count / 100L).ToString() ?? "";
                int num = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 11;
                stringBuilder1.AppendLine(string.Format(string.Format("{{0,-6}}{{1,5}}{{2, {0}}}", num), str1, str2, str3));
            }
            stringBuilder1.AppendLine(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
            StringBuilder stringBuilder2 = stringBuilder1;
            string format = string.Format("{{0,-6}}{{1, {0}}}", ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 6);
            long? txAmount = transaction.tx_amount;
            long num1 = 100;
            string str4 = MakeTitleText((txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num1) : new long?()).Value.ToString(ApplicationViewModel.DeviceConfiguration.APPLICATION_MONEY_FORMAT));
            string str5 = string.Format(format, "TOTAL:", str4);
            stringBuilder2.AppendLine(str5);
            stringBuilder1.AppendLine(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
            return stringBuilder1.ToString();
        }

        public event EventHandler<PrinterStateChangedEventArgs> PrinterStateChangedEvent;

        private void OnPrinterStateChangedEventEvent(
          object sender,
          PrinterStateChangedEventArgs e)
        {
            if (PrinterStateChangedEvent == null)
                return;
            PrinterStateChangedEvent(this, e);
        }

        public class PrinterStateChangedEventArgs : EventArgs
        {
            public PrinterState state = new PrinterState()
            {
                CoverOpen = false,
                HasPaper = true,
                HasError = false,
                ErrorCode = 0,
                ErrorType = PrinterErrorType.NONE,
                ErrorMessage = "No Error"
            };
        }
    }
}
