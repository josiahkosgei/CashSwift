// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetailsLclCashAcctDetailsInputAccount
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetailsLclCashAcctDetailsInputAccount
    {
        private string inputAccountIdField;

        private string accountFormatTypeField;

        public string inputAccountId
        {
            get
            {
                return inputAccountIdField;
            }
            set
            {
                inputAccountIdField = value;
            }
        }

        public string accountFormatType
        {
            get
            {
                return accountFormatTypeField;
            }
            set
            {
                accountFormatTypeField = value;
            }
        }
    }
}