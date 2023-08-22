// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnFXData
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnFXData
    {
        private FundsTransferTxnFXDataBuyEquivalent buyEquivalentField;

        private FundsTransferTxnFXDataSellEquivalent sellEquivalentField;

        private bool buyAmtEnteredField;

        private FundsTransferTxnFXDataExhangeRateDetails exhangeRateDetailsField;

        private FundsTransferTxnFXDataLocalRoundingDetails localRoundingDetailsField;

        private string userExtensionField;

        private string hostExtensionField;

        public FundsTransferTxnFXDataBuyEquivalent buyEquivalent
        {
            get
            {
                return buyEquivalentField;
            }
            set
            {
                buyEquivalentField = value;
            }
        }

        public FundsTransferTxnFXDataSellEquivalent sellEquivalent
        {
            get
            {
                return sellEquivalentField;
            }
            set
            {
                sellEquivalentField = value;
            }
        }

        public bool buyAmtEntered
        {
            get
            {
                return buyAmtEnteredField;
            }
            set
            {
                buyAmtEnteredField = value;
            }
        }

        public FundsTransferTxnFXDataExhangeRateDetails exhangeRateDetails
        {
            get
            {
                return exhangeRateDetailsField;
            }
            set
            {
                exhangeRateDetailsField = value;
            }
        }

        public FundsTransferTxnFXDataLocalRoundingDetails localRoundingDetails
        {
            get
            {
                return localRoundingDetailsField;
            }
            set
            {
                localRoundingDetailsField = value;
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