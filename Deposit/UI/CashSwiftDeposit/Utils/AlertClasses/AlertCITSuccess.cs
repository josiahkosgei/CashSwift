using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwift.Library.Standard.Security;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDeposit.ViewModels;
using CashSwiftUtil.Reporting.CITReporting;
using CashSwiftUtil.Reporting.MSExcel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using CIT = CashSwiftDataAccess.Entities.CIT;
using Transaction = CashSwiftDataAccess.Entities.Transaction;
using CITDenomination = CashSwiftDataAccess.Entities.CITDenomination;

namespace CashSwiftDeposit.Utils.AlertClasses
{
    public class AlertCITSuccess : AlertBase
    {
        public const int ALERT_ID = 1300;
        private CIT _cit;

        public AlertCITSuccess(CIT cit, Device device, DateTime dateDetected)
          : base(device, dateDetected)
        {
            _cit = cit != null ? cit : throw new NullReferenceException("Variable cit cannot be null");
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                AlertType = depositorDbContext.AlertMessageTypes.FirstOrDefault(x => x.id == 1300);
        }

        public override bool SendAlert()
        {
            try
            {
                using (DepositorDBContext DBContext = new DepositorDBContext())
                {
                    GenerateTokens();
                    AlertEvent entity = new AlertEvent()
                    {
                        id = GuidExt.UuidCreateSequential(),
                        created = DateTime.Now,
                        alert_type_id = AlertType.id,
                        machine_name = Environment.MachineName.ToUpperInvariant(),
                        date_detected = DateDetected,
                        date_resolved = new DateTime?(DateResolved),
                        device_id = Device.id,
                        is_resolved = true
                    };
                    DBContext.AlertEvents.Add(entity);
                    AlertEmail email = GenerateEmail(DBContext);
                    if (email != null)
                        entity.AlertEmails.Add(email);
                    AlertSMS sms = GenerateSMS();
                    if (sms != null)
                        entity.AlertSMSes.Add(sms);
                    ApplicationViewModel.SaveToDatabase(DBContext);
                }
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
            }
            return false;
        }

        private new AlertEmail GenerateEmail(DepositorDBContext DBContext)
        {
            AlertEmail alertEmail = new AlertEmail()
            {
                id = GuidExt.UuidCreateSequential(),
                created = DateTime.Now,
                html_message = GetHTMLBody(),
                raw_text_message = GetRawTextBody(),
                subject = AlertType.name,
                sent = false
            };
            try
            {
                CreateAlertEmailattachments(DBContext, alertEmail);
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(AlertCITSuccess), 1, "CreateAlertEmailattachments Failed", ex.MessageString());
            }
            return alertEmail.raw_text_message == null || alertEmail.html_message == null ? null : alertEmail;
        }

        private void CreateAlertEmailattachments(DepositorDBContext DBContext, AlertEmail alertEmail)
        {
            DirectoryInfo directory = Directory.CreateDirectory(ApplicationViewModel.DeviceConfiguration.EMAIL_LOCAL_FOLDER + "\\Attachments\\CITReport\\");
            GenerateCITReportAttachment(DBContext, alertEmail, directory);
        }

        private void GenerateCITReportAttachment(
          DepositorDBContext DBContext,
          AlertEmail alertEmail,
          DirectoryInfo directory)
        {
            AlertAttachmentType alertAttachmentType = DBContext.AlertAttachmentTypes.FirstOrDefault(x => x.code.Equals("130001", StringComparison.Ordinal));
            if (alertAttachmentType == null)
                return;
            if (!alertAttachmentType.enabled)
            {
                ApplicationViewModel.AlertLog.WarningFormat(nameof(AlertCITSuccess), "AttachmentType Disabled", "CreateAlertEmailattachments", "Attachment type {0} is disabled", alertAttachmentType.name);
            }
            else
            {
                ApplicationViewModel.AlertLog.TraceFormat(nameof(AlertCITSuccess), "AttachmentType Enabled", "CreateAlertEmailattachments", "Attachment type {0} is enabled", alertAttachmentType.name);
                ApplicationViewModel.AlertLog.Trace(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "create the CIT Report");
                CITReport CITReport = new CITReport()
                {
                    CIT = new CashSwiftUtil.Reporting.CITReporting.CIT()
                    {
                        id = _cit.id,
                        cit_complete_date = _cit.cit_complete_date,
                        cit_date = _cit.cit_date,
                        Error = _cit.cit_error.ToString("0"),
                        ErrorMessage = _cit.cit_error_message,
                        Complete = _cit.complete,
                        device_id = _cit.device_id,
                        device = _cit.Device.name,
                        FromDate = _cit.fromDate,
                        ToDate = _cit.toDate,
                        NewBagNumber = _cit.new_bag_number,
                        OldBagNumber = _cit.old_bag_number,
                        SealNumber = _cit.seal_number,
                        InitiatingUser = _cit?.StartUser?.username,
                        AuthorisingUser = _cit?.AuthUser?.username
                    }
                };
                ApplicationViewModel.AlertLog.TraceFormat(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "Add Txns to CITReport");
                CITReport.Transactions = new List<CashSwiftUtil.Reporting.CITReporting.Transaction>(_cit.Transactions.Count());
                foreach (Transaction transaction in (IEnumerable<Transaction>)_cit.Transactions.Where(x =>
                {
                    if (x.tx_error_code != 1010)
                        return true;
                    if (x.tx_error_code != 1011)
                        return false;
                    long? txAmount = x.tx_amount;
                    long num = 0;
                    return txAmount.GetValueOrDefault() > num & txAmount.HasValue;
                }).OrderBy(y => y.tx_start_date))
                {
                    Transaction x = transaction;
                    CITReport.Transactions.Add(new CashSwiftUtil.Reporting.CITReporting.Transaction()
                    {
                        id = x.id,
                        AccountName = x.cb_account_name,
                        AccountNumber = x.tx_account_number,
                        Amount = (x.tx_amount.HasValue ? x.tx_amount.Value : 0.0M) / 100.0M,
                        Currency = x.tx_currency,
                        StartTime = x.tx_start_date,
                        EndTime = x.tx_end_date.Value,
                        DepositorIDNumber = x.tx_id_number,
                        DepositorName = x.tx_depositor_name,
                        DepositorPhone = x.tx_phone,
                        DeviceID = x.device_id,
                        DeviceName = x.Device.name,
                        DeviceReferenceNumber = x.tx_random_number.Value.ToString("0"),
                        CB_Reference = x.cb_tx_number,
                        FundsSource = x.funds_source,
                        Narration = x.tx_narration,
                        RefAccountName = x.cb_ref_account_name,
                        RefAccountNumber = x.tx_ref_account,
                        SuspenseAccount = x.Device.DeviceSuspenseAccounts.FirstOrDefault(t => t.device_id == x.device_id && t.currency_code == x.tx_currency)?.account_number,
                        TransactionType = x.TransactionTypeListItem?.name,
                        ErrorCode = x.tx_error_code,
                        ErrorMessage = x.tx_error_message
                    });
                }
                CITReport.EscrowJams = _cit.Transactions.SelectMany(tx => tx.EscrowJams).Select(jam => new CashSwiftUtil.Reporting.CITReporting.EscrowJam()
                {
                    id = jam.id,
                    AdditionalInfo = jam.additional_info,
                    AuthorisingUser = jam.AuthorisingUser.username,
                    InitialisingUser = jam.InitialisingUser.username,
                    DateDetected = jam.date_detected,
                    DroppedAmount = jam.dropped_amount / 100M,
                    EscrowAmount = jam.escrow_amount / 100M,
                    PostedAmount = jam.posted_amount / 100M,
                    RetreivedAmount = jam.retreived_amount / 100M,
                    RecoveryDate = jam.recovery_date,
                    AccountNumber = jam.Transaction.tx_account_number,
                    DeviceReferenceNumber = string.Format("{0:0}", jam.Transaction.tx_random_number),
                    transaction_id = jam.transaction_id,
                    CB_Reference = jam.Transaction.cb_tx_number,
                    StartTime = jam.Transaction.tx_start_date,
                    EndTime = jam.Transaction.tx_end_date
                }).ToList();
                ApplicationViewModel.AlertLog.TraceFormat(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "Add CITDenoms to CITReport");
                CITReport.CITDenominations = new List<CashSwiftUtil.Reporting.CITReporting.CITDenomination>(_cit.CITDenominations.Count());
                foreach (CITDenomination citDenomination in (IEnumerable<CITDenomination>)_cit.CITDenominations)
                    CITReport.CITDenominations.Add(new CashSwiftUtil.Reporting.CITReporting.CITDenomination()
                    {
                        Currency = citDenomination.Currency.code,
                        Denom = citDenomination.denom / 100.0M,
                        Count = citDenomination.count,
                        SubTotal = citDenomination.subtotal / 100.0M
                    });
                ApplicationViewModel.AlertLog.DebugFormat(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "CITReport created SUCCESS");
                string path2 = string.Format("CITReport_{0:yyyy-MM-dd HH.mm.ss.fff}.xlsx", CITReport.CIT.cit_date);
                string str = Path.Combine(directory.FullName, path2);
                byte[] citExcelAttachment = ExcelManager.GenerateCITExcelAttachment(CITReport, str);
                FileInfo fileInfo = new FileInfo(str);
                ApplicationViewModel.AlertLog.InfoFormat(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "CITReport attachment saved at {0} with {1} bytes", str, fileInfo.Length.ToString("#,##0"));
                DBContext.AlertEmailAttachments.Add(new AlertEmailAttachment()
                {
                    id = Guid.NewGuid(),
                    alert_email_id = alertEmail.id,
                    name = path2,
                    path = str,
                    type = alertAttachmentType.code,
                    data = citExcelAttachment,
                    hash = CashSwiftHashing.HMACSHA512(Device.app_key, citExcelAttachment)
                });
                ApplicationViewModel.SaveToDatabase(DBContext);
            }
        }

        private AlertSMS GenerateSMS()
        {
            AlertSMS alertSm = new AlertSMS()
            {
                id = GuidExt.UuidCreateSequential(),
                created = DateTime.Now,
                message = GetSMSBody(),
                sent = false
            };
            return alertSm.message == null ? null : alertSm;
        }

        private new void GenerateTokens()
        {
            Tokens = new Dictionary<string, string>();
            Tokens.Add("[date]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            Tokens.Add("[device_id]", Device.device_number);
            Tokens.Add("[device_name]", Device.name);
            Tokens.Add("[device_location]", Device.device_location);
            Tokens.Add("[branch_name]", Device.Branch.name);
            Tokens.Add("[event_title]", AlertType.title);
            IDictionary<string, string> tokens1 = Tokens;
            int num = AlertType.id;
            string str1 = num.ToString() ?? "";
            tokens1.Add("[event_id]", str1);
            Tokens.Add("[event_name]", AlertType.name);
            Tokens.Add("[event_description]", AlertType.description);
            Tokens.Add("[cit_date]", _cit.cit_date.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            IDictionary<string, string> tokens2 = Tokens;
            DateTime? nullable = _cit.fromDate;
            DateTime dateTime;
            string str2;
            if (!nullable.HasValue)
            {
                dateTime = DateTime.MinValue;
                str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }
            else
            {
                nullable = _cit.fromDate;
                dateTime = nullable.Value;
                str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }
            tokens2.Add("[cit_start]", str2);
            IDictionary<string, string> tokens3 = Tokens;
            dateTime = _cit.toDate;
            string str3 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            tokens3.Add("[cit_end]", str3);
            IDictionary<string, string> tokens4 = Tokens;
            nullable = _cit.cit_complete_date;
            string str4;
            if (!nullable.HasValue)
            {
                str4 = null;
            }
            else
            {
                nullable = _cit.cit_complete_date;
                dateTime = nullable.Value;
                str4 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }
            tokens4.Add("[cit_completed_date]", str4);
            Tokens.Add("[user_init]", _cit.StartUser.username);
            Tokens.Add("[user_auth]", _cit.AuthUser.username);
            Tokens.Add("[old_bag_number]", _cit.old_bag_number);
            Tokens.Add("[new_bag_number]", _cit.new_bag_number);
            Tokens.Add("[seal_number]", _cit.seal_number);
            IDictionary<string, string> tokens5 = Tokens;
            num = _cit.cit_error;
            string str5 = num.ToString();
            tokens5.Add("[error_code]", str5);
            Tokens.Add("[error_message]", _cit.cit_error_message);
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken()
        {
            StringBuilder stringBuilder = new StringBuilder(byte.MaxValue);
            foreach (IGrouping<Currency, CITDenomination> source in _cit.CITDenominations.GroupBy(x => x.Currency))
            {
                Currency groupKey = source.Key;
                stringBuilder.AppendLine(string.Format("<hr /><h3>Currency: {0}</h3><hr />", groupKey.code.ToUpper()));
                stringBuilder.AppendLine(string.Format("Transaction Count:{0}<br />", _cit.Transactions.Where(x => x.Currency.code == groupKey.code).Count()));
                stringBuilder.AppendLine("<table style=\"text - align: left\">");
                stringBuilder.AppendLine("<tr><th>Denomination</th><th>Count</th><th>Sub Total</th></tr>");
                foreach (CITDenomination citDenomination in (IEnumerable<CITDenomination>)source)
                {
                    double num1 = citDenomination.denom / 100.0;
                    long count = citDenomination.count;
                    double num2 = citDenomination.denom * citDenomination.count / 100.0;
                    int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
                    stringBuilder.AppendLine(string.Format("<tr><td>{0:##,0.##}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", num1, count, num2));
                }
                stringBuilder.AppendLine(string.Format("<tr><td>{0}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", "TOTAL:", source.Sum(x => x.count), source.Sum(x => x.subtotal) / 100.0));
                stringBuilder.AppendLine("</table>");
            }
            return stringBuilder.ToString();
        }

        protected new string GenerateRawTextMessageToken()
        {
            StringBuilder stringBuilder = new StringBuilder(byte.MaxValue);
            foreach (IGrouping<Currency, CITDenomination> source in _cit.CITDenominations.GroupBy(x => x.Currency))
            {
                Currency groupKey = source.Key;
                stringBuilder.AppendLine("----------------------------------------");
                stringBuilder.AppendLine(string.Format("Currency: {0}", groupKey.code.ToUpper()));
                stringBuilder.AppendLine(string.Format("Transaction Count:{0}", _cit.Transactions.Where(x => x.Currency.code == groupKey.code).Count()));
                stringBuilder.AppendLine("----------------------------------------");
                stringBuilder.AppendLine(string.Format("{0,-10}{1,7}{2,21}", "Denomination", "Count", "Sub Total"));
                stringBuilder.AppendLine("________________________________________");
                foreach (CITDenomination citDenomination in (IEnumerable<CITDenomination>)source)
                {
                    double num1 = citDenomination.denom / 100.0;
                    long count = citDenomination.count;
                    double num2 = citDenomination.denom * citDenomination.count / 100.0;
                    int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
                    stringBuilder.AppendLine(string.Format("{0,-10:0.##}{1,7:##,0}{2,23:##,0.##}", num1, count, num2));
                }
                stringBuilder.AppendLine("========================================");
                stringBuilder.AppendLine(string.Format("{0,-10}{1,7:##,0}{2,23:##,0.##}", "TOTAL:", source.Sum(x => x.count), source.Sum(x => x.subtotal) / 100.0));
                stringBuilder.AppendLine("========================================");
            }
            return stringBuilder.ToString();
        }

        protected new string GenerateSMSMessageToken()
        {
            StringBuilder stringBuilder = new StringBuilder(byte.MaxValue);
            foreach (IGrouping<Currency, CITDenomination> source in _cit.CITDenominations.GroupBy(x => x.Currency))
            {
                Currency groupKey = source.Key;
                stringBuilder.AppendLine("CCY: " + groupKey.code.ToUpper());
                stringBuilder.AppendLine(string.Format("Tx: {0}", _cit.Transactions.Where(x => x.Currency == groupKey).Count()));
                stringBuilder.AppendLine("Notes: " + source.Sum(x => x.count).ToString());
                stringBuilder.AppendLine(string.Format("{0} Total: {1:0.##}", groupKey.code.ToUpper(), source.Sum(x => x.subtotal) / 100.0));
            }
            return stringBuilder.ToString();
        }

        private new string GetHTMLBody()
        {
            string htmlBody = AlertType.email_content_template;
            if (htmlBody != null)
            {
                foreach (KeyValuePair<string, string> token in (IEnumerable<KeyValuePair<string, string>>)Tokens)
                    htmlBody = htmlBody.Replace(token.Key, token.Value);
            }
            return htmlBody;
        }

        private new string GetRawTextBody()
        {
            string rawTextBody = AlertType.raw_email_content_template;
            if (rawTextBody != null)
            {
                foreach (KeyValuePair<string, string> token in (IEnumerable<KeyValuePair<string, string>>)Tokens)
                    rawTextBody = rawTextBody.Replace(token.Key, token.Value);
            }
            return rawTextBody;
        }

        private new string GetSMSBody()
        {
            string smsBody = AlertType?.phone_content_template;
            if (smsBody != null)
            {
                foreach (KeyValuePair<string, string> token in (IEnumerable<KeyValuePair<string, string>>)Tokens)
                    smsBody = smsBody.Replace(token.Key, token.Value);
            }
            return smsBody;
        }

        private new AlertEvent GetCorrespondingAlertEvent(DepositorDBContext DBContext) => throw new NotImplementedException();
    }
}
