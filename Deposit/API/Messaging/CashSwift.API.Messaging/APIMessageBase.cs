﻿using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace CashSwift.API.Messaging
{
    public class APIMessageBase
    {
        // [JsonProperty(Required = Required.Always)]
        public string SessionID { get; set; }

        // [JsonProperty(Required = Required.Always)]
        public string MessageID { get; set; }

        // [JsonProperty(Required = Required.Always)]
        public DateTime MessageDateTime { get; set; }

        // [JsonProperty(Required = Required.Always)]
        // [DataMember]
        public Guid AppID { get; set; }

        // [JsonProperty(Required = Required.Always)]
        // [DataMember(IsRequired = true)]
        public string AppName { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        public virtual string ToStringWithHidden() => ToString();
    }
}
