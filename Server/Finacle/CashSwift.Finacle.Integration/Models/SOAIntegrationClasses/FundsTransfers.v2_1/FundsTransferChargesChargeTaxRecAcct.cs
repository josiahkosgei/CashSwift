// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferChargesChargeTaxRecAcct
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferChargesChargeTaxRecAcct
    {
        private string standardAccountIdField;

        private FundsTransferChargesChargeTaxRecAcctPseudonym pseudonymField;

        private string externalAccountIdField;

        private string iBANField;

        private FundsTransferChargesChargeTaxRecAcctInputAccount inputAccountField;

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

        public FundsTransferChargesChargeTaxRecAcctPseudonym pseudonym
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

        public FundsTransferChargesChargeTaxRecAcctInputAccount inputAccount
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