using Newtonsoft.Json;

namespace CashSwift.API.Messaging
{
    public class PostTransactionRequestBase : APIDeviceRequestBase
    {
        [JsonProperty(Required = Required.Always)]
        public PostTransactionData Transaction { get; set; }
    }
}
