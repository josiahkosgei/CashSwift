// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.EnvelopeHeader
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class EnvelopeHeader
    {
        private HeaderReply headerReplyField;

        [XmlElement(Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader")]
        public HeaderReply HeaderReply
        {
            get
            {
                return headerReplyField;
            }
            set
            {
                headerReplyField = value;
            }
        }
    }
}