using Newtonsoft.Json;

namespace CashSwift.API.Messaging.Authentication
{
    public class AuthenticationRequest : APIRequestBase
    {
        [JsonIgnore]
        private object ToStringWithHiddenLock = new object();

        [JsonProperty(Required = Required.Always)]
        public string Username { get; set; }

        public bool ShouldSerializePassword() => SerializeHidden;

        [JsonProperty(Required = Required.Always)]
        public string Password { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool IsADUser { get; set; }

        public override string ToStringWithHidden()
        {
            lock (ToStringWithHiddenLock)
            {
                SerializeHidden = true;
                string stringWithHidden = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                SerializeHidden = false;
                return stringWithHidden;
            }
        }

        [JsonIgnore]
        private bool SerializeHidden { get; set; }
    }
}
