using Newtonsoft.Json;

namespace CashSwift.API.Messaging
{
    public abstract class APIRequestBase : APIMessageBase
    {
        [JsonProperty(Required = Required.Always)]
        public string Language { get; set; } = "en-gb";
    }
}
