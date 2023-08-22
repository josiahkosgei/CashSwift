// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.CoopValidateReferenceAccountNumberResponse
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses
{
    public class CoopValidateReferenceAccountNumberResponse : CoopMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(CoopValidateReferenceAccountNumberResponse));

        internal new static XmlSerializer Serializer => _serializer;
    }
}