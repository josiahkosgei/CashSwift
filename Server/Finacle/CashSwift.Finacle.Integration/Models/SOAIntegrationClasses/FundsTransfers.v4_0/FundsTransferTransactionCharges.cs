// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeBody

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionCharges
    {

        private FundsTransferTransactionChargesCharge chargeField;

        /// <remarks/>
        public FundsTransferTransactionChargesCharge Charge
        {
            get
            {
                return chargeField;
            }
            set
            {
                chargeField = value;
            }
        }
    }

}
