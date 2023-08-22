// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnFXDataLocalRoundingDetailsLclChargeDetails
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnFXDataLocalRoundingDetailsLclChargeDetails
    {
        private FundsTransferTxnFXDataLocalRoundingDetailsLclChargeDetailsLclChgAmtDetails lclChgAmtDetailsField;

        private FundsTransferTxnFXDataLocalRoundingDetailsLclChargeDetailsLclChgChgAcct lclChgChgAcctField;

        private string userExtensionField;

        private string hostExtensionField;

        public FundsTransferTxnFXDataLocalRoundingDetailsLclChargeDetailsLclChgAmtDetails lclChgAmtDetails
        {
            get
            {
                return lclChgAmtDetailsField;
            }
            set
            {
                lclChgAmtDetailsField = value;
            }
        }

        public FundsTransferTxnFXDataLocalRoundingDetailsLclChargeDetailsLclChgChgAcct lclChgChgAcct
        {
            get
            {
                return lclChgChgAcctField;
            }
            set
            {
                lclChgChgAcctField = value;
            }
        }

        public string userExtension
        {
            get
            {
                return userExtensionField;
            }
            set
            {
                userExtensionField = value;
            }
        }

        public string hostExtension
        {
            get
            {
                return hostExtensionField;
            }
            set
            {
                hostExtensionField = value;
            }
        }
    }
}