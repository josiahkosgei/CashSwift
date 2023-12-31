﻿using Newtonsoft.Json;
using System;

namespace CashSwift.API.Messaging
{
    public abstract class APIDeviceRequestBase : APIRequestBase
    {
        [JsonProperty(Required = Required.Always)]
        public Guid DeviceID { get; set; }
    }
}
