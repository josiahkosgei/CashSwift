// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.EnvelopeBody
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class EnvelopeBody
    {
        [XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/2.5")]
        public FundsTransferResponse FundsTransferResponse { get; set; }
    }
}
