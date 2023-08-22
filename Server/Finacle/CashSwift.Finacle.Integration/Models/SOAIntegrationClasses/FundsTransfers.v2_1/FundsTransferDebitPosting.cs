// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferDebitPosting
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferDebitPosting
    {
        private string postingActionField;

        private string standardAccountIdField;

        private string externalAccountIdField;

        private string userExtensionField;

        private string hostExtensionField;

        private FundsTransferDebitPostingAccount accountField;

        private FundsTransferDebitPostingCurrency currencyField;

        private DateTime valueDateField;

        public string postingAction
        {
            get
            {
                return postingActionField;
            }
            set
            {
                postingActionField = value;
            }
        }

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

        public FundsTransferDebitPostingAccount account
        {
            get
            {
                return accountField;
            }
            set
            {
                accountField = value;
            }
        }

        public FundsTransferDebitPostingCurrency currency
        {
            get
            {
                return currencyField;
            }
            set
            {
                currencyField = value;
            }
        }

        public DateTime valueDate
        {
            get
            {
                return valueDateField;
            }
            set
            {
                valueDateField = value;
            }
        }
    }
}