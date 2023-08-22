// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferAtmMessageHeaderAtmMsgType
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferAtmMessageHeaderAtmMsgType
    {
        private string messageClassField;

        private string messageFunctionField;

        private string txnOriginatorField;

        public string messageClass
        {
            get
            {
                return messageClassField;
            }
            set
            {
                messageClassField = value;
            }
        }

        public string messageFunction
        {
            get
            {
                return messageFunctionField;
            }
            set
            {
                messageFunctionField = value;
            }
        }

        public string txnOriginator
        {
            get
            {
                return txnOriginatorField;
            }
            set
            {
                txnOriginatorField = value;
            }
        }
    }
}