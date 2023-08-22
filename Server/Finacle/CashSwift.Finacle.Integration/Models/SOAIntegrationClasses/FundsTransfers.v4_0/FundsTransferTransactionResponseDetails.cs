// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeBody

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionResponseDetails
    {

        private string remarksField;

        /// <remarks/>
        public string Remarks
        {
            get
            {
                return remarksField;
            }
            set
            {
                remarksField = value;
            }
        }
    }

}
