using Newtonsoft.Json;

namespace CashSwift.API.Messaging.Integration.Transactions
{
    public class PostTransactionRequest : PostTransactionRequestBase
    {

        public string SystemCode_Cred { get; set; }
        [JsonProperty(Required = Required.Always)]

        public string BankID { get; set; }
        [JsonProperty(Required = Required.Always)]

        public string CDM_Number { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string SystemCode_FT { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string TransactionType { get; set; }

        public int TransactionTypeID { get; set; }

        public string RefAccountNumber { get; set; }

        public string RefAccountName { get; set; }

        public string DepositorName { get; set; }

        public string DepositorIDNumber { get; set; }

        public string DepositorPhone { get; set; }

        public string FundsSource { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string DeviceReferenceNumber { get; set; }
    }
}
