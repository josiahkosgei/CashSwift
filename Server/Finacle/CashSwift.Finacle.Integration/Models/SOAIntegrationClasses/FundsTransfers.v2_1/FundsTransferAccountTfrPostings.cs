// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferAccountTfrPostings
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferAccountTfrPostings
    {
        private string postingActionField;

        private FundsTransferAccountTfrPostingsAccount accountField;

        private FundsTransferAccountTfrPostingsCurrency currencyField;

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

        public FundsTransferAccountTfrPostingsAccount account
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

        public FundsTransferAccountTfrPostingsCurrency currency
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
