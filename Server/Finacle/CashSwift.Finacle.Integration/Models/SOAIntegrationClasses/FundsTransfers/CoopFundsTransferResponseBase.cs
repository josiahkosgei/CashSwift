// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.CoopFundsTransferResponseBase

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers
{
    public abstract class CoopFundsTransferResponseBase : CoopMessageBase
    {
        public string MessageID { get; set; }

        public string Currency { get; set; }

        public string CBReference { get; set; }

        public string CBMessageType { get; set; }

        public DateTime CBTransactionDate { get; set; }

        public string RequestUUID { get; set; }

        public string StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public bool Success { get; set; }

        public string ValidationStatus { get; set; }

        public string ValidationMessage { get; set; }
    }
}
