// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferCreditPostingAccountPseudonym
using System.ComponentModel;
using System.Xml.Serialization;


namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferCreditPostingAccountPseudonym
    {
        private string pseudonymIdField;

        private string pseudonymTypeField;

        private string isoCurrencyCodeField;

        private string branchCodeField;

        private string contextTypeField;

        private string contextValueField;

        public string pseudonymId
        {
            get
            {
                return pseudonymIdField;
            }
            set
            {
                pseudonymIdField = value;
            }
        }

        public string pseudonymType
        {
            get
            {
                return pseudonymTypeField;
            }
            set
            {
                pseudonymTypeField = value;
            }
        }

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

        public string branchCode
        {
            get
            {
                return branchCodeField;
            }
            set
            {
                branchCodeField = value;
            }
        }

        public string contextType
        {
            get
            {
                return contextTypeField;
            }
            set
            {
                contextTypeField = value;
            }
        }

        public string contextValue
        {
            get
            {
                return contextValueField;
            }
            set
            {
                contextValueField = value;
            }
        }
    }
}