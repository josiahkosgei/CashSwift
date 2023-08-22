// Utils.AlertClasses.AlertTransactionCustomerAlert


using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;

namespace CashSwiftDeposit.Utils.AlertClasses
{
    public class AlertTransactionCustomerAlert : AlertBase
    {
        public const int ALERT_ID = 4009;
        private AppTransaction _transaction;

        public AlertTransactionCustomerAlert(
          AppTransaction transaction,
          Device device,
          DateTime dateDetected)
          : base(device, dateDetected)
        {
            _transaction = transaction != null ? transaction : throw new NullReferenceException("Variable transaction cannot be null in " + GetType().Name);
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                AlertType = depositorDbContext.AlertMessageTypes.FirstOrDefault(x => x.id == 4009);
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
                sent = false,
                to = _transaction.Phone
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
            Tokens.Add("[date_detected]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            Tokens.Add("[device.name]", Device.name);
            Tokens.Add("[transaction.end_date]", _transaction.EndDate.ToString(ApplicationViewModel.DeviceConfiguration.SMS_DATE_FORMAT ?? "d/M/yy 'at' h:mm tt", CultureInfo.InvariantCulture));
            Tokens.Add("[transaction.currency]", _transaction.CurrencyCode);
            Tokens.Add("[transaction.total_amount]", _transaction.TotalDisplayAmount.ToString(ApplicationViewModel.DeviceConfiguration.SMS_AMOUNT_FORMAT ?? "#,#0.00", CultureInfo.InvariantCulture));
            Tokens.Add("[transaction.cr_account_number]", _transaction.AccountNumber);
            Tokens.Add("[transaction.depositor_name]", _transaction.DepositorName);
            Tokens.Add("[transaction.narration]", _transaction.Narration);
            Tokens.Add("[transaction.cb_tx_number]", _transaction.Transaction.cb_tx_number);
            Tokens.Add("[transaction.cr_account_name]", _transaction.AccountName);
            Tokens.Add("[transaction.branch_name]", _transaction.BranchName);
            Tokens.Add("[transaction.device_number]", _transaction.DeviceNumber);
            Tokens.Add("[transaction.funds_source]", _transaction.FundsSource);
            Tokens.Add("[transaction.id_number]", _transaction.IDNumber);
            Tokens.Add("[transaction.phone]", _transaction.Phone);
            Tokens.Add("[transaction.ref_account_number]", _transaction.ReferenceAccount);
            Tokens.Add("[transaction.ref_account_name]", _transaction.ReferenceAccountName);
            Tokens.Add("[transaction.start_date]", _transaction.StartDate.ToString(ApplicationViewModel.DeviceConfiguration.SMS_DATE_FORMAT ?? "d/M/yy 'at' h:mm tt", CultureInfo.InvariantCulture));
            Tokens.Add("[transaction.dr_account_number]", _transaction.SuspenseAccount);
            Tokens.Add("[transaction.transaction_type]", _transaction.TransactionType?.name);
            Tokens.Add("[bank.name]", Device.Branch.Bank.name);
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken() => _transaction.ToEmailString();

        protected new string GenerateRawTextMessageToken() => _transaction.ToRawTextString();

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
            string EventDetail = AlertType?.phone_content_template;
            if (EventDetail != null)
            {
                foreach (KeyValuePair<string, string> token in (IEnumerable<KeyValuePair<string, string>>)Tokens)
                    EventDetail = EventDetail.Replace(token.Key, token.Value);
            }
            ApplicationViewModel.Log.Info(nameof(AlertTransactionCustomerAlert), "Generated SMS Body", nameof(GetSMSBody), EventDetail);
            return EventDetail;
        }

        private new AlertEvent GetCorrespondingAlertEvent(DepositorDBContext DBContext) => throw new NotImplementedException();
    }
}
