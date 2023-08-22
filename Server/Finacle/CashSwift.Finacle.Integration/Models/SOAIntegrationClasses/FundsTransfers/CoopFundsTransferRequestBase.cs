// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.CoopFundsTransferRequestBase

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers
{
    public class CoopFundsTransferRequestBase : CoopMessageBase
    {
        public new string RawXML { get; set; }

        protected string RequestUUID { get; set; }

        protected DateTime MessageDateTime { get; set; }
    }
}
