// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeHeader

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeHeader
    {

        private ResponseHeader responseHeaderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
        public ResponseHeader ResponseHeader
        {
            get
            {
                return responseHeaderField;
            }
            set
            {
                responseHeaderField = value;
            }
        }
    }

}