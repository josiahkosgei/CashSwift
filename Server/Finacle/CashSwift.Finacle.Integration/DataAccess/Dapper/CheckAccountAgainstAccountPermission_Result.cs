using Newtonsoft.Json;


namespace CashSwift.Finacle.Integration.DataAccess.Dapper
{
    public class CheckAccountAgainstAccountPermission_Result
    {
        public Guid id { get; set; }

        public byte[] Icon { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string currency { get; set; }

        public string account_number { get; set; }

        public string account_name { get; set; }

        public bool enabed { get; set; } = true;


        public string error_message { get; set; }
    }
}
