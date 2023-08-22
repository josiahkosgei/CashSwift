// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeBody

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeBody
    {

        private FundsTransfer fundsTransferField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
        public FundsTransfer FundsTransfer
        {
            get
            {
                return fundsTransferField;
            }
            set
            {
                fundsTransferField = value;
            }
        }
    }

}
