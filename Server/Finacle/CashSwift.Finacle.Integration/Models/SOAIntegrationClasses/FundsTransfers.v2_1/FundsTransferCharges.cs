// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferCharges
using System.ComponentModel;
using System.Xml.Serialization;


namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferCharges
    {
        private string chargeClassificationField;

        private FundsTransferChargesCharge chargeField;

        public string chargeClassification
        {
            get
            {
                return chargeClassificationField;
            }
            set
            {
                chargeClassificationField = value;
            }
        }

        public FundsTransferChargesCharge charge
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