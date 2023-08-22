// Utils.AlertClasses.AlertNoteJamClearSuccess


using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace CashSwiftDeposit.Utils.AlertClasses
{
    public class AlertNoteJamClearSuccess : AlertBase
    {
        public const int ALERT_ID = 4002;
        private Transaction _transaction;
        private AlertEvent associatedAlertEvent;

        public AlertNoteJamClearSuccess(Transaction transaction, Device device, DateTime dateDetected)
          : base(device, dateDetected)
        {
            _transaction = transaction ?? throw new NullReferenceException("Variable transaction cannot be null in " + GetType().Name);
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                associatedAlertEvent = depositorDbContext.AlertEvents.Where(x => x.alert_type_id == 4002).OrderByDescending(x => x.created).FirstOrDefault();
                AlertType = depositorDbContext.AlertMessageTypes.FirstOrDefault(x => x.id == 4002);
            }
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
                    AlertEmail email = GenerateEmail();
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
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
            }
            return false;
        }

        private AlertEmail GenerateEmail()
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
            return alertEmail.raw_text_message == null || alertEmail.html_message == null ? null : alertEmail;
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
            Tokens.Add("[event_id]", AlertType.id.ToString() ?? "");
            Tokens.Add("[event_name]", AlertType.name);
            Tokens.Add("[event_description]", AlertType.description);
            IDictionary<string, string> tokens1 = Tokens;
            AlertEvent associatedAlertEvent = this.associatedAlertEvent;
            DateTime dateTime;
            string str1;
            if (associatedAlertEvent == null)
            {
                str1 = null;
            }
            else
            {
                dateTime = associatedAlertEvent.created;
                str1 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }
            tokens1.Add("[date_detected]", str1);
            IDictionary<string, string> tokens2 = Tokens;
            dateTime = DateTime.Now;
            string str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            tokens2.Add("[date_resolved]", str2);
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken()
        {
            StringBuilder stringBuilder1 = new StringBuilder(byte.MaxValue);
            stringBuilder1.AppendLine("<hr /><h3>Transaction</h3><hr />");
            StringBuilder stringBuilder2 = stringBuilder1;
            object[] objArray = new object[16]
            {
         _transaction.tx_start_date,
         _transaction.tx_end_date,
         _transaction.TransactionTypeListItem.name,
         _transaction.tx_account_number,
         _transaction.cb_account_name,
         _transaction.tx_ref_account,
         _transaction.cb_ref_account_name,
         _transaction.tx_narration,
         _transaction.tx_depositor_name,
         _transaction.tx_id_number,
         _transaction.tx_phone,
         _transaction.tx_currency,
        null,
        null,
        null,
        null
            };
            long? txAmount = _transaction.tx_amount;
            long num1 = 100;
            objArray[12] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num1) : new long?();
            objArray[13] = _transaction.funds_source;
            objArray[14] = _transaction.tx_error_code;
            objArray[15] = _transaction.tx_error_message;
            string str = string.Format("<p><table>\r\n                <tr><th>Start Date</th><th>End Date</th><th>Transaction Type</th><th>Account Number</th><th>Account Name</th><th>Reference Account Number</th><th>Reference Account Name</th>\r\n                <th>Narration</th><th>Depositor Name</th><th>Depositor ID</th><th>Depositor Phone</th><th>Currency</th><th>Amount</th><th>Source of Funds</th>\r\n                <th>Error Code</th><th>Error Message</th></tr>\r\n                <tr><td>{0}</td>  <td>{1}</td>  <td>{2}</td>  <td>{3}</td>  <td>{4}</td>  <td>{5}</td>  <td>{6}</td>  <td>{7}</td>  <td>{8}</td>  <td>{9}</td>  <td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td><td>{15}</td></tr>\r\n</table></p>", objArray);
            stringBuilder2.AppendLine(str);
            stringBuilder1.AppendLine("<p><table style=\"text - align: left\">");
            stringBuilder1.AppendLine("<tr><th>Denomination</th><th>Count</th><th>Sub Total</th></tr>");
            List<DenominationDetail> list = _transaction.DenominationDetails.ToList();
            foreach (DenominationDetail denominationDetail in list)
            {
                double num2 = denominationDetail.denom / 100.0;
                long count = denominationDetail.count;
                double num3 = denominationDetail.denom * denominationDetail.count / 100.0;
                int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
                stringBuilder1.AppendLine(string.Format("<tr><td>{0:##,0.##}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", num2, count, num3));
            }
            stringBuilder1.AppendLine(string.Format("<tr><td>{0}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", "TOTAL:", list.Sum(x => x.count), list.Sum(x => x.subtotal) / 100.0));
            stringBuilder1.AppendLine("</table></p>");
            return stringBuilder1.ToString();
        }

        protected new string GenerateRawTextMessageToken()
        {
            StringBuilder stringBuilder1 = new StringBuilder(byte.MaxValue);
            stringBuilder1.AppendLine("----------------------------------------");
            stringBuilder1.AppendLine("             Transaction");
            stringBuilder1.AppendLine("----------------------------------------");
            StringBuilder stringBuilder2 = stringBuilder1;
            object[] objArray = new object[16]
            {
         _transaction.tx_start_date,
         _transaction.tx_end_date,
         _transaction.TransactionTypeListItem.name,
         _transaction.tx_account_number,
         _transaction.cb_account_name,
         _transaction.tx_ref_account,
         _transaction.cb_ref_account_name,
         _transaction.tx_narration,
         _transaction.tx_depositor_name,
         _transaction.tx_id_number,
         _transaction.tx_phone,
         _transaction.tx_currency,
        null,
        null,
        null,
        null
            };
            long? txAmount = _transaction.tx_amount;
            long num1 = 100;
            objArray[12] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num1) : new long?();
            objArray[13] = _transaction.funds_source;
            objArray[14] = _transaction.tx_error_code;
            objArray[15] = _transaction.tx_error_message;
            string str = string.Format("\r\nStart Date:                 {0}\r\nEnd Date:                   {1}\r\nTransaction Type:           {2}\r\nAccount Number:             {3}\r\nAccount Name:               {4}\r\nReference Account Number:   {5}\r\nReference Account Name:     {6}            \r\nNarration:                  {7}\r\nDepositor Name:             {8}\r\nDepositor ID:               {9}\r\nDepositor Phone:            {10}\r\nCurrency:                   {11}\r\nAmount:                     {12}\r\nSource of Funds:            {13}\r\nError Code:                 {14}\r\nError Message:              {15}", objArray);
            stringBuilder2.AppendLine(str);
            stringBuilder1.AppendLine("________________________________________");
            stringBuilder1.AppendLine(string.Format("{0,-10}{1,7}{2,21}", "Denomination", "Count", "Sub Total"));
            stringBuilder1.AppendLine("________________________________________");
            List<DenominationDetail> list = _transaction.DenominationDetails.ToList();
            foreach (DenominationDetail denominationDetail in list)
            {
                double num2 = denominationDetail.denom / 100.0;
                long count = denominationDetail.count;
                double num3 = denominationDetail.denom * denominationDetail.count / 100.0;
                int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
                stringBuilder1.AppendLine(string.Format("{0,-10:0.##}{1,7:##,0}{2,23:##,0.##}", num2, count, num3));
            }
            stringBuilder1.AppendLine("========================================");
            stringBuilder1.AppendLine(string.Format("{0,-10}{1,7:##,0}{2,23:##,0.##}", "TOTAL:", list.Sum(x => x.count), list.Sum(x => x.subtotal) / 100.0));
            stringBuilder1.AppendLine("========================================");
            return stringBuilder1.ToString();
        }

        protected new string GenerateSMSMessageToken() => null;

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
