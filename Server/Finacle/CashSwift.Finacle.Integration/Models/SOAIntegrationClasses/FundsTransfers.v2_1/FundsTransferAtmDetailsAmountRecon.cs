// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferAtmDetailsAmountRecon
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferAtmDetailsAmountRecon
    {
        private string isoCurrencyCodeField;

        private string amountField;

        public string isoCurrencyCode
        {
            get
            {
                return isoCurrencyCodeField;
            }
            set
            {
                isoCurrencyCodeField = value;
            }
        }

        public string amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }
    }
}
