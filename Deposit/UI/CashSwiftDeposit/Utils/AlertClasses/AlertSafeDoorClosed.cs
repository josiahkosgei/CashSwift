using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace CashSwiftDeposit.Utils.AlertClasses
{
    internal class AlertSafeDoorClosed : AlertBase
    {
        public const int ALERT_ID = 1201;
        private bool _duringCIT;

        public AlertSafeDoorClosed(Device device, DateTime dateDetected, bool duringCIT = false)
          : base(device, dateDetected)
        {
            _duringCIT = duringCIT;
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                AlertType = depositorDbContext.AlertMessageTypes.FirstOrDefault(x => x.id == 1201);
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
            Tokens.Add("[date_detected]", DateDetected.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            Tokens.Add("[date_resolved]", "");
            Tokens.Add("[resolution_duration]", "");
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken() => "Door closed" + (_duringCIT ? " during CIT" : " outside normal operation");

        protected new string GenerateRawTextMessageToken() => "Door closed" + (_duringCIT ? " during CIT" : " outside normal operation");

        protected new string GenerateSMSMessageToken() => "Door closed" + (_duringCIT ? " during CIT" : " outside normal operation");

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
