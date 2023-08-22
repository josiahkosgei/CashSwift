// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.EnvelopeBody
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class EnvelopeBody
    {
        private DataOutput dataOutputField;

        [XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/Account/Service/AccountFundsTransfer/Post/2.1/DataIO")]
        public DataOutput DataOutput
        {
            get
            {
                return dataOutputField;
            }
            set
            {
                dataOutputField = value;
            }
        }
    }
}