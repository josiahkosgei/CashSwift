using CashSwift.API.Messaging.APIClients;
using CashSwift.API.Messaging.Communication.Emails;
using CashSwift.API.Messaging.Communication.SMSes;
using CashSwift.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Communication.Clients
{
    public class CommunicationServiceClient : APIClient, IEmailController, ISMSController
    {
        public CommunicationServiceClient(
          string apiBaseAddress,
          Guid AppID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashSwiftAPILogger(nameof(CommunicationServiceClient), configuration), apiBaseAddress, AppID, appKey, configuration)
        {
        }

        public async Task<EmailResponse> SendEmailAsync(EmailRequest request) => await SendAsync<EmailResponse>("api/Email/SendEmail", request);

        public async Task<SMSResponse> SendSMSAsync(SMSRequest request) => await SendAsync<SMSResponse>("api/SMS/SendSMS", request);
    }
}
