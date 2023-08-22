﻿using Newtonsoft.Json;

namespace CashSwift.API.Messaging
{
    public class PostBankAccount
    {
        [JsonProperty(Required = Required.Always)]
        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Currency { get; set; }
    }
}
