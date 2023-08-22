using Newtonsoft.Json;

namespace CashSwift.API.Messaging.Communication.Emails
{
    public class EmailRequest : APIRequestBase
    {
        [JsonProperty(Required = Required.Always)]
        public EmailMessage Message { get; set; }
    }
}
