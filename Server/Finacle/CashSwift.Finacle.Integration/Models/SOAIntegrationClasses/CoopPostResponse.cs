// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.CoopPostResponse

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses
{
    public class CoopPostResponse
    {
        public string RequestUUID { get; set; }

        public string Raw { get; set; }

        public bool Success { get; set; }

        public string StatusCode { get; set; }

        public string CBReversalReference { get; set; }

        public DateTime CBValueDateTime { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public string TransactionID { get; set; }

        public string PostResponseCode { get; set; }

        public string PostResponseMessage { get; set; }
    }
}