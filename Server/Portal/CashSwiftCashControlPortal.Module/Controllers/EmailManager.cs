
//Controllers.EmailManager


using CashSwift.Library.Standard.Logging;
using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Server;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    internal class EmailManager
    {
        public CashSwiftLogger log = new CashSwiftLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), "WebPortalLog", null);
        public EmailConfiguration EmailConfiguration;

        public EmailManager()
        {
        }

        public EmailManager(Session session) => EmailConfiguration = JsonConvert.DeserializeObject<EmailConfiguration>((session.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_EMAIL_CONFIG")) ?? throw new NullReferenceException("EMAIL_CONFIG not found")).config_value);

        public EmailManager(IObjectSpace objectSpace) => EmailConfiguration = JsonConvert.DeserializeObject<EmailConfiguration>((objectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_EMAIL_CONFIG")) ?? throw new NullReferenceException("EMAIL_CONFIG not found")).config_value);

        public void SendNewUserEmail(ApplicationUser user, string password)
        {
            log.Trace(nameof(EmailManager), "Processing", nameof(SendNewUserEmail), "Inside method for user {0}", new object[1]
            {
         user?.UserName
            });
            if (string.IsNullOrWhiteSpace(EmailConfiguration.EMAIL_FROM))
            {
                log.Error(nameof(EmailManager), "Invalid Configuration", nameof(SendNewUserEmail), "EMAIL_FROM is invalid", Array.Empty<object>());
                throw new Exception("EMAIL_FROM is invalid");
            }
            SendEmail(GenerateNewUserEmail(user, password));
            log.Debug(nameof(EmailManager), "Processing", nameof(SendNewUserEmail), "End method for user {0}", new object[1]
            {
         user?.UserName
            });
        }

        public void SendPasswordResetEmail(ApplicationUser user, string password)
        {
            log.Trace(nameof(EmailManager), "Processing", nameof(SendPasswordResetEmail), "Inside method", Array.Empty<object>());
            SendEmail(GeneratePasswordResetEmail(user, password));
            log.Debug(nameof(EmailManager), "Processing", nameof(SendPasswordResetEmail), "End method", Array.Empty<object>());
        }

        public void SendEmail(MailMessage mailMessage)
        {
            log.Trace(nameof(EmailManager), "Processing", nameof(SendEmail), "Inside method", Array.Empty<object>());
            SmtpClient smtpClient = new SmtpClient(EmailConfiguration.EMAIL_HOST, EmailConfiguration.EMAIL_PORT)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Timeout = EmailConfiguration.EMAIL_TIMEOUT,
                Credentials =  new NetworkCredential(EmailConfiguration.EMAIL_USERNAME, EmailConfiguration.EMAIL_PASSWORD),
                EnableSsl = EmailConfiguration.EMAIL_ENABLE_SSL
            };
            try
            {
                log.Debug(nameof(EmailManager), "Attempt", nameof(SendEmail), "sending email '{0}' to '{1}' SUCCESS", new object[2]
                {
           mailMessage.Subject,
           mailMessage?.To[0]?.Address
                });
                smtpClient.Send(mailMessage);
                log.Info(nameof(EmailManager), "SUCCESS", nameof(SendEmail), "sent email '{0}' to '{1}' SUCCESS", new object[2]
                {
           mailMessage.Subject,
           mailMessage?.To[0]?.Address
                });
            }
            catch (NullReferenceException ex)
            {
                log.Error(nameof(EmailManager), ex.GetType().Name, nameof(SendEmail), ex.MessageString(), Array.Empty<object>());
            }
            catch (SmtpFailedRecipientsException ex)
            {
                log.Error(nameof(EmailManager), ex.GetType().Name, nameof(SendEmail), ex.MessageString(), Array.Empty<object>());
            }
            catch (Exception ex)
            {
                log.Error(nameof(EmailManager), ex.GetType().Name, nameof(SendEmail), ex.MessageString(), Array.Empty<object>());
            }
        }

        public MailMessage GeneratePasswordResetEmail(ApplicationUser user, string password)
        {
            log.Trace(nameof(EmailManager), "Processing", nameof(GeneratePasswordResetEmail), "Generate template", Array.Empty<object>());
            string str = "Dear " + user.fname + ",\r\n\r\nYour account password has been reset successfully by your administrator.\r\n\r\nKindly use the following credentials to login to :\r\nurl:        " + EmailConfiguration.WEB_PORTAL_URI + "\r\nusername:   " + user.UserName + "\r\npassword:   " + password + "\r\n\r\nIf you did not request a password reset, or if this is not your account, contact your administrator.\r\n\r\nRegards,\r\n\r\n\r\n";
            MailMessage passwordResetEmail = new MailMessage
            {
                IsBodyHtml = false,
                Body = str,
                From = new MailAddress(EmailConfiguration.EMAIL_FROM)
            };
            passwordResetEmail.To.Add(new MailAddress(user.email, string.Format("{0} {1}", user.fname, user.lname)));
            passwordResetEmail.Subject = "[CDM][] Password Reset";
            log.Trace(nameof(EmailManager), "Processing", nameof(GeneratePasswordResetEmail), "Generated email {0}", new object[1]
            {
         passwordResetEmail.Subject
            });
            return passwordResetEmail;
        }

        public MailMessage GenerateNewUserEmail(ApplicationUser user, string password)
        {
            log.Trace(nameof(EmailManager), "Processing", nameof(GenerateNewUserEmail), "Generate template", Array.Empty<object>());
            string str = "Dear " + user.fname + ",\r\n\r\nYour account for the  System has been created successfully by your administrator.\r\n\r\nKindly use the following credentials to login to :\r\n" + (user.Roles.Count() > 0 ? "url:        " + EmailConfiguration.WEB_PORTAL_URI : "") + "\r\nusername:   " + user.UserName + "\r\n" + (user.IsActiveDirectoryUser ? "Login using your Active Directory credentials." : "password: " + password) + "\r\n\r\nIf you did not request a password reset, or if this is not your account, contact your administrator.\r\n\r\nRegards,\r\n\r\n\r\n";
            MailMessage newUserEmail = new MailMessage
            {
                IsBodyHtml = false,
                Body = str,
                From = new MailAddress(EmailConfiguration.EMAIL_FROM)
            };
            newUserEmail.To.Add(new MailAddress(user.email, string.Format("{0} {1}", user.fname, user.lname)));
            newUserEmail.Subject = "[CDM][] User Creation";
            log.Trace(nameof(EmailManager), "Processing", nameof(GenerateNewUserEmail), "Generated email {0}", new object[1]
            {
         newUserEmail.Subject
            });
            return newUserEmail;
        }
    }
}
