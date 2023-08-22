// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferChargesChargeChargeRecAcctDetails
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferChargesChargeChargeRecAcctDetails
    {
        private string standardAccountIdField;

        private FundsTransferChargesChargeChargeRecAcctDetailsPseudonym pseudonymField;

        private string externalAccountIdField;

        private string iBANField;

        private FundsTransferChargesChargeChargeRecAcctDetailsInputAccount inputAccountField;

        private string userExtensionField;

        private string hostExtensionField;

        public string standardAccountId
        {
            get
            {
                return standardAccountIdField;
            }
            set
            {
                standardAccountIdField = value;
            }
        }

        public FundsTransferChargesChargeChargeRecAcctDetailsPseudonym pseudonym
        {
            get
            {
                return pseudonymField;
            }
            set
            {
                pseudonymField = value;
            }
        }

        public string externalAccountId
        {
            get
            {
                return externalAccountIdField;
            }
            set
            {
                externalAccountIdField = value;
            }
        }

        public string IBAN
        {
            get
            {
                return iBANField;
            }
            set
            {
                iBANField = value;
            }
        }

        public FundsTransferChargesChargeChargeRecAcctDetailsInputAccount inputAccount
        {
            get
            {
                return inputAccountField;
            }
            set
            {
                inputAccountField = value;
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
