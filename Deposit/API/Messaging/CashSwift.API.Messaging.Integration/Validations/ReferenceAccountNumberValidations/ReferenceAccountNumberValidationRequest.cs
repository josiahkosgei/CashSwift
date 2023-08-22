using CashSwift.API.Messaging.Models;
using Newtonsoft.Json;

namespace CashSwift.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations
{
    public class ReferenceAccountNumberValidationRequest : ValidationRequestBase
    {
        [JsonProperty(Required = Required.Always)]
        public string ReferenceAccountNumber { get; set; }
    }
}
