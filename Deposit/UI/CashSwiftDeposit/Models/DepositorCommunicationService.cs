using CashSwift.API.Messaging.Communication.Clients;
using CashSwift.API.Messaging.Communication.Emails;
using CashSwift.API.Messaging.Communication.SMSes;
using CashSwift.Library.Standard.Logging;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CashSwiftDeposit.Models
{
    internal class DepositorCommunicationService : IDisposable
    {
        private static DepositorCommunicationService instance;
        private static ICashSwiftLogger Log;
        private static string commserv_uri;
        private static Guid AppID;
        private static byte[] AppKey;
        private static string AppName;
        private BackgroundWorker EmailSendingWorker = new BackgroundWorker();
        private bool quitSendingEmail = false;
        private int alert_batch_size = Math.Max(ApplicationViewModel.DeviceConfiguration.ALERT_BATCH_SIZE, 5);

        private DepositorCommunicationService()
        {
            EmailSendingWorker.DoWork += new DoWorkEventHandler(EmailSendingWorker_DoWork);
            EmailSendingWorker.RunWorkerAsync();
        }
        public static DepositorCommunicationService GetDepositorCommunicationService() => instance;

        public static DepositorCommunicationService NewDepositorCommunicationService(
          string CommServURI,
          Guid appID,
          byte[] appKey,
          string appName)
        {
            if (!ApplicationViewModel.DeviceConfiguration.EMAIL_CAN_SEND && !ApplicationViewModel.DeviceConfiguration.SMS_CAN_SEND)
                return null;
            if (instance != null)
                instance.Dispose();
            if (instance == null)
            {
                Log = new CashSwiftLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), nameof(DepositorCommunicationService), null);
                AppID = appID;
                AppKey = appKey;
                AppName = appName;
                commserv_uri = CommServURI;
                instance = new DepositorCommunicationService();
            }
            return GetDepositorCommunicationService();
        }

        private void EmailSendingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.Debug(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "EmailSendingWorker_DoWork started");
            while (!quitSendingEmail)
            {
                try
                {
                    using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                    {
                        if (ApplicationViewModel.DeviceConfiguration.EMAIL_CAN_SEND || ApplicationViewModel.DeviceConfiguration.SMS_CAN_SEND)
                        {
                            List<AlertEvent> list = depositorDbContext.AlertEvents.Where(x => x.is_processed == false).OrderBy(y => y.date_detected).Take(alert_batch_size).ToList();
                            Log.Trace(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "Found {0} AlertEvents to process", list.Count());
                            foreach (AlertEvent alertEvent in list)
                            {
                                try
                                {
                                    Log.Info(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), string.Format("processing AlertEvent {0}", alertEvent));
                                    ProcessEmail(alertEvent, depositorDbContext);
                                    ProcessSMS(alertEvent, depositorDbContext);
                                    Log.Info(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "alertEvent [{0}] processed SUCCESS", alertEvent.ToString());
                                    alertEvent.is_processed = true;
                                }
                                catch (NullReferenceException ex)
                                {
                                    Log.Info(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "alertEvent [{0}] processed ERROR", alertEvent.ToString());
                                    Log.Error(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), "NullReferenceException", "Error processing alertEvent [{0}]: {1}", alertEvent.ToString(), ex.MessageString());
                                    alertEvent.is_processed = true;
                                    SaveToDatabase(depositorDbContext);
                                    Thread.Sleep(Math.Max(1000, ApplicationViewModel.DeviceConfiguration.EMAIL_SEND_INTERVAL));
                                }
                                catch (Exception ex)
                                {
                                    Log.Info(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "alertEvent [{0}] processed ERROR", alertEvent.ToString());
                                    Log.Error(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), "Exception", "Error processing alertEvent [{0}]: {1}", alertEvent.ToString(), ex.MessageString());
                                    alertEvent.is_processed = false;
                                    SaveToDatabase(depositorDbContext);
                                    Thread.Sleep(Math.Max(1000, ApplicationViewModel.DeviceConfiguration.EMAIL_SEND_INTERVAL));
                                }
                                SaveToDatabase(depositorDbContext);
                            }
                        }
                        else
                            Log.Debug(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "Error: CanSendEmail = {0}, CanSendSMS = {1}", ApplicationViewModel.DeviceConfiguration.EMAIL_CAN_SEND, ApplicationViewModel.DeviceConfiguration.SMS_CAN_SEND);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), "Exception", "Error processing: {0}", ex.MessageString());
                }
                Log.Trace(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "while loop completed, sleeping...");
                Thread.Sleep(Math.Max(1000, ApplicationViewModel.DeviceConfiguration.ALERT_DB_POLL_INTERVAL));
            }
        }

        private void SaveToDatabase(DepositorDBContext DBContext)
        {
            try
            {
                ApplicationViewModel.SaveToDatabase(DBContext);
            }
            catch (Exception ex)
            {
            }
        }

        private EmailRequest GenerateEmail(
          Device device,
          AlertEmail alertEmail,
          ApplicationUser recipient)
        {
            using (new DepositorDBContext())
            {
                Log.Debug(nameof(DepositorCommunicationService), nameof(GenerateEmail), nameof(GenerateEmail), "Generating MailMessage for AlertEmail:{0} and recipient: {1}", alertEmail.ToString(), recipient?.email);
                if (alertEmail == null)
                    throw new NullReferenceException("alertEmail cannot be null.");
                EmailRequest email = new EmailRequest();
                email.Message = new EmailMessage()
                {
                    HTMLContent = PersonaliseMessage(alertEmail.html_message, recipient),
                    ToAddresses = new List<EmailAddress>()
          {
            new EmailAddress()
            {
              Name = recipient?.FullName,
              Address = recipient?.email
            }
          },
                    Subject = device.name + ": " + alertEmail.subject.Trim()
                };
                email.MessageDateTime = DateTime.Now;
                Guid guid = alertEmail.id;
                email.SessionID = guid.ToString();
                guid = Guid.NewGuid();
                email.MessageID = guid.ToString();
                email.AppID = AppID;
                email.AppName = AppName;
                return email;
            }
        }

        private SMSRequest GenerateSMS(
          AlertSMS alertSMS,
          ApplicationUser recipient,
          string phone)
        {
            using (new DepositorDBContext())
            {
                Log.Debug(nameof(DepositorCommunicationService), nameof(GenerateSMS), nameof(GenerateSMS), "Generating SMS for AlertSMS:{0} and recipient: {1}", alertSMS.ToString(), recipient?.phone);
                if (alertSMS == null)
                    throw new NullReferenceException("alertSMS cannot be null.");
                SMSRequest sms = new SMSRequest();
                sms.AppID = AppID;
                sms.AppName = AppName;
                sms.SMSMessage = new SMSMessage()
                {
                    MessageText = PersonaliseMessage(alertSMS.message, recipient),
                    ToContacts = new List<SMSContact>()
          {
            new SMSContact() { PhoneNumber = phone }
          }
                };
                sms.MessageDateTime = DateTime.Now;
                sms.SessionID = alertSMS.id.ToString();
                sms.MessageID = Guid.NewGuid().ToString();
                return sms;
            }
        }

        private string PersonaliseMessage(string message, ApplicationUser user)
        {
            Log.Debug(nameof(DepositorCommunicationService), nameof(PersonaliseMessage), nameof(PersonaliseMessage), "Personalising an email message for user:{0} ", user?.email);
            string str = message;
            if (user != null)
                str = str.Replace("[user_fname]", user?.fname).Replace("[user_lname]", user?.lname).Replace("[user_username]", user?.username).Replace("[user_email]", user?.email).Replace("[user_phone]", user?.phone).Replace("[user_role_name]", user?.role.name);
            return str;
        }

        private void ProcessEmail(AlertEvent alertEvent, DepositorDBContext DepositorDBContext)
        {
            if (!ApplicationViewModel.DeviceConfiguration.EMAIL_CAN_SEND)
                return;
            List<AlertEmail> list1 = DepositorDBContext.AlertEmails.Where(x => x.alert_event_id == alertEvent.id).ToList();
            Device device = DepositorDBContext.Devices.FirstOrDefault(x => x.id == alertEvent.device_id);
            if (device == null)
                throw new NullReferenceException("device cannot be null in EmailSendingWorker_DoWork()");
            Parallel.ForEach(list1, alertEmail =>
            {
                Log.Info(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "processing alertEmail {0}", alertEmail.id);
                try
                {
                    List<AlertEmailAttachment> alertEmailAttachmentList = new List<AlertEmailAttachment>();
                    List<EmailAttachment> alertEmailAttachments = new List<EmailAttachment>(5);
                    if (ApplicationViewModel.DeviceConfiguration.EMAIL_SEND_ATTACHMENT)
                        alertEmailAttachments = DepositorDBContext.AlertEmailAttachments.Where(y => y.alert_email_id == alertEmail.id).ToList().Join(DepositorDBContext.AlertAttachmentTypes, alertEmailAttachment => alertEmailAttachment.type, alertAttachmentType => alertAttachmentType.code, (alertEmailAttachment, alertAttachmentType) => new EmailAttachment()
                        {
                            Name = alertEmailAttachment.name,
                            MimeType = new EmailAttachmentMIMEType()
                            {
                                MimeType = alertAttachmentType.mime_type,
                                MimeSubType = alertAttachmentType.mime_subtype
                            },
                            Data = alertEmailAttachment.data
                        }).ToList();
                    else
                        Log.Warning(nameof(DepositorCommunicationService), "Attachments Disabled", "GenerateEmail", "Email attachment sending is disabled in config");
                    Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "get the recipients");
                    var deviceUsersByDevice = DepositorDBContext.ApplicationUsers.Where(x => x.user_group == device.user_group).ToList(); //DepositorDBContext.GetDeviceUsersByDevice((int?)device?.user_group);
                    List<ApplicationUser> applicationUserList;
                    if (deviceUsersByDevice == null)
                    {
                        applicationUserList = null;
                    }
                    else
                    {
                        var list2 = deviceUsersByDevice;
                        if (list2 == null)
                        {
                            applicationUserList = null;
                        }
                        else
                        {
                            IEnumerable<ApplicationUser> source = list2.Where(deviceUser => (bool)deviceUser.email_enabled && !string.IsNullOrEmpty(deviceUser.email) && deviceUser.role.AlertMessageRegistries.FirstOrDefault(alertMessageRegistryItem => alertMessageRegistryItem.alert_type_id == alertEvent.alert_type_id) != null);
                            applicationUserList = source != null ? source.ToList() : null;
                        }
                    }
                    IList<ApplicationUser> source1 = applicationUserList;
                    Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "found {0} recipients", source1.Count);
                    alertEmail.to = string.Join("|", source1 != null ? source1.Where(x => x?.email != null).Select(x => x.email).ToList() : (IEnumerable<string>)null);
                    alertEmail.to = string.IsNullOrWhiteSpace(alertEmail.to) ? null : alertEmail.to;
                    Log.Trace(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "send emails to each recipient");
                    Parallel.ForEach(source1, async recipient =>
              {
                  try
                  {
                      EmailRequest emailTemplate = GenerateEmail(device, alertEmail, recipient);
                      emailTemplate.Message.Attachments = alertEmailAttachments;
                      Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "sending email {0} to {1}", alertEmail.ToString(), emailTemplate.Message.ToAddresses[0].Address);
                      await SendEmailAsync(emailTemplate);
                      Log.Info(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "sending email {0} to {1} SUCCESS", alertEmail.ToString(), emailTemplate.Message.ToAddresses[0].Address);
                      emailTemplate = null;
                  }
                  catch (TimeoutException ex)
                  {
                      Log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "TimeoutException", "TimeoutException sending alertEmail to [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                      throw;
                  }
                  catch (NullReferenceException ex)
                  {
                      Log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "NullReferenceException", "NullReferenceException sending alertEmail to [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                  }
                  catch (SmtpFailedRecipientsException ex)
                  {
                      Log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "SmtpFailedRecipientsException", "SmtpFailedRecipientsException sending alertEmail to [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                  }
                  catch (Exception ex)
                  {
                      Log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "Exception", "Exception sending alertEmail [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                  }
                  Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "processed recipient {0}", recipient?.email);
              });
                    Log.Info(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "alertEmail [{0}] processed SUCCESS", alertEmail.ToString());
                    alertEmail.sent = true;
                    alertEmail.send_date = new DateTime?(DateTime.Now);
                    alertEmail.send_error = false;
                    alertEmail.send_error_message = "";
                }
                catch (Exception ex)
                {
                    Log.Info(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "alertEmail [{0}] processed ERROR", alertEmail.id);
                    Log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "Exception", "Error processing alertEmail [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                    alertEmail.sent = false;
                    alertEmail.send_error = true;
                    alertEmail.send_error_message = ex.MessageString(new int?(200));
                }
            });
            SaveToDatabase(DepositorDBContext);
        }

        private void ProcessSMS(AlertEvent alertEvent, DepositorDBContext DepositorDBContext)
        {
            if (!ApplicationViewModel.DeviceConfiguration.SMS_CAN_SEND)
                return;
            Parallel.ForEach(DepositorDBContext.AlertSMSs.Where(x => x.alert_event_id == alertEvent.id).ToList(), alertSMS =>
            {
                Device device = DepositorDBContext.Devices.FirstOrDefault(x => x.id == alertEvent.device_id);
                if (device == null)
                    throw new NullReferenceException("device cannot be null in EmailSendingWorker_DoWork()");
                try
                {
                    Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSMS), nameof(ProcessSMS), "get the recipients");
                    var deviceUsersByDevice = DepositorDBContext.ApplicationUsers.Where(x => x.user_group == device.user_group);
                    List<ApplicationUser> applicationUserList;
                    if (deviceUsersByDevice == null)
                    {
                        applicationUserList = null;
                    }
                    else
                    {
                        List<ApplicationUser> list = deviceUsersByDevice.ToList();
                        if (list == null)
                        {
                            applicationUserList = null;
                        }
                        else
                        {
                            IEnumerable<ApplicationUser> source = list.Where(deviceUser => deviceUser.phone_enabled && deviceUser.role.AlertMessageRegistries.FirstOrDefault(alertMessageRegistryItem => alertMessageRegistryItem.alert_type_id == alertEvent.alert_type_id) != null);
                            applicationUserList = source != null ? source.ToList() : null;
                        }
                    }
                    IList<ApplicationUser> recipients = applicationUserList;
                    Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSMS), nameof(ProcessSMS), "found {0} recipients", recipients.Count);
                    List<string> stringList1 = new List<string>(recipients.Count + 1);
                    if (!string.IsNullOrWhiteSpace(alertSMS.to))
                        stringList1.Add(alertSMS.to);
                    List<string> stringList2 = stringList1;
                    IList<ApplicationUser> source1 = recipients;
                    List<string> collection = source1 != null ? source1.Where(x => x?.phone != null).Select(x => x.phone).ToList() : null;
                    stringList2.AddRange(collection);
                    alertSMS.to = string.Join("|", stringList1);
                    AlertSMS alertSm = alertSMS;
                    IList<ApplicationUser> source2 = recipients;
                    string str = string.Join("|", source2 != null ? source2.Where(x => x.phone_enabled && x.phone != null).Select(x => x.phone).ToList() : null);
                    alertSm.to = str;
                    alertSMS.to = string.IsNullOrWhiteSpace(alertSMS.to) ? null : alertSMS.to;
                    Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSMS), nameof(ProcessSMS), "send SMSes to each recipient");
                    Parallel.ForEach(stringList1, async phoneNumber =>
              {
                  SMSRequest SMSTemplate = GenerateSMS(alertSMS, recipients.FirstOrDefault(x => x.phone.Equals(phoneNumber, StringComparison.Ordinal)), phoneNumber);
                  try
                  {
                      Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSMS), nameof(ProcessSMS), "sending SMS {0} to {1}", alertSMS.ToString(), SMSTemplate.SMSMessage.ToContacts[0].PhoneNumber);
                      SMSResponse result = await SendSMSAsync(SMSTemplate);
                      Log.Info(nameof(DepositorCommunicationService), nameof(ProcessSMS), nameof(ProcessSMS), "sending SMS {0} to {1} SUCCESS", alertSMS.ToString(), SMSTemplate.SMSMessage.ToContacts[0].PhoneNumber);
                      result = null;
                  }
                  catch (TimeoutException ex)
                  {
                      Log.Error(nameof(DepositorCommunicationService), nameof(ProcessSMS), "TimeoutException", "TimeoutException sending alertSMS [{0}] to {1} : {2}", alertSMS.ToString(), SMSTemplate.SMSMessage.ToContacts[0].PhoneNumber, ex.MessageString());
                  }
                  catch (NullReferenceException ex)
                  {
                      Log.Error(nameof(DepositorCommunicationService), nameof(ProcessSMS), "NullReferenceException", "NullReferenceException sending alertSMS [{0}] to {1} : {2}", alertSMS.ToString(), SMSTemplate.SMSMessage.ToContacts[0].PhoneNumber, ex.MessageString());
                  }
                  catch (SmtpFailedRecipientsException ex)
                  {
                      Log.Error(nameof(DepositorCommunicationService), nameof(ProcessSMS), "SmtpFailedRecipientsException", "SmtpFailedRecipientsException sending alertSMS [{0}] to {1} : {2}", alertSMS.ToString(), SMSTemplate.SMSMessage.ToContacts[0].PhoneNumber, ex.MessageString());
                  }
                  catch (Exception ex)
                  {
                      Log.Error(nameof(DepositorCommunicationService), nameof(ProcessSMS), "Exception", "Exception sending alertSMS [{0}]: {1}", alertSMS.ToString(), ex.MessageString());
                  }
                  Log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSMS), "processed recipient {0}", phoneNumber);
                  SMSTemplate = null;
              });
                    Log.Info(nameof(DepositorCommunicationService), nameof(ProcessSMS), nameof(ProcessSMS), "alertSMS [{0}] processed SUCCESS", alertSMS.ToString());
                    alertSMS.sent = true;
                    alertSMS.send_date = new DateTime?(DateTime.Now);
                    alertSMS.send_error = false;
                    alertSMS.send_error_message = "";
                    SaveToDatabase(DepositorDBContext);
                }
                catch (Exception ex)
                {
                    Log.Info(nameof(DepositorCommunicationService), nameof(ProcessSMS), nameof(ProcessSMS), "alertSMS [{0}] processed ERROR", alertSMS.id);
                    Log.Error(nameof(DepositorCommunicationService), nameof(ProcessSMS), "Exception", "Error processing alertSMS [{0}]: {1}", alertSMS.ToString(), ex.MessageString());
                    alertSMS.sent = false;
                    alertSMS.send_error = true;
                    alertSMS.send_error_message = ex.MessageString(new int?(200));
                    throw;
                }
            });
        }

        public static async Task SendEmailAsync(EmailRequest email)
        {
            CommunicationServiceClient client = new CommunicationServiceClient(commserv_uri, AppID, AppKey, null);
            try
            {
                EmailResponse emailResponse = await client.SendEmailAsync(email);
            }
            catch (TimeoutException ex)
            {
                Log.Error(nameof(DepositorCommunicationService), "SendEmail", "TimeoutException", "Error sending email [{0}]: {1}", email.ToString(), ex.MessageString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(nameof(DepositorCommunicationService), "SendEmail", "Exception", "Error sending email [{0}]: {1}", email.ToString(), ex.MessageString());
                throw;
            }
            client = null;
        }

        public static async Task<SMSResponse> SendSMSAsync(SMSRequest sms)
        {
            CommunicationServiceClient client = new CommunicationServiceClient(commserv_uri, AppID, AppKey, null);
            SMSResponse smsResponse;
            try
            {
                smsResponse = await client.SendSMSAsync(sms);
            }
            catch (TimeoutException ex)
            {
                Log.Error(nameof(DepositorCommunicationService), "SendSMS", "TimeoutException", "Error sending SMS [{0}]: {1}", sms.ToString(), ex.MessageString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(nameof(DepositorCommunicationService), "SendSMS", "Exception", "Error sending SMS [{0}]: {1}", sms.ToString(), ex.MessageString());
                throw;
            }
            client = null;
            return smsResponse;
        }

        public void Dispose()
        {
            EmailSendingWorker?.Dispose();
            EmailSendingWorker = null;
        }
    }
}
