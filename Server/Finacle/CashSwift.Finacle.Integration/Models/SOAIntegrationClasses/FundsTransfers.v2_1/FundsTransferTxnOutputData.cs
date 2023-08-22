// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnOutputData
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnOutputData
    {
        private string hostTransactionRefField;

        private string userExtensionField;

        private string hostExtensionField;

        public string hostTransactionRef
        {
            get
            {
                return hostTransactionRefField;
            }
            set
            {
                hostTransactionRefField = value;
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