// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnFXDataExhangeRateDetails
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnFXDataExhangeRateDetails
    {
        private string exchangeRateField;

        private string multiplyDivideField;

        private string exchangeRateTypeField;

        private string userExtensionField;

        private string hostExtensionField;

        public string exchangeRate
        {
            get
            {
                return exchangeRateField;
            }
            set
            {
                exchangeRateField = value;
            }
        }

        public string multiplyDivide
        {
            get
            {
                return multiplyDivideField;
            }
            set
            {
                multiplyDivideField = value;
            }
        }

        public string exchangeRateType
        {
            get
            {
                return exchangeRateTypeField;
            }
            set
            {
                exchangeRateTypeField = value;
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