// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnFXDataSellEquivalent
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnFXDataSellEquivalent
    {
        private string isoCurrencyCodeField;

        private decimal amountField;

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

        public decimal amount
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