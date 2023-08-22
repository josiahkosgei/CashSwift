// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferChargesCharge
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferChargesCharge
    {
        private FundsTransferChargesChargeChargeCcyAmtDetails chargeCcyAmtDetailsField;

        private FundsTransferChargesChargeFundingAcctCcyDetails fundingAcctCcyDetailsField;

        private FundsTransferChargesChargeChargeExRateDetails chargeExRateDetailsField;

        private string chargePostingTxnCodeField;

        private string chargeNarrativeField;

        private FundsTransferChargesChargeChargeRecAcctDetails chargeRecAcctDetailsField;

        private FundsTransferChargesChargeFundingAccount fundingAccountField;

        private string taxCodeField;

        private FundsTransferChargesChargeTaxFndAcctAmtDetails taxFndAcctAmtDetailsField;

        private FundsTransferChargesChargeTaxCcyAmtDetails taxCcyAmtDetailsField;

        private FundsTransferChargesChargeTaxExchangeRateDetails taxExchangeRateDetailsField;

        private string taxNarrativeField;

        private FundsTransferChargesChargeTaxRecAcct taxRecAcctField;

        private string userExtensionField;

        private string hostExtensionField;

        public FundsTransferChargesChargeChargeCcyAmtDetails chargeCcyAmtDetails
        {
            get
            {
                return chargeCcyAmtDetailsField;
            }
            set
            {
                chargeCcyAmtDetailsField = value;
            }
        }

        public FundsTransferChargesChargeFundingAcctCcyDetails fundingAcctCcyDetails
        {
            get
            {
                return fundingAcctCcyDetailsField;
            }
            set
            {
                fundingAcctCcyDetailsField = value;
            }
        }

        public FundsTransferChargesChargeChargeExRateDetails chargeExRateDetails
        {
            get
            {
                return chargeExRateDetailsField;
            }
            set
            {
                chargeExRateDetailsField = value;
            }
        }

        public string chargePostingTxnCode
        {
            get
            {
                return chargePostingTxnCodeField;
            }
            set
            {
                chargePostingTxnCodeField = value;
            }
        }

        public string chargeNarrative
        {
            get
            {
                return chargeNarrativeField;
            }
            set
            {
                chargeNarrativeField = value;
            }
        }

        public FundsTransferChargesChargeChargeRecAcctDetails chargeRecAcctDetails
        {
            get
            {
                return chargeRecAcctDetailsField;
            }
            set
            {
                chargeRecAcctDetailsField = value;
            }
        }

        public FundsTransferChargesChargeFundingAccount fundingAccount
        {
            get
            {
                return fundingAccountField;
            }
            set
            {
                fundingAccountField = value;
            }
        }

        public string taxCode
        {
            get
            {
                return taxCodeField;
            }
            set
            {
                taxCodeField = value;
            }
        }

        public FundsTransferChargesChargeTaxFndAcctAmtDetails taxFndAcctAmtDetails
        {
            get
            {
                return taxFndAcctAmtDetailsField;
            }
            set
            {
                taxFndAcctAmtDetailsField = value;
            }
        }

        public FundsTransferChargesChargeTaxCcyAmtDetails taxCcyAmtDetails
        {
            get
            {
                return taxCcyAmtDetailsField;
            }
            set
            {
                taxCcyAmtDetailsField = value;
            }
        }

        public FundsTransferChargesChargeTaxExchangeRateDetails taxExchangeRateDetails
        {
            get
            {
                return taxExchangeRateDetailsField;
            }
            set
            {
                taxExchangeRateDetailsField = value;
            }
        }

        public string taxNarrative
        {
            get
            {
                return taxNarrativeField;
            }
            set
            {
                taxNarrativeField = value;
            }
        }

        public FundsTransferChargesChargeTaxRecAcct taxRecAcct
        {
            get
            {
                return taxRecAcctField;
            }
            set
            {
                taxRecAcctField = value;
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