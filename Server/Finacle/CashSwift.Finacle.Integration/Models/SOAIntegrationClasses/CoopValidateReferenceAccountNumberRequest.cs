// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.CoopValidateReferenceAccountNumberRequest

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses
{
    public class CoopValidateReferenceAccountNumberRequest : CoopMessageBase
    {
        public new string RawXML { get; }

        private string RequestUUID { get; }

        private DateTime MessageDateTime { get; }

        public CoopValidateReferenceAccountNumberRequest()
        {
        }

        public CoopValidateReferenceAccountNumberRequest(string requestUUID, DateTime messageDateTime)
        {
            RequestUUID = requestUUID;
            MessageDateTime = messageDateTime;
            RawXML = "";
        }
    }
}