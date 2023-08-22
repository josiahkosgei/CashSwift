// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetails
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetails
    {
        private bool lclCashReceivedField;

        private FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetailsLclCashAmtDetails lclCashAmtDetailsField;

        private FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetailsLclCashAcctDetails lclCashAcctDetailsField;

        private string userExtensionField;

        private string hostExtensionField;

        public bool lclCashReceived
        {
            get
            {
                return lclCashReceivedField;
            }
            set
            {
                lclCashReceivedField = value;
            }
        }

        public FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetailsLclCashAmtDetails lclCashAmtDetails
        {
            get
            {
                return lclCashAmtDetailsField;
            }
            set
            {
                lclCashAmtDetailsField = value;
            }
        }

        public FundsTransferTxnFXDataLocalRoundingDetailsLocalCashDetailsLclCashAcctDetails lclCashAcctDetails
        {
            get
            {
                return lclCashAcctDetailsField;
            }
            set
            {
                lclCashAcctDetailsField = value;
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