// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnFXDataLocalRoundingDetails
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnFXDataLocalRoundingDetails
    {
        private FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetails localCashDetailsField;

        private FundsTransferTxnFXDataLocalRoundingDetailsLocalEquivDetails localEquivDetailsField;

        private FundsTransferTxnFXDataLocalRoundingDetailsLclChargeDetails lclChargeDetailsField;

        public FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetails localCashDetails
        {
            get
            {
                return localCashDetailsField;
            }
            set
            {
                localCashDetailsField = value;
            }
        }

        public FundsTransferTxnFXDataLocalRoundingDetailsLocalEquivDetails localEquivDetails
        {
            get
            {
                return localEquivDetailsField;
            }
            set
            {
                localEquivDetailsField = value;
            }
        }

        public FundsTransferTxnFXDataLocalRoundingDetailsLclChargeDetails lclChargeDetails
        {
            get
            {
                return lclChargeDetailsField;
            }
            set
            {
                lclChargeDetailsField = value;
            }
        }
    }
}