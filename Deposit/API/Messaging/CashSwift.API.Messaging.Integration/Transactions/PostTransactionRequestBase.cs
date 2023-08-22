using Newtonsoft.Json;

namespace CashSwift.API.Messaging.Integration.Transactions
{
  public class PostTransactionRequestBase : APIDeviceRequestBase
  {
    [JsonProperty(Required = Required.Always)]
    public PostTransactionData Transaction { get; set; }
  }
}
