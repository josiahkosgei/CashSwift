// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferCreditPosting
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferCreditPosting
    {
        private string postingActionField;

        private FundsTransferCreditPostingAccount accountField;

        private FundsTransferCreditPostingCurrency currencyField;

        private DateTime valueDateField;

        private string userExtensionField;

        private string hostExtensionField;

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

        public FundsTransferCreditPostingAccount account
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

        public FundsTransferCreditPostingCurrency currency
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