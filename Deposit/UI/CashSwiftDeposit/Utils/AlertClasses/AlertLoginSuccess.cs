﻿// Utils.AlertClasses.AlertLoginSuccess


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
    internal class AlertLoginSuccess : AlertBase
    {
        public const int ALERT_ID = 3002;
        private string _errorMessage;
        private ApplicationUser _user;
        private string _username;

        public AlertLoginSuccess(
          Device device,
          DateTime dateDetected,
          string errorMessage,
          string username,
          ApplicationUser user = null)
          : base(device, dateDetected)
        {
            _user = user;
            _errorMessage = errorMessage;
            _username = username;
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                AlertType = depositorDbContext.AlertMessageTypes.FirstOrDefault(x => x.id == 3002);
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
            Tokens.Add("[login_username]", _username);
            Tokens.Add("[login_error]", _errorMessage);
            Tokens.Add("[login_user_fname]", _user?.fname);
            Tokens.Add("[login_user_lname]", _user?.lname);
            Tokens.Add("[login_user_username]", _user?.username);
            Tokens.Add("[login_user_email]", _user?.email);
            Tokens.Add("[login_user_phone]", _user?.phone);
            IDictionary<string, string> tokens = Tokens;
            ApplicationUser user = _user;
            bool? nullable1;
            bool? nullable2;
            if (user == null)
            {
                nullable2 = new bool?();
            }
            else
            {
                nullable1 = user.depositor_enabled;
                nullable2 = nullable1.HasValue ? new bool?(!nullable1.GetValueOrDefault()) : new bool?();
            }
            nullable1 = nullable2;
            string str = nullable1.ToString();
            tokens.Add("[login_user_islocked]", str);
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken() => "";

        protected new string GenerateRawTextMessageToken() => "";

        protected new string GenerateSMSMessageToken() => "";

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
