
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace CashSwiftDeposit.Utils.AlertClasses
{
    public class AlertCITFailed : AlertBase
    {
        public const int ALERT_ID = 1300;
        private CIT _cit;

        public AlertCITFailed(CIT cit, Device device, DateTime dateDetected)
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
            var usernameStrt = new DepositorDBContext().ApplicationUsers.Where(x => x.id==_cit.start_user).FirstOrDefault();
            var usernameAuth = new DepositorDBContext().ApplicationUsers.Where(x => x.id==_cit.auth_user).FirstOrDefault();
            tokens4.Add("[cit_completed_date]", str4);
            Tokens.Add("[user_init]", usernameStrt.username);
            Tokens.Add("[user_auth]", usernameAuth.username);
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
