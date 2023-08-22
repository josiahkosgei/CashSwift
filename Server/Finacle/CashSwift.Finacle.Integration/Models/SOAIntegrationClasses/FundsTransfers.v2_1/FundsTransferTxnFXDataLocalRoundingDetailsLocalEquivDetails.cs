// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnFXDataLocalRoundingDetailsLocalEquivDetails
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnFXDataLocalRoundingDetailsLocalEquivDetails
    {
        private FundsTransferTxnFXDataLocalRoundingDetailsLocalEquivDetailsEquivCcyAmtDetails equivCcyAmtDetailsField;

        private FundsTransferTxnFXDataLocalRoundingDetailsLocalEquivDetailsLclCcyAmtDetails lclCcyAmtDetailsField;

        private string lclExchRateDetailsField;

        private string userExtensionField;

        private string hostExtensionField;

        public FundsTransferTxnFXDataLocalRoundingDetailsLocalEquivDetailsEquivCcyAmtDetails equivCcyAmtDetails
        {
            get
            {
                return equivCcyAmtDetailsField;
            }
            set
            {
                equivCcyAmtDetailsField = value;
            }
        }

        public FundsTransferTxnFXDataLocalRoundingDetailsLocalEquivDetailsLclCcyAmtDetails lclCcyAmtDetails
        {
            get
            {
                return lclCcyAmtDetailsField;
            }
            set
            {
                lclCcyAmtDetailsField = value;
            }
        }

        public string lclExchRateDetails
        {
            get
            {
                return lclExchRateDetailsField;
            }
            set
            {
                lclExchRateDetailsField = value;
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