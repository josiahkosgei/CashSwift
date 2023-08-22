using Newtonsoft.Json;
using System;

namespace CashSwift.API.Messaging
{
    public class PostTransactionData
    {
        
        public string SystemCode_Cred { get; set; }
        [JsonProperty(Required = Required.Always)]

        public string BankID { get; set; }
        [JsonProperty(Required = Required.Always)]

        public string CDM_Number { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string SystemCode_FT { get; set; }
        [JsonProperty(Required = Required.Always)]
        public Guid TransactionID { get; set; }

        [JsonProperty(Required = Required.Always)]
        public PostBankAccount DebitAccount { get; set; }

        [JsonProperty(Required = Required.Always)]
        public PostBankAccount CreditAccount { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Decimal Amount { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Guid DeviceID { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string DeviceNumber { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime DateTime { get; set; }

        public string Narration { get; set; }
    }
}
